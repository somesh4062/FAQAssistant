using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FaqAssistant.api.Models
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<FAQList> faq { get; set; }
        public DbSet<CategoryList> category { get; set; }
        public DbSet<Users> users { get; set; } 

    }
    public class FAQ
    {
        public List<FAQList> faqs { get; set; } 
        public bool isSuccess { get; set; }
        public string errortext { get; set; }

    }
    public class FAQList
    {
        [Key]
        public int faqid { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public string categoryname { get; set; }    
        public int categoryid { get; set; }
    }
    public class FAQInput
    {
        public int faqid { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public int categoryid { get; set; }
        public int createdby { get; set; }
        public string categoryname { get; set; }    
        public List<string> tags { get; set; }
    }
    public class FAQDeleteInput
    {
        public int faqid { get; set; }
    }
    public class CategoryInput
    {

        public int userid { get; set; }
        public string categoryname { get; set; }
    }

    public class CategoryResponse
    {
        public List<CategoryList> categories { get; set; }
         public bool isSuccess { get; set; }
        public string errorText { get; set; }
       
    }
    
    public class CategoryList
    {
        [Key]
        public int CategoryId { get; set; }
        public string categoryname { get; set; }
        public int createdby { get; set; }

    }
    public class CommanResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public string errorText { get; set; }

    }

    public class Users
    {
        //public List<UserList> userlist { get; set; }
        [Key]
        public int userid { get; set; }
        public string username { get; set; }
        public string useremail { get; set; }

    }
}