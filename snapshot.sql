--
-- PostgreSQL database cluster dump
--

-- Started on 2024-12-22 19:46:19

SET default_transaction_read_only = off;

SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;

--
-- Roles
--

CREATE ROLE vtys;
ALTER ROLE vtys WITH SUPERUSER INHERIT CREATEROLE CREATEDB LOGIN REPLICATION BYPASSRLS;

--
-- User Configurations
--








--
-- Databases
--

--
-- Database "template1" dump
--

\connect template1

--
-- PostgreSQL database dump
--

-- Dumped from database version 17.0 (Debian 17.0-1.pgdg120+1)
-- Dumped by pg_dump version 17.0

-- Started on 2024-12-22 19:46:19

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

-- Completed on 2024-12-22 19:46:20

--
-- PostgreSQL database dump complete
--

--
-- Database "postgres" dump
--

\connect postgres

--
-- PostgreSQL database dump
--

-- Dumped from database version 17.0 (Debian 17.0-1.pgdg120+1)
-- Dumped by pg_dump version 17.0

-- Started on 2024-12-22 19:46:20

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

-- Completed on 2024-12-22 19:46:20

--
-- PostgreSQL database dump complete
--

--
-- Database "vtysdb" dump
--

--
-- PostgreSQL database dump
--

-- Dumped from database version 17.0 (Debian 17.0-1.pgdg120+1)
-- Dumped by pg_dump version 17.0

-- Started on 2024-12-22 19:46:20

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 3520 (class 1262 OID 24678)
-- Name: vtysdb; Type: DATABASE; Schema: -; Owner: vtys
--

CREATE DATABASE vtysdb WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.utf8';


ALTER DATABASE vtysdb OWNER TO vtys;

\connect vtysdb

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 255 (class 1255 OID 49164)
-- Name: calculate_total_siparis_tutari(); Type: PROCEDURE; Schema: public; Owner: vtys
--

CREATE PROCEDURE public.calculate_total_siparis_tutari()
    LANGUAGE plpgsql
    AS $$
BEGIN
    -- Toplam satış miktarını hesaplama
    DECLARE
        toplam_satis NUMERIC;
    BEGIN
        SELECT COALESCE(SUM(sd."Adet"), 0)
        INTO toplam_satis
        FROM "SiparisDetaylar" sd;
        
        RAISE NOTICE 'Toplam Satış Miktarı: %', toplam_satis;
    END;
END;
$$;


ALTER PROCEDURE public.calculate_total_siparis_tutari() OWNER TO vtys;

--
-- TOC entry 236 (class 1255 OID 40991)
-- Name: create_musteri_via_existed_kisi(uuid, text); Type: PROCEDURE; Schema: public; Owner: vtys
--

CREATE PROCEDURE public.create_musteri_via_existed_kisi(IN p_kisi_id uuid, IN p_telefon text)
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO public."Musteriler"(
        "Id", "Ad", "Soyad", "Discriminator", "MusteriId", "Telefon")
    VALUES (
        p_kisi_id,  -- Kişinin ID'sini müşteri ID'si olarak kullan
        (SELECT "Ad" FROM public."Kisiler" WHERE "Id" = p_kisi_id LIMIT 1),
        (SELECT "Soyad" FROM public."Kisiler" WHERE "Id" = p_kisi_id LIMIT 1),
        'Müşteri',  -- Discriminator değeri
		p_kisi_id,
        p_telefon   -- Telefon, parametre olarak alındı
    );
END;
$$;


ALTER PROCEDURE public.create_musteri_via_existed_kisi(IN p_kisi_id uuid, IN p_telefon text) OWNER TO vtys;

--
-- TOC entry 258 (class 1255 OID 40967)
-- Name: create_personel_via_existed_kisi(uuid, text); Type: PROCEDURE; Schema: public; Owner: vtys
--

CREATE PROCEDURE public.create_personel_via_existed_kisi(IN p_kisi_id uuid, IN p_pozisyon text)
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO public."Personeller"(
        "Id", "Ad", "Soyad", "Discriminator", "PersonelId", "Pozisyon")
    VALUES (
        p_kisi_id,  -- Kişinin ID'sini personel ID'si olarak kullan
        (SELECT "Ad" FROM public."Kisiler" WHERE "Id" = p_kisi_id LIMIT 1),
        (SELECT "Soyad" FROM public."Kisiler" WHERE "Id" = p_kisi_id LIMIT 1),
        'Personel',  -- Discriminator değeri
        p_kisi_id,  -- PersonelId, Kisiler tablosundaki kişinin ID'si
        p_pozisyon  -- Pozisyon parametre olarak alındı
    );
END;
$$;


ALTER PROCEDURE public.create_personel_via_existed_kisi(IN p_kisi_id uuid, IN p_pozisyon text) OWNER TO vtys;

--
-- TOC entry 252 (class 1255 OID 49163)
-- Name: get_masalar_by_durum(); Type: PROCEDURE; Schema: public; Owner: vtys
--

CREATE PROCEDURE public.get_masalar_by_durum()
    LANGUAGE plpgsql
    AS $$
DECLARE
    record RECORD;  -- 'record' değişkenini doğru türde tanımlıyoruz
BEGIN
    -- Tüm masaların durumlarını getirme
    RAISE NOTICE 'Tüm masaların durumu:';
    FOR record IN 
        SELECT m."MasaNo", m."Durum"  -- Doğru sütun adlarını kullanıyoruz
        FROM "Masalar" m
    LOOP
        RAISE NOTICE 'Masa No: %, Durum: %', record."MasaNo", record."Durum";
    END LOOP;
END;
$$;


ALTER PROCEDURE public.get_masalar_by_durum() OWNER TO vtys;

--
-- TOC entry 256 (class 1255 OID 49165)
-- Name: get_most_valued_siparis(); Type: PROCEDURE; Schema: public; Owner: vtys
--

CREATE PROCEDURE public.get_most_valued_siparis()
    LANGUAGE plpgsql
    AS $$
DECLARE
    record RECORD;  -- 'record' değişkenini doğru türde tanımlıyoruz
BEGIN
    -- En yüksek tutarlı siparişi bulma
    RAISE NOTICE 'En yüksek tutarlı sipariş:';
    FOR record IN 
        SELECT s."Id", COALESCE(SUM(sd."Adet" * m."Fiyat"), 0) AS "ToplamTutar"
        FROM "Siparisler" s
        JOIN "SiparisDetaylar" sd ON sd."SiparisID" = s."Id"
        JOIN "Menuler" m ON m."Id" = sd."MenuID"
        GROUP BY s."Id"
        ORDER BY "ToplamTutar" DESC
        LIMIT 1
    LOOP
        RAISE NOTICE 'Sipariş ID: %, Toplam Tutar: %', record."Id", record."ToplamTutar";
    END LOOP;
END;
$$;


ALTER PROCEDURE public.get_most_valued_siparis() OWNER TO vtys;

--
-- TOC entry 259 (class 1255 OID 49169)
-- Name: gider_ekle(); Type: FUNCTION; Schema: public; Owner: vtys
--

CREATE FUNCTION public.gider_ekle() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
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
$$;


ALTER FUNCTION public.gider_ekle() OWNER TO vtys;

--
-- TOC entry 233 (class 1255 OID 33231)
-- Name: insert_kisi(text, text); Type: PROCEDURE; Schema: public; Owner: vtys
--

CREATE PROCEDURE public.insert_kisi(IN p_ad text, IN p_soyad text)
    LANGUAGE plpgsql
    AS $$
BEGIN
    -- Kisiler tablosuna yeni kişi kaydını ekle
    INSERT INTO public."Kisiler"(
        "Id", "Ad", "Soyad", "Discriminator"
    )
    VALUES (gen_random_uuid(), p_ad, p_soyad, 'Kişi');
END;
$$;


ALTER PROCEDURE public.insert_kisi(IN p_ad text, IN p_soyad text) OWNER TO vtys;

--
-- TOC entry 254 (class 1255 OID 40965)
-- Name: insert_musteri(text, text, text); Type: PROCEDURE; Schema: public; Owner: vtys
--

CREATE PROCEDURE public.insert_musteri(IN p_ad text, IN p_soyad text, IN p_telefon text)
    LANGUAGE plpgsql
    AS $$
DECLARE
    new_id UUID;  -- Yeni oluşturulan Id'yi saklamak için değişken
BEGIN
    -- Kisiler tablosuna yeni kişi kaydını ekle
    INSERT INTO public."Kisiler"(
        "Id", "Ad", "Soyad", "Discriminator"
    )
    VALUES (gen_random_uuid(), p_ad, p_soyad, 'Kişi')
    RETURNING "Id" INTO new_id;  -- Yeni oluşturulan Id'yi 'new_id' değişkenine al

    -- create_musteri_via_existed_kisi prosedürünü çağır
    CALL create_musteri_via_existed_kisi(new_id, p_telefon);  -- Yeni Id ve telefon bilgisini gönder
END;
$$;


ALTER PROCEDURE public.insert_musteri(IN p_ad text, IN p_soyad text, IN p_telefon text) OWNER TO vtys;

--
-- TOC entry 238 (class 1255 OID 33232)
-- Name: insert_personel(text, text, text); Type: PROCEDURE; Schema: public; Owner: vtys
--

CREATE PROCEDURE public.insert_personel(IN p_ad text, IN p_soyad text, IN p_pozisyon text)
    LANGUAGE plpgsql
    AS $$
DECLARE
    new_id UUID;  -- Yeni oluşturulan Id'yi saklamak için değişken
BEGIN
    -- Kisiler tablosuna yeni kişi kaydını ekle
    INSERT INTO public."Kisiler"(
        "Id", "Ad", "Soyad", "Discriminator"
    )
    VALUES (gen_random_uuid(), p_ad, p_soyad, 'Kişi')
    RETURNING "Id" INTO new_id;  -- Yeni oluşturulan Id'yi 'new_id' değişkenine al

    -- create_personel_via_Existed_kisi prosedürünü çağır
    CALL create_personel_via_Existed_kisi(new_id, p_pozisyon);  -- Yeni Id ve pozisyonu gönder
END;
$$;


ALTER PROCEDURE public.insert_personel(IN p_ad text, IN p_soyad text, IN p_pozisyon text) OWNER TO vtys;

--
-- TOC entry 237 (class 1255 OID 49171)
-- Name: musteri_silme(); Type: FUNCTION; Schema: public; Owner: vtys
--

CREATE FUNCTION public.musteri_silme() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    DELETE FROM public."Rezervasyonlar" WHERE "MusteriID" = OLD."Id";
    DELETE FROM public."Siparisler" WHERE "MusteriID" = OLD."Id";
    RETURN OLD;
END;
$$;


ALTER FUNCTION public.musteri_silme() OWNER TO vtys;

--
-- TOC entry 235 (class 1255 OID 40989)
-- Name: remove_musteri(uuid); Type: PROCEDURE; Schema: public; Owner: vtys
--

CREATE PROCEDURE public.remove_musteri(IN p_id uuid)
    LANGUAGE plpgsql
    AS $$
BEGIN
    DELETE FROM public."Musteriler" WHERE "MusteriId" = p_id;
END;
$$;


ALTER PROCEDURE public.remove_musteri(IN p_id uuid) OWNER TO vtys;

--
-- TOC entry 234 (class 1255 OID 40988)
-- Name: remove_personel(uuid); Type: PROCEDURE; Schema: public; Owner: vtys
--

CREATE PROCEDURE public.remove_personel(IN p_id uuid)
    LANGUAGE plpgsql
    AS $$
BEGIN
    DELETE FROM public."Personeller" WHERE "PersonelId" = p_id;
END;
$$;


ALTER PROCEDURE public.remove_personel(IN p_id uuid) OWNER TO vtys;

--
-- TOC entry 240 (class 1255 OID 49175)
-- Name: stok_ekle(); Type: FUNCTION; Schema: public; Owner: vtys
--

CREATE FUNCTION public.stok_ekle() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO public."Stoklar"(
       "Id", "MalzemeID", "Miktar")
    VALUES (gen_random_uuid(),NEW."Id", 0);  -- Yeni malzeme kaydı eklendiğinde, miktar 0 olarak atanır
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.stok_ekle() OWNER TO vtys;

--
-- TOC entry 241 (class 1255 OID 49173)
-- Name: stok_guncelle(); Type: FUNCTION; Schema: public; Owner: vtys
--

CREATE FUNCTION public.stok_guncelle() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE public."Stoklar"
    SET "Miktar" = "Miktar" + NEW."Miktar"  -- Sipariş edilen miktarı stok miktarına ekle
    WHERE "MalzemeID" = NEW."MalzemeID";  -- İlgili malzeme kaydını bul
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.stok_guncelle() OWNER TO vtys;

--
-- TOC entry 257 (class 1255 OID 40968)
-- Name: update_musteri(uuid, text, text, text); Type: PROCEDURE; Schema: public; Owner: vtys
--

CREATE PROCEDURE public.update_musteri(IN p_id uuid, IN p_ad text, IN p_soyad text, IN p_telefon text)
    LANGUAGE plpgsql
    AS $$
BEGIN
    -- Kisiler tablosunda ilgili kişiyi güncelle
    UPDATE public."Kisiler"
    SET 
        "Ad" = p_ad,
        "Soyad" = p_soyad
    WHERE "Id" = p_id;

    -- Musteriler tablosundaki telefon bilgisini güncelle
    UPDATE public."Musteriler"
    SET 
        "Telefon" = p_telefon
    WHERE "Id" = p_id;

END;
$$;


ALTER PROCEDURE public.update_musteri(IN p_id uuid, IN p_ad text, IN p_soyad text, IN p_telefon text) OWNER TO vtys;

--
-- TOC entry 239 (class 1255 OID 33235)
-- Name: update_personel(uuid, text, text, text); Type: PROCEDURE; Schema: public; Owner: vtys
--

CREATE PROCEDURE public.update_personel(IN p_id uuid, IN p_ad text, IN p_soyad text, IN p_pozisyon text)
    LANGUAGE plpgsql
    AS $$
BEGIN
    -- Kisiler tablosunda ilgili kişiyi güncelle
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
$$;


ALTER PROCEDURE public.update_personel(IN p_id uuid, IN p_ad text, IN p_soyad text, IN p_pozisyon text) OWNER TO vtys;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 217 (class 1259 OID 32987)
-- Name: Giderler; Type: TABLE; Schema: public; Owner: vtys
--

CREATE TABLE public."Giderler" (
    "Id" uuid NOT NULL,
    "Ad" text NOT NULL,
    "Tutar" numeric NOT NULL,
    "Tarih" timestamp with time zone NOT NULL
);


ALTER TABLE public."Giderler" OWNER TO vtys;

--
-- TOC entry 218 (class 1259 OID 32994)
-- Name: Kategoriler; Type: TABLE; Schema: public; Owner: vtys
--

CREATE TABLE public."Kategoriler" (
    "Id" uuid NOT NULL,
    "Ad" text NOT NULL
);


ALTER TABLE public."Kategoriler" OWNER TO vtys;

--
-- TOC entry 219 (class 1259 OID 33001)
-- Name: Kisiler; Type: TABLE; Schema: public; Owner: vtys
--

CREATE TABLE public."Kisiler" (
    "Id" uuid NOT NULL,
    "Ad" text NOT NULL,
    "Soyad" text NOT NULL,
    "Discriminator" character varying(8)
);


ALTER TABLE public."Kisiler" OWNER TO vtys;

--
-- TOC entry 224 (class 1259 OID 33063)
-- Name: Malzemeler; Type: TABLE; Schema: public; Owner: vtys
--

CREATE TABLE public."Malzemeler" (
    "Id" uuid NOT NULL,
    "Ad" text NOT NULL,
    "TedarikciID" uuid NOT NULL
);


ALTER TABLE public."Malzemeler" OWNER TO vtys;

--
-- TOC entry 220 (class 1259 OID 33008)
-- Name: Masalar; Type: TABLE; Schema: public; Owner: vtys
--

CREATE TABLE public."Masalar" (
    "Id" uuid NOT NULL,
    "MasaNo" integer NOT NULL,
    "Kapasite" integer NOT NULL,
    "Durum" boolean NOT NULL
);


ALTER TABLE public."Masalar" OWNER TO vtys;

--
-- TOC entry 223 (class 1259 OID 33027)
-- Name: Menuler; Type: TABLE; Schema: public; Owner: vtys
--

CREATE TABLE public."Menuler" (
    "Id" uuid NOT NULL,
    "Ad" text NOT NULL,
    "Fiyat" numeric NOT NULL,
    "KategoriID" uuid NOT NULL
);


ALTER TABLE public."Menuler" OWNER TO vtys;

--
-- TOC entry 231 (class 1259 OID 33203)
-- Name: Musteriler; Type: TABLE; Schema: public; Owner: vtys
--

CREATE TABLE public."Musteriler" (
    "Id" uuid,
    "MusteriId" uuid NOT NULL,
    "Telefon" text NOT NULL
)
INHERITS (public."Kisiler");


ALTER TABLE public."Musteriler" OWNER TO vtys;

--
-- TOC entry 221 (class 1259 OID 33013)
-- Name: OdemeTurleri; Type: TABLE; Schema: public; Owner: vtys
--

CREATE TABLE public."OdemeTurleri" (
    "Id" uuid NOT NULL,
    "Ad" text NOT NULL
);


ALTER TABLE public."OdemeTurleri" OWNER TO vtys;

--
-- TOC entry 229 (class 1259 OID 33137)
-- Name: Odemeler; Type: TABLE; Schema: public; Owner: vtys
--

CREATE TABLE public."Odemeler" (
    "Id" uuid NOT NULL,
    "SiparisID" uuid NOT NULL,
    "OdemeTuruID" uuid NOT NULL,
    "Tutar" numeric NOT NULL
);


ALTER TABLE public."Odemeler" OWNER TO vtys;

--
-- TOC entry 232 (class 1259 OID 40992)
-- Name: Personeller; Type: TABLE; Schema: public; Owner: vtys
--

CREATE TABLE public."Personeller" (
    "Id" uuid,
    "PersonelId" uuid NOT NULL,
    "Pozisyon" text NOT NULL
)
INHERITS (public."Kisiler");


ALTER TABLE public."Personeller" OWNER TO vtys;

--
-- TOC entry 225 (class 1259 OID 33075)
-- Name: Rezervasyonlar; Type: TABLE; Schema: public; Owner: vtys
--

CREATE TABLE public."Rezervasyonlar" (
    "Id" uuid NOT NULL,
    "MasaID" uuid NOT NULL,
    "MusteriID" uuid NOT NULL,
    "RezervasyonTarihi" timestamp with time zone NOT NULL
);


ALTER TABLE public."Rezervasyonlar" OWNER TO vtys;

--
-- TOC entry 230 (class 1259 OID 33154)
-- Name: SiparisDetaylar; Type: TABLE; Schema: public; Owner: vtys
--

CREATE TABLE public."SiparisDetaylar" (
    "Id" uuid NOT NULL,
    "SiparisID" uuid NOT NULL,
    "MenuID" uuid NOT NULL,
    "Adet" integer NOT NULL,
    "OdendiMi" boolean DEFAULT false NOT NULL
);


ALTER TABLE public."SiparisDetaylar" OWNER TO vtys;

--
-- TOC entry 226 (class 1259 OID 33090)
-- Name: Siparisler; Type: TABLE; Schema: public; Owner: vtys
--

CREATE TABLE public."Siparisler" (
    "Id" uuid NOT NULL,
    "MasaID" uuid NOT NULL,
    "MusteriID" uuid,
    "PersonelID" uuid NOT NULL,
    "SiparisTarihi" timestamp with time zone NOT NULL,
    "OdendiMi" boolean DEFAULT false NOT NULL
);


ALTER TABLE public."Siparisler" OWNER TO vtys;

--
-- TOC entry 227 (class 1259 OID 33110)
-- Name: Stoklar; Type: TABLE; Schema: public; Owner: vtys
--

CREATE TABLE public."Stoklar" (
    "Id" uuid NOT NULL,
    "MalzemeID" uuid NOT NULL,
    "Miktar" integer NOT NULL
);


ALTER TABLE public."Stoklar" OWNER TO vtys;

--
-- TOC entry 228 (class 1259 OID 33120)
-- Name: TedarikSiparisleri; Type: TABLE; Schema: public; Owner: vtys
--

CREATE TABLE public."TedarikSiparisleri" (
    "Id" uuid NOT NULL,
    "TedarikciID" uuid NOT NULL,
    "MalzemeID" uuid NOT NULL,
    "BirimFiyat" numeric NOT NULL,
    "Miktar" numeric NOT NULL,
    "SiparisTarihi" timestamp with time zone NOT NULL
);


ALTER TABLE public."TedarikSiparisleri" OWNER TO vtys;

--
-- TOC entry 222 (class 1259 OID 33020)
-- Name: Tedarikciler; Type: TABLE; Schema: public; Owner: vtys
--

CREATE TABLE public."Tedarikciler" (
    "Id" uuid NOT NULL,
    "Ad" text NOT NULL,
    "Iletisim" text NOT NULL
);


ALTER TABLE public."Tedarikciler" OWNER TO vtys;

--
-- TOC entry 3499 (class 0 OID 32987)
-- Dependencies: 217
-- Data for Name: Giderler; Type: TABLE DATA; Schema: public; Owner: vtys
--

COPY public."Giderler" ("Id", "Ad", "Tutar", "Tarih") FROM stdin;
11111111-1111-1111-1111-111111111111	Elektrik	150.00	2024-12-01 09:00:00+00
11111111-1111-1111-1111-111111111112	Su	50.00	2024-12-01 09:00:00+00
11111111-1111-1111-1111-111111111113	Temizlik Malzemeleri	75.00	2024-12-02 09:00:00+00
11111111-1111-1111-1111-111111111114	Yemek Malzemeleri	500.00	2024-12-02 09:00:00+00
11111111-1111-1111-1111-111111111115	Bakım	200.00	2024-12-03 09:00:00+00
11111111-1111-1111-1111-111111111116	Sigorta	300.00	2024-12-03 09:00:00+00
11111111-1111-1111-1111-111111111117	Kira	1000.00	2024-12-04 09:00:00+00
11111111-1111-1111-1111-111111111118	Aydınlatma	120.00	2024-12-04 09:00:00+00
11111111-1111-1111-1111-111111111119	Temizlik Ücretleri	80.00	2024-12-05 09:00:00+00
11111111-1111-1111-1111-111111111120	Personel Ücreti	1500.00	2024-12-05 09:00:00+00
27d53022-dbd8-40ce-a1cb-b63404b862f4	Tedarik Siparişi	450	2024-12-20 13:52:20.741776+00
9732e49f-8010-49bb-8c77-c4638b6efa8d	Tedarik Siparişi	900	2024-12-20 14:19:43.227732+00
0193e923-556b-70cf-8dc1-687054a0fbaa	Maaş Ödemesi	20000	2024-12-21 12:13:37.202+00
\.


--
-- TOC entry 3500 (class 0 OID 32994)
-- Dependencies: 218
-- Data for Name: Kategoriler; Type: TABLE DATA; Schema: public; Owner: vtys
--

COPY public."Kategoriler" ("Id", "Ad") FROM stdin;
22222222-2222-2222-2222-222222222221	Ana Yemek
22222222-2222-2222-2222-222222222222	İçecekler
22222222-2222-2222-2222-222222222223	Tatlılar
22222222-2222-2222-2222-222222222224	Başlangıçlar
22222222-2222-2222-2222-222222222225	Salatalar
22222222-2222-2222-2222-222222222226	Çorbalar
22222222-2222-2222-2222-222222222227	Atıştırmalıklar
22222222-2222-2222-2222-222222222228	Dondurmalar
22222222-2222-2222-2222-222222222229	İçecekler Alkollü
22222222-2222-2222-2222-222222222230	Şaraplar
\.


--
-- TOC entry 3501 (class 0 OID 33001)
-- Dependencies: 219
-- Data for Name: Kisiler; Type: TABLE DATA; Schema: public; Owner: vtys
--

COPY public."Kisiler" ("Id", "Ad", "Soyad", "Discriminator") FROM stdin;
8b0d6eeb-a1e2-46fc-9660-1e16d92acca2	Murat	Öztürk	Kişi
4be35515-f626-4c97-8261-c19b5c12af11	Yıldıray Ali	Kara	Kişi
ed579a84-16bf-45e9-b940-0fc2a257fc1a	Doğan	Kaya	Kişi
dd00912e-f645-4fee-9040-f1ff97ca7cc7	Yavuz	Selim İlhan	Kişi
e420d8c4-4521-416a-94ca-29439394d620	Metehan	Öztürk	Kişi
1a683e1f-f06e-4439-91ce-fdb8fdad9d67	Celal	Çeken	Kişi
184ae6b5-40f8-482e-b2b8-184256beec67	Ayşe	Korkmaz	Kişi
65c4beb1-1830-4da7-ad59-e6dee737cf8e	Elif	Çakır	Kişi
2d310725-3ab6-4795-b8b2-ebd9e3cb6f27	Buse	Güçlü	Kişi
6f0bd5ee-4b37-47b9-b171-dc566ac8d72a	Aslı	Han	Kişi
0d770138-49c8-412b-8608-91890205985e	Melek	Çelik	Kişi
d9dc2a86-9a4d-4c65-b014-f72ebf5cf398	Tuğçe	Özsoy	Kişi
\.


--
-- TOC entry 3506 (class 0 OID 33063)
-- Dependencies: 224
-- Data for Name: Malzemeler; Type: TABLE DATA; Schema: public; Owner: vtys
--

COPY public."Malzemeler" ("Id", "Ad", "TedarikciID") FROM stdin;
22222222-2222-2222-2222-222222222211	Malzeme A	11111111-1111-1111-1111-111111111111
22222222-2222-2222-2222-222222222212	Malzeme B	11111111-1111-1111-1111-111111111112
22222222-2222-2222-2222-222222222213	Malzeme C	11111111-1111-1111-1111-111111111113
22222222-2222-2222-2222-222222222214	Malzeme D	11111111-1111-1111-1111-111111111114
22222222-2222-2222-2222-222222222215	Malzeme E	11111111-1111-1111-1111-111111111115
22222222-2222-2222-2222-222222222216	Malzeme F	11111111-1111-1111-1111-111111111116
22222222-2222-2222-2222-222222222217	Malzeme G	11111111-1111-1111-1111-111111111117
22222222-2222-2222-2222-222222222218	Malzeme H	11111111-1111-1111-1111-111111111118
22222222-2222-2222-2222-222222222219	Malzeme I	11111111-1111-1111-1111-111111111119
22222222-2222-2222-2222-222222222220	Malzeme J	11111111-1111-1111-1111-111111111120
22222222-2222-2222-2222-222222222230	Malzeme K	11111111-1111-1111-1111-111111111111
\.


--
-- TOC entry 3502 (class 0 OID 33008)
-- Dependencies: 220
-- Data for Name: Masalar; Type: TABLE DATA; Schema: public; Owner: vtys
--

COPY public."Masalar" ("Id", "MasaNo", "Kapasite", "Durum") FROM stdin;
55555555-5555-5555-5555-555555555560	10	2	t
0193e942-8e52-7d3b-8525-45b8cbdc17bb	15	4	t
0193e941-f2a2-782f-8c62-af883567321a	12	5	t
0193e942-2896-7fbe-9ffe-66b7015cb307	11	5	t
0193e942-402e-7506-8829-8d6f1ab3701b	13	8	t
0193e942-6849-7f7b-a03a-f3982651e15d	14	5	t
55555555-5555-5555-5555-555555555555	5	4	t
55555555-5555-5555-5555-555555555556	6	2	t
55555555-5555-5555-5555-555555555557	7	8	t
55555555-5555-5555-5555-555555555558	8	4	t
55555555-5555-5555-5555-555555555559	9	6	t
55555555-5555-5555-5555-555555555551	1	4	f
55555555-5555-5555-5555-555555555552	2	2	f
55555555-5555-5555-5555-555555555553	3	6	f
0193d37c-3c20-709f-835c-186a70e17552	0	0	t
55555555-5555-5555-5555-555555555554	4	4	f
\.


--
-- TOC entry 3505 (class 0 OID 33027)
-- Dependencies: 223
-- Data for Name: Menuler; Type: TABLE DATA; Schema: public; Owner: vtys
--

COPY public."Menuler" ("Id", "Ad", "Fiyat", "KategoriID") FROM stdin;
66666666-6666-6666-6666-666666666661	Köfte	45.00	22222222-2222-2222-2222-222222222221
66666666-6666-6666-6666-666666666662	Çorba	25.00	22222222-2222-2222-2222-222222222226
66666666-6666-6666-6666-666666666663	Salata	15.00	22222222-2222-2222-2222-222222222225
66666666-6666-6666-6666-666666666664	Pizza	50.00	22222222-2222-2222-2222-222222222221
66666666-6666-6666-6666-666666666665	Dondurma	20.00	22222222-2222-2222-2222-222222222228
66666666-6666-6666-6666-666666666666	Tavuk	40.00	22222222-2222-2222-2222-222222222221
66666666-6666-6666-6666-666666666667	Makarna	35.00	22222222-2222-2222-2222-222222222221
66666666-6666-6666-6666-666666666668	Ayran	5.00	22222222-2222-2222-2222-222222222222
66666666-6666-6666-6666-666666666669	Fanta	7.00	22222222-2222-2222-2222-222222222222
66666666-6666-6666-6666-666666666670	Soda	6.00	22222222-2222-2222-2222-222222222222
\.


--
-- TOC entry 3513 (class 0 OID 33203)
-- Dependencies: 231
-- Data for Name: Musteriler; Type: TABLE DATA; Schema: public; Owner: vtys
--

COPY public."Musteriler" ("Id", "Ad", "Soyad", "Discriminator", "MusteriId", "Telefon") FROM stdin;
8b0d6eeb-a1e2-46fc-9660-1e16d92acca2	Murat	Öztürk	Müşteri	8b0d6eeb-a1e2-46fc-9660-1e16d92acca2	12312321
4be35515-f626-4c97-8261-c19b5c12af11	Yıldıray Ali	Kara	Müşteri	4be35515-f626-4c97-8261-c19b5c12af11	505050
ed579a84-16bf-45e9-b940-0fc2a257fc1a	Doğan	Kaya	Müşteri	ed579a84-16bf-45e9-b940-0fc2a257fc1a	5454545
dd00912e-f645-4fee-9040-f1ff97ca7cc7	Yavuz	Selim İlhan	Müşteri	dd00912e-f645-4fee-9040-f1ff97ca7cc7	5454545
e420d8c4-4521-416a-94ca-29439394d620	Metehan	Öztürk	Müşteri	e420d8c4-4521-416a-94ca-29439394d620	5454545
1a683e1f-f06e-4439-91ce-fdb8fdad9d67	Celal	Çeken	Müşteri	1a683e1f-f06e-4439-91ce-fdb8fdad9d67	1999819198
\.


--
-- TOC entry 3503 (class 0 OID 33013)
-- Dependencies: 221
-- Data for Name: OdemeTurleri; Type: TABLE DATA; Schema: public; Owner: vtys
--

COPY public."OdemeTurleri" ("Id", "Ad") FROM stdin;
f8f70783-a57e-4d9f-b614-2f98ce5ad606	Nakit
41b99b12-977d-45a5-8340-8c2fa69c5e17	Kredi Kartı
7b004b17-71f0-483f-838d-eb419df50677	Banka Kartı
ecd7332a-dda8-4c39-9387-93b10250371d	Havale/EFT
de4ffc0d-bf4c-4dbb-96c8-eeeceeab9343	PayPal
3892473b-e1ec-4f84-a4f8-a838cd156493	Apple Pay
6b73c195-c402-4310-b1ea-0cfae0e0dd56	Google Pay
90af442a-b18f-497f-b419-262e6aac7830	Kripto Para
d0304ae7-bb51-4673-b392-6796eaa843e6	Mobil Ödeme
9f22f3ce-182e-4696-b53d-ef216fcf0806	Çek
\.


--
-- TOC entry 3511 (class 0 OID 33137)
-- Dependencies: 229
-- Data for Name: Odemeler; Type: TABLE DATA; Schema: public; Owner: vtys
--

COPY public."Odemeler" ("Id", "SiparisID", "OdemeTuruID", "Tutar") FROM stdin;
0193ef40-9cd4-7345-9a4b-7a16c9b427f8	88d18cc6-5eec-4cd3-82c1-313ec0647edf	7b004b17-71f0-483f-838d-eb419df50677	300.00
0193ef40-a990-7d93-9d26-e4d86e6e74d2	88d18cc6-5eec-4cd3-82c1-313ec0647edf	90af442a-b18f-497f-b419-262e6aac7830	345.00
0193ef40-bab3-7e0f-83be-fbb2e80d4a89	38012b9b-67b2-488b-84dd-ccade4c2aab2	41b99b12-977d-45a5-8340-8c2fa69c5e17	3040.00
0193ef40-c829-78e7-a9cf-6b1eb4a385bc	38012b9b-67b2-488b-84dd-ccade4c2aab2	90af442a-b18f-497f-b419-262e6aac7830	150.00
0193ef40-d605-76ef-93d0-d3c291e0d895	a27a23c6-4d85-4810-88f1-3136be21020e	d0304ae7-bb51-4673-b392-6796eaa843e6	300.00
0193ef41-b755-75a8-b32c-d9b18ee10e2d	8c15291f-6082-48d6-9277-6706deb8ec96	7b004b17-71f0-483f-838d-eb419df50677	300.00
0193ef41-c10e-7811-9d12-b04a93ee44fb	8c15291f-6082-48d6-9277-6706deb8ec96	de4ffc0d-bf4c-4dbb-96c8-eeeceeab9343	655.00
\.


--
-- TOC entry 3514 (class 0 OID 40992)
-- Dependencies: 232
-- Data for Name: Personeller; Type: TABLE DATA; Schema: public; Owner: vtys
--

COPY public."Personeller" ("Id", "Ad", "Soyad", "Discriminator", "PersonelId", "Pozisyon") FROM stdin;
184ae6b5-40f8-482e-b2b8-184256beec67	Ayşe	Korkmaz	Personel	184ae6b5-40f8-482e-b2b8-184256beec67	Personel
65c4beb1-1830-4da7-ad59-e6dee737cf8e	Elif	Çakır	Personel	65c4beb1-1830-4da7-ad59-e6dee737cf8e	Personel
2d310725-3ab6-4795-b8b2-ebd9e3cb6f27	Buse	Güçlü	Personel	2d310725-3ab6-4795-b8b2-ebd9e3cb6f27	Personel
6f0bd5ee-4b37-47b9-b171-dc566ac8d72a	Aslı	Han	Personel	6f0bd5ee-4b37-47b9-b171-dc566ac8d72a	Personel
0d770138-49c8-412b-8608-91890205985e	Melek	Çelik	Personel	0d770138-49c8-412b-8608-91890205985e	Personel
4be35515-f626-4c97-8261-c19b5c12af11	Yıldıray Ali	Kara	Personel	4be35515-f626-4c97-8261-c19b5c12af11	Personel
dd00912e-f645-4fee-9040-f1ff97ca7cc7	Yavuz	Selim İlhan	Personel	dd00912e-f645-4fee-9040-f1ff97ca7cc7	Personel
e420d8c4-4521-416a-94ca-29439394d620	Metehan	Öztürk	Personel	e420d8c4-4521-416a-94ca-29439394d620	Personel
d9dc2a86-9a4d-4c65-b014-f72ebf5cf398	Tuğçe	Özsoy	Personel	d9dc2a86-9a4d-4c65-b014-f72ebf5cf398	Personel
\.


--
-- TOC entry 3507 (class 0 OID 33075)
-- Dependencies: 225
-- Data for Name: Rezervasyonlar; Type: TABLE DATA; Schema: public; Owner: vtys
--

COPY public."Rezervasyonlar" ("Id", "MasaID", "MusteriID", "RezervasyonTarihi") FROM stdin;
9e718962-1176-4ac8-93dd-25bb83d6f5c0	55555555-5555-5555-5555-555555555551	8b0d6eeb-a1e2-46fc-9660-1e16d92acca2	2024-12-22 16:42:19.002269+00
31016d51-8980-44b4-bcec-436696145682	0193e942-8e52-7d3b-8525-45b8cbdc17bb	4be35515-f626-4c97-8261-c19b5c12af11	2024-12-22 16:42:19.002269+00
e32171a1-614d-4b38-bb71-e7c82c03a592	0193e942-2896-7fbe-9ffe-66b7015cb307	ed579a84-16bf-45e9-b940-0fc2a257fc1a	2024-12-22 16:42:19.002269+00
7b88fbae-097c-41ab-83f2-a94ae3adf3f9	55555555-5555-5555-5555-555555555554	dd00912e-f645-4fee-9040-f1ff97ca7cc7	2024-12-22 16:42:19.002269+00
\.


--
-- TOC entry 3512 (class 0 OID 33154)
-- Dependencies: 230
-- Data for Name: SiparisDetaylar; Type: TABLE DATA; Schema: public; Owner: vtys
--

COPY public."SiparisDetaylar" ("Id", "SiparisID", "MenuID", "Adet", "OdendiMi") FROM stdin;
0193ef40-3d55-7ca4-9189-25647c51363e	88d18cc6-5eec-4cd3-82c1-313ec0647edf	66666666-6666-6666-6666-666666666662	12	t
0193ef40-3d61-75ec-801b-2602349d9bce	88d18cc6-5eec-4cd3-82c1-313ec0647edf	66666666-6666-6666-6666-666666666663	23	t
0193ef40-688e-7973-b825-91b472639c60	38012b9b-67b2-488b-84dd-ccade4c2aab2	66666666-6666-6666-6666-666666666661	42	t
0193ef40-688e-797f-be1d-625df270855a	38012b9b-67b2-488b-84dd-ccade4c2aab2	66666666-6666-6666-6666-666666666664	23	t
0193ef40-688e-7863-8661-82f94396deb1	38012b9b-67b2-488b-84dd-ccade4c2aab2	66666666-6666-6666-6666-666666666664	3	t
0193ef40-8a0d-7a55-a165-f6ac330abe74	a27a23c6-4d85-4810-88f1-3136be21020e	66666666-6666-6666-6666-666666666662	12	t
0193ef41-72f2-71c4-b4bd-8ab9051ab7ad	64e7111a-d0c3-4653-a4f2-f7030c54a50c	66666666-6666-6666-6666-666666666662	12	f
0193ef41-8d1c-7801-bffa-0d3d597589b7	64ca4ace-8e30-4b16-a3fd-91048b0de276	66666666-6666-6666-6666-666666666665	12	f
0193ef41-a60a-7cda-92df-4f2f2dc1ccd8	a3c93ed0-113e-426d-911a-bb0f7a4998aa	66666666-6666-6666-6666-666666666664	23	f
0193ef41-5cbd-71a3-a7db-b311b7e278e1	8c15291f-6082-48d6-9277-6706deb8ec96	66666666-6666-6666-6666-666666666662	12	t
0193ef41-5cbd-746c-a699-3b883d2108f4	8c15291f-6082-48d6-9277-6706deb8ec96	66666666-6666-6666-6666-666666666663	13	t
0193ef41-5cbd-76f6-a608-b57a539b0c0d	8c15291f-6082-48d6-9277-6706deb8ec96	66666666-6666-6666-6666-666666666665	23	t
0193ef41-db91-78b0-89da-4c580483d1e9	e79fb173-ee13-4e41-9c9d-3816df0d20c3	66666666-6666-6666-6666-666666666663	12	f
\.


--
-- TOC entry 3508 (class 0 OID 33090)
-- Dependencies: 226
-- Data for Name: Siparisler; Type: TABLE DATA; Schema: public; Owner: vtys
--

COPY public."Siparisler" ("Id", "MasaID", "MusteriID", "PersonelID", "SiparisTarihi", "OdendiMi") FROM stdin;
88d18cc6-5eec-4cd3-82c1-313ec0647edf	0193d37c-3c20-709f-835c-186a70e17552	4be35515-f626-4c97-8261-c19b5c12af11	dd00912e-f645-4fee-9040-f1ff97ca7cc7	2024-12-22 16:43:04.773374+00	t
38012b9b-67b2-488b-84dd-ccade4c2aab2	55555555-5555-5555-5555-555555555551	ed579a84-16bf-45e9-b940-0fc2a257fc1a	e420d8c4-4521-416a-94ca-29439394d620	2024-12-22 16:43:15.953128+00	t
a27a23c6-4d85-4810-88f1-3136be21020e	55555555-5555-5555-5555-555555555552	1a683e1f-f06e-4439-91ce-fdb8fdad9d67	184ae6b5-40f8-482e-b2b8-184256beec67	2024-12-22 16:43:24.528949+00	t
64e7111a-d0c3-4653-a4f2-f7030c54a50c	55555555-5555-5555-5555-555555555551	ed579a84-16bf-45e9-b940-0fc2a257fc1a	e420d8c4-4521-416a-94ca-29439394d620	2024-12-22 16:44:24.152946+00	f
64ca4ace-8e30-4b16-a3fd-91048b0de276	55555555-5555-5555-5555-555555555552	dd00912e-f645-4fee-9040-f1ff97ca7cc7	6f0bd5ee-4b37-47b9-b171-dc566ac8d72a	2024-12-22 16:44:30.849904+00	f
a3c93ed0-113e-426d-911a-bb0f7a4998aa	55555555-5555-5555-5555-555555555553	dd00912e-f645-4fee-9040-f1ff97ca7cc7	4be35515-f626-4c97-8261-c19b5c12af11	2024-12-22 16:44:37.234149+00	f
8c15291f-6082-48d6-9277-6706deb8ec96	0193d37c-3c20-709f-835c-186a70e17552	dd00912e-f645-4fee-9040-f1ff97ca7cc7	65c4beb1-1830-4da7-ad59-e6dee737cf8e	2024-12-22 16:44:18.46871+00	t
e79fb173-ee13-4e41-9c9d-3816df0d20c3	55555555-5555-5555-5555-555555555554	1a683e1f-f06e-4439-91ce-fdb8fdad9d67	65c4beb1-1830-4da7-ad59-e6dee737cf8e	2024-12-22 16:44:50.93571+00	f
\.


--
-- TOC entry 3509 (class 0 OID 33110)
-- Dependencies: 227
-- Data for Name: Stoklar; Type: TABLE DATA; Schema: public; Owner: vtys
--

COPY public."Stoklar" ("Id", "MalzemeID", "Miktar") FROM stdin;
33333333-3333-3333-3333-333333333311	22222222-2222-2222-2222-222222222211	100
33333333-3333-3333-3333-333333333312	22222222-2222-2222-2222-222222222212	150
33333333-3333-3333-3333-333333333313	22222222-2222-2222-2222-222222222213	200
33333333-3333-3333-3333-333333333314	22222222-2222-2222-2222-222222222214	50
33333333-3333-3333-3333-333333333315	22222222-2222-2222-2222-222222222215	300
33333333-3333-3333-3333-333333333316	22222222-2222-2222-2222-222222222216	120
33333333-3333-3333-3333-333333333317	22222222-2222-2222-2222-222222222217	75
33333333-3333-3333-3333-333333333318	22222222-2222-2222-2222-222222222218	60
33333333-3333-3333-3333-333333333319	22222222-2222-2222-2222-222222222219	250
33333333-3333-3333-3333-333333333320	22222222-2222-2222-2222-222222222220	180
25a73fd8-9658-480d-bcc6-0d7c5c472c3a	22222222-2222-2222-2222-222222222230	30
\.


--
-- TOC entry 3510 (class 0 OID 33120)
-- Dependencies: 228
-- Data for Name: TedarikSiparisleri; Type: TABLE DATA; Schema: public; Owner: vtys
--

COPY public."TedarikSiparisleri" ("Id", "TedarikciID", "MalzemeID", "BirimFiyat", "Miktar", "SiparisTarihi") FROM stdin;
44444444-4444-4444-4444-444444444411	11111111-1111-1111-1111-111111111111	22222222-2222-2222-2222-222222222211	10.50	100	2024-12-01 00:00:00+00
44444444-4444-4444-4444-444444444412	11111111-1111-1111-1111-111111111112	22222222-2222-2222-2222-222222222212	15.75	200	2024-12-02 00:00:00+00
44444444-4444-4444-4444-444444444413	11111111-1111-1111-1111-111111111113	22222222-2222-2222-2222-222222222213	12.30	150	2024-12-03 00:00:00+00
44444444-4444-4444-4444-444444444414	11111111-1111-1111-1111-111111111114	22222222-2222-2222-2222-222222222214	9.60	50	2024-12-04 00:00:00+00
44444444-4444-4444-4444-444444444415	11111111-1111-1111-1111-111111111115	22222222-2222-2222-2222-222222222215	14.80	300	2024-12-05 00:00:00+00
44444444-4444-4444-4444-444444444416	11111111-1111-1111-1111-111111111116	22222222-2222-2222-2222-222222222216	11.25	120	2024-12-06 00:00:00+00
44444444-4444-4444-4444-444444444417	11111111-1111-1111-1111-111111111117	22222222-2222-2222-2222-222222222217	13.90	75	2024-12-07 00:00:00+00
44444444-4444-4444-4444-444444444418	11111111-1111-1111-1111-111111111118	22222222-2222-2222-2222-222222222218	7.45	60	2024-12-08 00:00:00+00
44444444-4444-4444-4444-444444444419	11111111-1111-1111-1111-111111111119	22222222-2222-2222-2222-222222222219	10.00	250	2024-12-09 00:00:00+00
44444444-4444-4444-4444-444444444420	11111111-1111-1111-1111-111111111120	22222222-2222-2222-2222-222222222220	16.40	180	2024-12-10 00:00:00+00
adc9e6eb-547b-47b6-9afa-59efb7e86d53	11111111-1111-1111-1111-111111111111	22222222-2222-2222-2222-222222222211	15	30	2024-12-20 13:52:20.741776+00
4dda64b7-d52e-4819-802a-c54162f813f7	11111111-1111-1111-1111-111111111111	22222222-2222-2222-2222-222222222230	30	30	2024-12-20 14:19:43.227732+00
\.


--
-- TOC entry 3504 (class 0 OID 33020)
-- Dependencies: 222
-- Data for Name: Tedarikciler; Type: TABLE DATA; Schema: public; Owner: vtys
--

COPY public."Tedarikciler" ("Id", "Ad", "Iletisim") FROM stdin;
11111111-1111-1111-1111-111111111111	Tedarikçi A	1234567890
11111111-1111-1111-1111-111111111112	Tedarikçi B	0987654321
11111111-1111-1111-1111-111111111113	Tedarikçi C	1122334455
11111111-1111-1111-1111-111111111114	Tedarikçi D	2233445566
11111111-1111-1111-1111-111111111115	Tedarikçi E	3344556677
11111111-1111-1111-1111-111111111116	Tedarikçi F	4455667788
11111111-1111-1111-1111-111111111117	Tedarikçi G	5566778899
11111111-1111-1111-1111-111111111118	Tedarikçi H	6677889900
11111111-1111-1111-1111-111111111119	Tedarikçi I	7788990011
11111111-1111-1111-1111-111111111120	Tedarikçi J	8899001122
\.


--
-- TOC entry 3288 (class 2606 OID 32993)
-- Name: Giderler PK_Giderler; Type: CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Giderler"
    ADD CONSTRAINT "PK_Giderler" PRIMARY KEY ("Id");


--
-- TOC entry 3290 (class 2606 OID 33000)
-- Name: Kategoriler PK_Kategoriler; Type: CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Kategoriler"
    ADD CONSTRAINT "PK_Kategoriler" PRIMARY KEY ("Id");


--
-- TOC entry 3292 (class 2606 OID 33007)
-- Name: Kisiler PK_Kisiler; Type: CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Kisiler"
    ADD CONSTRAINT "PK_Kisiler" PRIMARY KEY ("Id");


--
-- TOC entry 3306 (class 2606 OID 33069)
-- Name: Malzemeler PK_Malzemeler; Type: CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Malzemeler"
    ADD CONSTRAINT "PK_Malzemeler" PRIMARY KEY ("Id");


--
-- TOC entry 3294 (class 2606 OID 33012)
-- Name: Masalar PK_Masalar; Type: CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Masalar"
    ADD CONSTRAINT "PK_Masalar" PRIMARY KEY ("Id");


--
-- TOC entry 3303 (class 2606 OID 33033)
-- Name: Menuler PK_Menuler; Type: CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Menuler"
    ADD CONSTRAINT "PK_Menuler" PRIMARY KEY ("Id");


--
-- TOC entry 3332 (class 2606 OID 33209)
-- Name: Musteriler PK_Musteriler; Type: CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Musteriler"
    ADD CONSTRAINT "PK_Musteriler" PRIMARY KEY ("Id");


--
-- TOC entry 3298 (class 2606 OID 33019)
-- Name: OdemeTurleri PK_OdemeTurleri; Type: CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."OdemeTurleri"
    ADD CONSTRAINT "PK_OdemeTurleri" PRIMARY KEY ("Id");


--
-- TOC entry 3326 (class 2606 OID 33143)
-- Name: Odemeler PK_Odemeler; Type: CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Odemeler"
    ADD CONSTRAINT "PK_Odemeler" PRIMARY KEY ("Id");


--
-- TOC entry 3334 (class 2606 OID 40998)
-- Name: Personeller PK_Personeller; Type: CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Personeller"
    ADD CONSTRAINT "PK_Personeller" PRIMARY KEY ("Id");


--
-- TOC entry 3310 (class 2606 OID 33079)
-- Name: Rezervasyonlar PK_Rezervasyonlar; Type: CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Rezervasyonlar"
    ADD CONSTRAINT "PK_Rezervasyonlar" PRIMARY KEY ("Id");


--
-- TOC entry 3330 (class 2606 OID 33158)
-- Name: SiparisDetaylar PK_SiparisDetaylar; Type: CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."SiparisDetaylar"
    ADD CONSTRAINT "PK_SiparisDetaylar" PRIMARY KEY ("Id");


--
-- TOC entry 3315 (class 2606 OID 33094)
-- Name: Siparisler PK_Siparisler; Type: CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Siparisler"
    ADD CONSTRAINT "PK_Siparisler" PRIMARY KEY ("Id");


--
-- TOC entry 3318 (class 2606 OID 33114)
-- Name: Stoklar PK_Stoklar; Type: CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Stoklar"
    ADD CONSTRAINT "PK_Stoklar" PRIMARY KEY ("Id");


--
-- TOC entry 3322 (class 2606 OID 33126)
-- Name: TedarikSiparisleri PK_TedarikSiparisleri; Type: CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."TedarikSiparisleri"
    ADD CONSTRAINT "PK_TedarikSiparisleri" PRIMARY KEY ("Id");


--
-- TOC entry 3300 (class 2606 OID 33026)
-- Name: Tedarikciler PK_Tedarikciler; Type: CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Tedarikciler"
    ADD CONSTRAINT "PK_Tedarikciler" PRIMARY KEY ("Id");


--
-- TOC entry 3296 (class 2606 OID 57354)
-- Name: Masalar masalar_MasaNo_Unique; Type: CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Masalar"
    ADD CONSTRAINT "masalar_MasaNo_Unique" UNIQUE ("MasaNo");


--
-- TOC entry 3304 (class 1259 OID 33169)
-- Name: IX_Malzemeler_TedarikciID; Type: INDEX; Schema: public; Owner: vtys
--

CREATE INDEX "IX_Malzemeler_TedarikciID" ON public."Malzemeler" USING btree ("TedarikciID");


--
-- TOC entry 3301 (class 1259 OID 33170)
-- Name: IX_Menuler_KategoriID; Type: INDEX; Schema: public; Owner: vtys
--

CREATE INDEX "IX_Menuler_KategoriID" ON public."Menuler" USING btree ("KategoriID");


--
-- TOC entry 3323 (class 1259 OID 33171)
-- Name: IX_Odemeler_OdemeTuruID; Type: INDEX; Schema: public; Owner: vtys
--

CREATE INDEX "IX_Odemeler_OdemeTuruID" ON public."Odemeler" USING btree ("OdemeTuruID");


--
-- TOC entry 3324 (class 1259 OID 33172)
-- Name: IX_Odemeler_SiparisID; Type: INDEX; Schema: public; Owner: vtys
--

CREATE INDEX "IX_Odemeler_SiparisID" ON public."Odemeler" USING btree ("SiparisID");


--
-- TOC entry 3307 (class 1259 OID 33173)
-- Name: IX_Rezervasyonlar_MasaID; Type: INDEX; Schema: public; Owner: vtys
--

CREATE INDEX "IX_Rezervasyonlar_MasaID" ON public."Rezervasyonlar" USING btree ("MasaID");


--
-- TOC entry 3308 (class 1259 OID 33174)
-- Name: IX_Rezervasyonlar_MusteriID; Type: INDEX; Schema: public; Owner: vtys
--

CREATE INDEX "IX_Rezervasyonlar_MusteriID" ON public."Rezervasyonlar" USING btree ("MusteriID");


--
-- TOC entry 3327 (class 1259 OID 33175)
-- Name: IX_SiparisDetaylar_MenuID; Type: INDEX; Schema: public; Owner: vtys
--

CREATE INDEX "IX_SiparisDetaylar_MenuID" ON public."SiparisDetaylar" USING btree ("MenuID");


--
-- TOC entry 3328 (class 1259 OID 33176)
-- Name: IX_SiparisDetaylar_SiparisID; Type: INDEX; Schema: public; Owner: vtys
--

CREATE INDEX "IX_SiparisDetaylar_SiparisID" ON public."SiparisDetaylar" USING btree ("SiparisID");


--
-- TOC entry 3311 (class 1259 OID 33177)
-- Name: IX_Siparisler_MasaID; Type: INDEX; Schema: public; Owner: vtys
--

CREATE INDEX "IX_Siparisler_MasaID" ON public."Siparisler" USING btree ("MasaID");


--
-- TOC entry 3312 (class 1259 OID 33178)
-- Name: IX_Siparisler_MusteriID; Type: INDEX; Schema: public; Owner: vtys
--

CREATE INDEX "IX_Siparisler_MusteriID" ON public."Siparisler" USING btree ("MusteriID");


--
-- TOC entry 3313 (class 1259 OID 33179)
-- Name: IX_Siparisler_PersonelID; Type: INDEX; Schema: public; Owner: vtys
--

CREATE INDEX "IX_Siparisler_PersonelID" ON public."Siparisler" USING btree ("PersonelID");


--
-- TOC entry 3316 (class 1259 OID 33180)
-- Name: IX_Stoklar_MalzemeID; Type: INDEX; Schema: public; Owner: vtys
--

CREATE INDEX "IX_Stoklar_MalzemeID" ON public."Stoklar" USING btree ("MalzemeID");


--
-- TOC entry 3319 (class 1259 OID 33181)
-- Name: IX_TedarikSiparisleri_MalzemeID; Type: INDEX; Schema: public; Owner: vtys
--

CREATE INDEX "IX_TedarikSiparisleri_MalzemeID" ON public."TedarikSiparisleri" USING btree ("MalzemeID");


--
-- TOC entry 3320 (class 1259 OID 33182)
-- Name: IX_TedarikSiparisleri_TedarikciID; Type: INDEX; Schema: public; Owner: vtys
--

CREATE INDEX "IX_TedarikSiparisleri_TedarikciID" ON public."TedarikSiparisleri" USING btree ("TedarikciID");


--
-- TOC entry 3351 (class 2620 OID 49170)
-- Name: TedarikSiparisleri gider_trigger; Type: TRIGGER; Schema: public; Owner: vtys
--

CREATE TRIGGER gider_trigger AFTER INSERT ON public."TedarikSiparisleri" FOR EACH ROW EXECUTE FUNCTION public.gider_ekle();


--
-- TOC entry 3350 (class 2620 OID 49176)
-- Name: Malzemeler malzeme_ekle_trigger; Type: TRIGGER; Schema: public; Owner: vtys
--

CREATE TRIGGER malzeme_ekle_trigger AFTER INSERT ON public."Malzemeler" FOR EACH ROW EXECUTE FUNCTION public.stok_ekle();


--
-- TOC entry 3353 (class 2620 OID 49172)
-- Name: Musteriler musteri_sil_trigger; Type: TRIGGER; Schema: public; Owner: vtys
--

CREATE TRIGGER musteri_sil_trigger BEFORE DELETE ON public."Musteriler" FOR EACH ROW EXECUTE FUNCTION public.musteri_silme();


--
-- TOC entry 3352 (class 2620 OID 49177)
-- Name: TedarikSiparisleri siparis_ekle_trigger; Type: TRIGGER; Schema: public; Owner: vtys
--

CREATE TRIGGER siparis_ekle_trigger AFTER INSERT ON public."TedarikSiparisleri" FOR EACH ROW EXECUTE FUNCTION public.stok_guncelle();


--
-- TOC entry 3336 (class 2606 OID 33070)
-- Name: Malzemeler FK_Malzemeler_Tedarikciler_TedarikciID; Type: FK CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Malzemeler"
    ADD CONSTRAINT "FK_Malzemeler_Tedarikciler_TedarikciID" FOREIGN KEY ("TedarikciID") REFERENCES public."Tedarikciler"("Id") ON DELETE CASCADE;


--
-- TOC entry 3335 (class 2606 OID 33034)
-- Name: Menuler FK_Menuler_Kategoriler_KategoriID; Type: FK CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Menuler"
    ADD CONSTRAINT "FK_Menuler_Kategoriler_KategoriID" FOREIGN KEY ("KategoriID") REFERENCES public."Kategoriler"("Id") ON DELETE CASCADE;


--
-- TOC entry 3348 (class 2606 OID 33238)
-- Name: Musteriler FK_Musteriler_Kisiler_Id; Type: FK CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Musteriler"
    ADD CONSTRAINT "FK_Musteriler_Kisiler_Id" FOREIGN KEY ("Id") REFERENCES public."Kisiler"("Id");


--
-- TOC entry 3344 (class 2606 OID 33144)
-- Name: Odemeler FK_Odemeler_OdemeTurleri_OdemeTuruID; Type: FK CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Odemeler"
    ADD CONSTRAINT "FK_Odemeler_OdemeTurleri_OdemeTuruID" FOREIGN KEY ("OdemeTuruID") REFERENCES public."OdemeTurleri"("Id") ON DELETE CASCADE;


--
-- TOC entry 3345 (class 2606 OID 33149)
-- Name: Odemeler FK_Odemeler_Siparisler_SiparisID; Type: FK CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Odemeler"
    ADD CONSTRAINT "FK_Odemeler_Siparisler_SiparisID" FOREIGN KEY ("SiparisID") REFERENCES public."Siparisler"("Id") ON DELETE CASCADE;


--
-- TOC entry 3349 (class 2606 OID 41005)
-- Name: Personeller FK_Personeller_Kisiler_Id; Type: FK CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Personeller"
    ADD CONSTRAINT "FK_Personeller_Kisiler_Id" FOREIGN KEY ("PersonelId") REFERENCES public."Kisiler"("Id");


--
-- TOC entry 3337 (class 2606 OID 33080)
-- Name: Rezervasyonlar FK_Rezervasyonlar_Masalar_MasaID; Type: FK CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Rezervasyonlar"
    ADD CONSTRAINT "FK_Rezervasyonlar_Masalar_MasaID" FOREIGN KEY ("MasaID") REFERENCES public."Masalar"("Id") ON DELETE CASCADE;


--
-- TOC entry 3338 (class 2606 OID 33215)
-- Name: Rezervasyonlar FK_Rezervasyonlar_Musteri; Type: FK CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Rezervasyonlar"
    ADD CONSTRAINT "FK_Rezervasyonlar_Musteri" FOREIGN KEY ("MusteriID") REFERENCES public."Musteriler"("Id") ON DELETE CASCADE;


--
-- TOC entry 3339 (class 2606 OID 33220)
-- Name: Siparisler FK_Rezervasyonlar_Musteri; Type: FK CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Siparisler"
    ADD CONSTRAINT "FK_Rezervasyonlar_Musteri" FOREIGN KEY ("MusteriID") REFERENCES public."Musteriler"("Id") ON DELETE CASCADE;


--
-- TOC entry 3346 (class 2606 OID 33159)
-- Name: SiparisDetaylar FK_SiparisDetaylar_Menuler_MenuID; Type: FK CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."SiparisDetaylar"
    ADD CONSTRAINT "FK_SiparisDetaylar_Menuler_MenuID" FOREIGN KEY ("MenuID") REFERENCES public."Menuler"("Id") ON DELETE CASCADE;


--
-- TOC entry 3347 (class 2606 OID 33164)
-- Name: SiparisDetaylar FK_SiparisDetaylar_Siparisler_SiparisID; Type: FK CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."SiparisDetaylar"
    ADD CONSTRAINT "FK_SiparisDetaylar_Siparisler_SiparisID" FOREIGN KEY ("SiparisID") REFERENCES public."Siparisler"("Id") ON DELETE CASCADE;


--
-- TOC entry 3340 (class 2606 OID 33095)
-- Name: Siparisler FK_Siparisler_Masalar_MasaID; Type: FK CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Siparisler"
    ADD CONSTRAINT "FK_Siparisler_Masalar_MasaID" FOREIGN KEY ("MasaID") REFERENCES public."Masalar"("Id") ON DELETE CASCADE;


--
-- TOC entry 3341 (class 2606 OID 33115)
-- Name: Stoklar FK_Stoklar_Malzemeler_MalzemeID; Type: FK CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."Stoklar"
    ADD CONSTRAINT "FK_Stoklar_Malzemeler_MalzemeID" FOREIGN KEY ("MalzemeID") REFERENCES public."Malzemeler"("Id") ON DELETE CASCADE;


--
-- TOC entry 3342 (class 2606 OID 33127)
-- Name: TedarikSiparisleri FK_TedarikSiparisleri_Malzemeler_MalzemeID; Type: FK CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."TedarikSiparisleri"
    ADD CONSTRAINT "FK_TedarikSiparisleri_Malzemeler_MalzemeID" FOREIGN KEY ("MalzemeID") REFERENCES public."Malzemeler"("Id") ON DELETE CASCADE;


--
-- TOC entry 3343 (class 2606 OID 33132)
-- Name: TedarikSiparisleri FK_TedarikSiparisleri_Tedarikciler_TedarikciID; Type: FK CONSTRAINT; Schema: public; Owner: vtys
--

ALTER TABLE ONLY public."TedarikSiparisleri"
    ADD CONSTRAINT "FK_TedarikSiparisleri_Tedarikciler_TedarikciID" FOREIGN KEY ("TedarikciID") REFERENCES public."Tedarikciler"("Id") ON DELETE CASCADE;


-- Completed on 2024-12-22 19:46:20

--
-- PostgreSQL database dump complete
--

-- Completed on 2024-12-22 19:46:20

--
-- PostgreSQL database cluster dump complete
--

