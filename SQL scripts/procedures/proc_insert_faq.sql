
CREATE OR REPLACE PROCEDURE public.insert_faq(IN p_question text, IN p_answer text, IN p_categoryid integer, IN p_tags text, IN p_createdby integer, IN p_categoryname text)
 LANGUAGE plpgsql
AS $procedure$
DECLARE
    tag TEXT;
BEGIN
    --Insert the FAQ
    INSERT INTO faq (question, answer, categoryid,createdby,categoryname)
    VALUES (p_question, p_answer, p_categoryid, p_createdby,p_categoryname);

    FOREACH tag IN ARRAY string_to_array(p_tags, ',')
    LOOP
        INSERT INTO tag (keyword)
        VALUES (trim(tag));
    END LOOP;

END;
$procedure$
;
