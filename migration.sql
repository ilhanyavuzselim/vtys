
-- Table: public.Kisiler

-- DROP TABLE IF EXISTS public."Kisiler";

CREATE TABLE IF NOT EXISTS public."Kisiler"
(
    "Id" uuid NOT NULL,
    "Ad" text COLLATE pg_catalog."default" NOT NULL,
    "Soyad" text COLLATE pg_catalog."default" NOT NULL,
    "Discriminator" character varying(8) COLLATE pg_catalog."default",
    CONSTRAINT "PK_Kisiler" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Kisiler"
    OWNER to vtys;

-- Table: public.Musteriler

-- DROP TABLE IF EXISTS public."Musteriler";

CREATE TABLE IF NOT EXISTS public."Musteriler"
(
    -- Inherited from table public."Kisiler": "Id" uuid NOT NULL,
    -- Inherited from table public."Kisiler": "Ad" text COLLATE pg_catalog."default" NOT NULL,
    -- Inherited from table public."Kisiler": "Soyad" text COLLATE pg_catalog."default" NOT NULL,
    -- Inherited from table public."Kisiler": "Discriminator" character varying(8) COLLATE pg_catalog."default",
    "MusteriId" uuid NOT NULL,
    "Telefon" text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "PK_Musteriler" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Musteriler_Kisiler_Id" FOREIGN KEY ("Id")
        REFERENCES public."Kisiler" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)
    INHERITS (public."Kisiler")

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Musteriler"
    OWNER to vtys;

-- Table: public.Personeller

-- DROP TABLE IF EXISTS public."Personeller";

CREATE TABLE IF NOT EXISTS public."Personeller"
(
    -- Inherited from table public."Kisiler": "Id" uuid NOT NULL,
    -- Inherited from table public."Kisiler": "Ad" text COLLATE pg_catalog."default" NOT NULL,
    -- Inherited from table public."Kisiler": "Soyad" text COLLATE pg_catalog."default" NOT NULL,
    -- Inherited from table public."Kisiler": "Discriminator" character varying(8) COLLATE pg_catalog."default",
    "PersonelId" uuid NOT NULL,
    "Pozisyon" text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "PK_Personeller" PRIMARY KEY ("Id")
)
    INHERITS (public."Kisiler")

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Personeller"
    OWNER to vtys;



BEGIN
    -- 1. Kisiler tablosunda mevcut ki�i kayd�n� g�ncelle
    UPDATE public."Kisiler"
    SET "Discriminator" = 'Personel'
    WHERE "Id" = p_kisi_id;

    -- 2. Personeller tablosuna yeni personel kayd�n� ekle
    INSERT INTO public."Personeller"(
        "Id", "Ad", "Soyad", "Discriminator", "PersonelId", "Pozisyon")
    VALUES (
        p_kisi_id,  -- Ki�inin ID'sini personel ID'si olarak kullan
        (SELECT "Ad" FROM public."Kisiler" WHERE "Id" = p_kisi_id LIMIT 1),
        (SELECT "Soyad" FROM public."Kisiler" WHERE "Id" = p_kisi_id LIMIT 1),
        'Personel',  -- Discriminator de�eri
        p_kisi_id,  -- PersonelId, Kisiler tablosundaki ki�inin ID'si
        p_pozisyon  -- Pozisyon parametre olarak al�nd�
    );
END;


BEGIN
    -- Kisiler tablosuna yeni ki�i kayd�n� ekle
    INSERT INTO public."Kisiler"(
        "Id", "Ad", "Soyad", "Discriminator"
    )
    VALUES (gen_random_uuid(), p_ad, p_soyad, 'Ki�i');
END;



DECLARE
    new_id UUID;  -- Yeni olu�turulan Id'yi saklamak i�in de�i�ken
BEGIN
    -- Kisiler tablosuna yeni ki�i kayd�n� ekle
    INSERT INTO public."Kisiler"(
        "Id", "Ad", "Soyad", "Discriminator"
    )
    VALUES (gen_random_uuid(), p_ad, p_soyad, 'Ki�i')
    RETURNING "Id" INTO new_id;  -- Yeni olu�turulan Id'yi 'new_id' de�i�kenine al

    -- create_personel_via_Existed_kisi prosed�r�n� �a��r
    CALL create_personel_via_Existed_kisi(new_id, p_pozisyon);  -- Yeni Id ve pozisyonu g�nder
END;



BEGIN
    -- Kisiler tablosunda ilgili ki�iyi g�ncelle
    UPDATE public."Kisiler"
    SET 
        "Ad" = p_ad,
        "Soyad" = p_soyad
    WHERE "Id" = p_id;

    -- Personel tablosundaki pozisyon bilgisini g�ncelle
    UPDATE public."Personeller"
    SET "Pozisyon" = p_pozisyon
    WHERE "Id" = p_id;

END;


BEGIN
    -- Personel kayd� silindi�inde, Kisiler tablosundaki ilgili kayd�n Discriminator'�n� 'Ki�i' yap
    UPDATE public."Kisiler"
    SET "Discriminator" = 'Ki�i'
    WHERE "Id" = OLD."Id";  -- Silinen kayd�n ID'sine g�re g�ncelleme yap�yoruz
    RETURN OLD;  -- Trigger'�n i�lem sonunda silinen kayd� d�nd�rmesi gerekiyor
END;


CREATE OR REPLACE TRIGGER trigger_update_kisiler_discriminator
    AFTER DELETE
    ON public."Personeller"
    FOR EACH ROW
    EXECUTE FUNCTION public.update_kisiler_discriminator_on_delete();


BEGIN
    -- Giderler tablosuna yeni bir gider kaydı ekleme
    INSERT INTO public."Giderler" ("Id", "Ad", "Tutar", "Tarih")
    VALUES (
        gen_random_uuid(),  -- UUID oluşturulacak
        'Tedarik Siparişi',  -- Gider adı
        NEW."BirimFiyat" * NEW."Miktar",  -- BirimFiyat ve Miktar'ı çarparak gider tutarını hesapla
        CURRENT_TIMESTAMP  -- Geçerli zaman
    );
    
    RETURN NEW;  -- Yeni kaydın eklenmesine izin verir
END;

BEGIN
    UPDATE public."Masalar"
    SET "Durum" = false
    WHERE "Id" = NEW."MasaID";
    RETURN NEW;
END;

BEGIN
    DELETE FROM public."Rezervasyonlar" WHERE "MusteriID" = OLD."Id";
    DELETE FROM public."Siparisler" WHERE "MusteriID" = OLD."Id";
    RETURN OLD;
END;

BEGIN
    -- Stokta malzeme mevcut mu kontrol et
    IF EXISTS (SELECT 1 FROM public."Stoklar" WHERE "MalzemeID" = NEW."MalzemeID") THEN
        -- Eğer stokta varsa, mevcut miktarı artır
        UPDATE public."Stoklar"
        SET "Miktar" = "Miktar" + NEW."Miktar"
        WHERE "MalzemeID" = NEW."MalzemeID";
    ELSE
        -- Eğer stokta yoksa, yeni bir stok kaydı ekle
        INSERT INTO public."Stoklar" ("Id", "MalzemeID", "Miktar")
        VALUES (gen_random_uuid(), NEW."MalzemeID", NEW."Miktar");
    END IF;

    RETURN NEW;
END;

