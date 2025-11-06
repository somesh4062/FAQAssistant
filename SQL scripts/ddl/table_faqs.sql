

CREATE TABLE faq (
	faqid serial4 NOT NULL,
	categoryid int4 NOT NULL,
	createdby int4 NOT NULL,
	question varchar(4000) NULL,
	answer varchar(4000) NULL,
	createdon timestamp DEFAULT CURRENT_TIMESTAMP NULL,
	updatedon timestamp NULL,
	categoryname varchar(20) NULL,
	CONSTRAINT faq_pkey PRIMARY KEY (faqid)
);


-- public.faq foreign keys

ALTER TABLE public.faq ADD CONSTRAINT faq_categoryid_fkey FOREIGN KEY (categoryid) REFERENCES category("CategoryId");
ALTER TABLE public.faq ADD CONSTRAINT faq_createdby_fkey FOREIGN KEY (createdby) REFERENCES users(userid);