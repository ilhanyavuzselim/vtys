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
    -- 1. Kisiler tablosunda mevcut kiþi kaydýný güncelle
    UPDATE public."Kisiler"
    SET "Discriminator" = 'Personel'
    WHERE "Id" = p_kisi_id;

    -- 2. Personeller tablosuna yeni personel kaydýný ekle
    INSERT INTO public."Personeller"(
        "Id", "Ad", "Soyad", "Discriminator", "PersonelId", "Pozisyon")
    VALUES (
        p_kisi_id,  -- Kiþinin ID'sini personel ID'si olarak kullan
        (SELECT "Ad" FROM public."Kisiler" WHERE "Id" = p_kisi_id LIMIT 1),
        (SELECT "Soyad" FROM public."Kisiler" WHERE "Id" = p_kisi_id LIMIT 1),
        'Personel',  -- Discriminator deðeri
        p_kisi_id,  -- PersonelId, Kisiler tablosundaki kiþinin ID'si
        p_pozisyon  -- Pozisyon parametre olarak alýndý
    );
END;


BEGIN
    -- Kisiler tablosuna yeni kiþi kaydýný ekle
    INSERT INTO public."Kisiler"(
        "Id", "Ad", "Soyad", "Discriminator"
    )
    VALUES (gen_random_uuid(), p_ad, p_soyad, 'Kiþi');
END;



DECLARE
    new_id UUID;  -- Yeni oluþturulan Id'yi saklamak için deðiþken
BEGIN
    -- Kisiler tablosuna yeni kiþi kaydýný ekle
    INSERT INTO public."Kisiler"(
        "Id", "Ad", "Soyad", "Discriminator"
    )
    VALUES (gen_random_uuid(), p_ad, p_soyad, 'Kiþi')
    RETURNING "Id" INTO new_id;  -- Yeni oluþturulan Id'yi 'new_id' deðiþkenine al

    -- create_personel_via_Existed_kisi prosedürünü çaðýr
    CALL create_personel_via_Existed_kisi(new_id, p_pozisyon);  -- Yeni Id ve pozisyonu gönder
END;



BEGIN
    -- Kisiler tablosunda ilgili kiþiyi güncelle
    UPDATE public."Kisiler"
    SET 
        "Ad" = p_ad,
        "Soyad" = p_soyad
    WHERE "Id" = p_id;

    -- Personel tablosundaki pozisyon bilgisini güncelle
    UPDATE public."Personeller"
    SET "Pozisyon" = p_pozisyon
    WHERE "Id" = p_id;

END;


BEGIN
    -- Personel kaydý silindiðinde, Kisiler tablosundaki ilgili kaydýn Discriminator'ýný 'Kiþi' yap
    UPDATE public."Kisiler"
    SET "Discriminator" = 'Kiþi'
    WHERE "Id" = OLD."Id";  -- Silinen kaydýn ID'sine göre güncelleme yapýyoruz
    RETURN OLD;  -- Trigger'ýn iþlem sonunda silinen kaydý döndürmesi gerekiyor
END;


CREATE OR REPLACE TRIGGER trigger_update_kisiler_discriminator
    AFTER DELETE
    ON public."Personeller"
    FOR EACH ROW
    EXECUTE FUNCTION public.update_kisiler_discriminator_on_delete();
