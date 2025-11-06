

CREATE TABLE category (
	"CategoryId" int4 DEFAULT nextval('category_categoryid_seq'::regclass) NOT NULL,
	categoryname varchar(50) NOT NULL,
	createdby int4 NOT NULL,
	createdon timestamp DEFAULT CURRENT_TIMESTAMP NULL,
	CONSTRAINT category_pkey PRIMARY KEY ("CategoryId")
);


-- public.category foreign keys

ALTER TABLE public.category ADD CONSTRAINT category_createdby_fkey FOREIGN KEY (createdby) REFERENCES users(userid);