
CREATE OR REPLACE PROCEDURE public.delete_faq(IN p_id integer)
 LANGUAGE plpgsql
AS $procedure$
begin
	delete from faq where faqid = p_id;
end;
$procedure$
;
