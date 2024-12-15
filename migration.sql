
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
