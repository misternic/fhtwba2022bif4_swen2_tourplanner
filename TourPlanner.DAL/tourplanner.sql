--
-- PostgreSQL database dump
--

-- Dumped from database version 14.2
-- Dumped by pg_dump version 14.2

-- Started on 2022-03-09 12:19:48 CET

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

DROP DATABASE tourplanner;
--
-- TOC entry 3582 (class 1262 OID 24613)
-- Name: tourplanner; Type: DATABASE; Schema: -; Owner: -
--

CREATE DATABASE tourplanner WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'en_GB.UTF-8';


\connect tourplanner

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_table_access_method = heap;

--
-- TOC entry 210 (class 1259 OID 24626)
-- Name: logs; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.logs (
    id uuid NOT NULL,
    tour_id uuid NOT NULL,
    date date NOT NULL,
    difficulty integer,
    duration integer NOT NULL,
    rating integer,
    comment character varying(500)
);


--
-- TOC entry 209 (class 1259 OID 24619)
-- Name: tours; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public.tours (
    id uuid NOT NULL,
    name character varying(100) NOT NULL,
    description character varying(500),
    "from" character varying(250) NOT NULL,
    "to" character varying(250) NOT NULL,
    transport_type integer,
    distance double precision,
    estimated_duration integer
);


--
-- TOC entry 3436 (class 2606 OID 24632)
-- Name: logs logs_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.logs
    ADD CONSTRAINT logs_pkey PRIMARY KEY (id);


--
-- TOC entry 3434 (class 2606 OID 24625)
-- Name: tours tours_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.tours
    ADD CONSTRAINT tours_pkey PRIMARY KEY (id);


--
-- TOC entry 3437 (class 2606 OID 24633)
-- Name: logs fk_tour_id; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public.logs
    ADD CONSTRAINT fk_tour_id FOREIGN KEY (tour_id) REFERENCES public.tours(id);


-- Completed on 2022-03-09 12:19:48 CET

--
-- PostgreSQL database dump complete
--

