
CREATE OR REPLACE PROCEDURE public.update_faq(IN p_id integer, IN p_question text, IN p_answer text, IN p_categoryid integer, IN p_createdby integer)
 LANGUAGE plpgsql
AS $procedure$
DECLARE
BEGIN

	update faq 
	set question = p_question,
	answer = p_answer,
	categoryid =  p_categoryid,
	createdby = p_createdby,
	updatedon = current_timestamp 
	where  
	faqid = p_id; 

END;
$procedure$
;
