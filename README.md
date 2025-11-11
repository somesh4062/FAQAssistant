# FAQAssistant
FAQ Assistant using .Net
(To check LLM integrated from OpenRouter v2-llm branch)
Setup Instructions
Prerequisites
    - VS Code or Visual Studio
    - PostgreSQL (DBeaver to handle sql related queries)
    - Postman/Swagger (testing & managing APIs)
    - Git (Cloning the repo and version controlling)


Clone the repo 

#create 
create database faqs 
#then SQL scripts -> dll -> "Exceute the tables scripts for tables creation."
#and after that SQL -> procedures -> "Exceute procedures"

API Endpoints 

BASE URL - http://localhost:5062/api/faq/

   Endpoint                                            Desc                                                       

  /api/faq/GetFAQs                                 | Fetch all FAQs                                
  /api/faq/GetFAQs?categoryname=account            | Fetch FAQs that belong to the “account”category.                    
  /api/faq/GetFAQs?searchtext=refund               | Search FAQs where the question or answer contains the word “refund”. 
  /api/faq/GetFAQs?faqid=30                        | Fetch the FAQ with specific ID = 30.                                 
  /api/faq/GetCategories                           | Fetch all Categories 
  /api/faq/GetCategories?pageNumber=2&pageSize=5   | Fetch all Categories (paginated)

  api/faq/AddFAQ                                   | Adds FAQ in DB only with validate userid 
  api/faq/AddCategory                              | Adds Category in only with validate userid 
  api/faq/UpdateFAQ                                | Updates FAQ in DB
  api/faq/DeleteFAQ                                | Delete FAQ from DB using FAQ id 


Sample Request-Response :

 /api/faq/GetFAQs 
   Response - 
   {
    "faqs": [
        {
            "faqid": 20,
            "question": "how can i reset my password?",
            "answer": "go to settings > security > reset password and follow the steps.",
            "categoryname": "account",
            "categoryid": 20
        }
    ],
    "isSuccess": true,
    "errortext": ""
}

 /api/faq/GetFAQs?categoryname=orders   
{
    "faqs": [
        {
            "faqid": 22,
            "question": "how can i track my order?",
            "answer": "navigate to my orders and click on track order to view delivery updates.",
            "categoryname": "orders",
            "categoryid": 22
        }
    ],
    "isSuccess": true,
    "errortext": ""
}

 /api/faq/GetFAQs?searchtext=refund 

{
    "faqs": [
        {
            "faqid": 21,
            "question": "how do i request a refund?",
            "answer": "open your orders page, select the order, and click on request refund.",
            "categoryname": "payments",
            "categoryid": 21
        }
    ],
    "isSuccess": true,
    "errortext": ""
}

/api/faq/GetFAQs?faqid=30 
{
    "faqs": [
        {
            "faqid": 30,
            "question": "how can i download my invoice?",
            "answer": "open the billing section, select your order, and click on download invoice.",
            "categoryname": "invoices",
            "categoryid": 30
        }
    ],
    "isSuccess": true,
    "errortext": ""
}

/api/faq/GetCategories?pageNumber=2&pageSize=2

{
    "categories": [
        {
            "categoryId": 23,
            "categoryname": "delivery",
            "createdby": 2
        },
        {
            "categoryId": 24,
            "categoryname": "returns",
            "createdby": 1
        },
        {
            "categoryId": 25,
            "categoryname": "technical support",
            "createdby": 2
        }
    ],
    "isSuccess": true,
    "errorText": ""
}

api/faq/AddFAQ 
  Request - 
    {
    "categoryid": 24,
    "createdby": 1,
    "categoryname": "Returns",
    "question": "Can I return a product after 30 days?",
    "answer": "Returns are accepted within 30 days of delivery, provided the product meets our return policy conditions.",
    "tags": ["returns", "refund", "policy", "product"]
    }
  Response - 
    {
    "isSuccess": true,
    "message": "FAQ added successfully",
    "errorText": ""
    }  

Error Response (as userid are only 1 and 2, if FAQ is createdby other user id)
Response - 
{
    "isSuccess": false,
    "message": "Failed to add FAQ",
    "errorText": "User does not exist"
}

api/faq/AddCategory    
  Request - 
  {
    "categoryname": "Help Center",
    "userid": 1
  }
  Response - 
  {
    "isSuccess": true,
    "message": "Category added successfully",
    "errorText": ""
  }

  api/faq/UpdateFAQ    
  Request- 
  {
  "question": "How to get invoice?",
  "answer": "Go to orders details.",
  "categoryid": 22,
  "createdby":1,
  "faqid": 32,
  "tags" : [],
  "categoryname" : "security"
}
Response -
{
    "isSuccess": true,
    "message": "FAQ updated successfully",
    "errorText": ""
}

api/faq/DeleteFAQ                                

Request - 
{
  "faqid": 21
}
Response - 
{
    "isSuccess": true,
    "message": "FAQ deleted successfully",
    "errorText": ""
}
  
