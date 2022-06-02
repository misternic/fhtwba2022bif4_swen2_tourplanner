-- public.tours definition

-- Drop table

-- DROP TABLE public.tours;

CREATE TABLE public.tours (
	id uuid NOT NULL,
	"name" varchar(100) NOT NULL,
	description varchar(500) NULL,
	"from" varchar(250) NOT NULL,
	"to" varchar(250) NOT NULL,
	transport_type int4 NULL,
	distance float8 NULL,
	estimated_duration int4 NULL,
	created timestamp NULL DEFAULT CURRENT_TIMESTAMP,
	CONSTRAINT tours_pkey PRIMARY KEY (id)
);


-- public.logs definition

-- Drop table

-- DROP TABLE public.logs;

CREATE TABLE public.logs (
	id uuid NOT NULL,
	tour_id uuid NOT NULL,
	"date" date NOT NULL,
	difficulty int4 NULL,
	duration int4 NOT NULL,
	rating int4 NULL,
	"comment" varchar(500) NULL,
	created timestamp NULL DEFAULT CURRENT_TIMESTAMP,
	CONSTRAINT logs_pkey PRIMARY KEY (id),
	CONSTRAINT fk_tour_id FOREIGN KEY (tour_id) REFERENCES public.tours(id) ON DELETE CASCADE
);