
CREATE TABLE users (
	userid serial4 NOT NULL,
	username varchar(20) NOT NULL,
	useremail varchar(20) NOT NULL,
	creadtedon timestamp DEFAULT CURRENT_TIMESTAMP NULL,
	CONSTRAINT users_pkey PRIMARY KEY (userid)
);