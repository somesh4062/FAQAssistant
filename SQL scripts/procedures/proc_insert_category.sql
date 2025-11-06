
CREATE OR REPLACE PROCEDURE public.insert_category(IN p_categoryname character varying, IN p_userid integer)
 LANGUAGE plpgsql
AS $procedure$
declare 
	begin
		insert
	into
	category (categoryname,
	createdby)
values (p_categoryname,
p_userid);
end;

$procedure$
;
