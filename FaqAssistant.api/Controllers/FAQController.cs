using System.Threading.Tasks;
using FaqAssistant.api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace FaqAssistant.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FAQController : ControllerBase
    {
         private readonly IConfiguration _configuration;
         private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;
        public FAQController(AppDbContext context, IConfiguration configuration, HttpClient httpClient)
        {
            _context = context;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        [HttpGet("GetFAQs")]
        public async Task<FAQ> GetFAQsAsync(int? faqid = null, string? categoryname = null, string? searchtext = null)
        {
            FAQ response = new FAQ();
            var faqs = new List<FAQList>();
            string sql = string.Empty;
            try
            {
                if (faqid.HasValue)
                {
                    var result = await _context.faq.FindAsync(faqid.Value);
                    if (result == null)
                    {
                        response.isSuccess = false;
                        response.errortext = "FAQ not found";
                        return response;
                    }
                    faqs.Add(result);
                    response.faqs = faqs;
                    response.isSuccess = true;
                    response.errortext = string.Empty;
                    return response;
                }
                else if (!string.IsNullOrEmpty(categoryname))
                {
                    var result = await _context.faq.Where(f => f.categoryname == categoryname.ToLower()).ToListAsync();
                    if (result.Count == 0)
                    {
                        response.isSuccess = false;
                        response.errortext = "No FAQs found for the given category";
                        return response;
                    }
                    faqs = result;
                    response.faqs = faqs;
                    response.isSuccess = true;
                    response.errortext = string.Empty;
                    return response;
                }
                else if (!string.IsNullOrEmpty(searchtext))
                {
                    var result = await _context.faq
                        .Where(f => EF.Functions.ILike(f.question, "%" + searchtext + "%") ||
                                    EF.Functions.ILike(f.answer, "%" + searchtext + "%"))
                        .ToListAsync();
                    if (result.Count == 0)
                    {
                        response.isSuccess = false;
                        response.errortext = "No FAQs found for the given search text";
                        return response;
                    }
                    faqs = result;
                    response.faqs = faqs;
                    response.isSuccess = true;
                    response.errortext = string.Empty;
                    return response;
                }
                else
                {
                    var result = await _context.faq.ToListAsync();
                    if (result.Count == 0)
                    {
                        response.isSuccess = false;
                        response.errortext = "No FAQs found";
                        return response;
                    }
                    response.faqs = result;
                    response.isSuccess = true;
                    response.errortext = string.Empty;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.errortext = ex.Message;
            }
            return response;
        }

        [HttpPost("AddFAQ")]
        public async Task<CommanResponse> AddFAQ(FAQInput faqInput)
        {
            CommanResponse response = new CommanResponse();
            try
            {
                if (!await UserExists(faqInput.createdby))
                {
                    response.isSuccess = false;
                    response.message = "Failed to add FAQ";
                    response.errorText = "User does not exist";
                    return response;
                }
                else
                {

                    var sql = "Call insert_faq({0},{1},{2},{3},{4},{5});";
                    await _context.Database.ExecuteSqlRawAsync
                    (sql,
                    faqInput.question,
                    faqInput.answer,
                    faqInput.categoryid,
                    string.Join(",",
                    faqInput.tags),
                    faqInput.createdby,
                    faqInput.categoryname);

                    response.message = "FAQ added successfully";
                    response.isSuccess = true;
                    response.errorText = string.Empty;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.message = "Failed to add FAQ";
                response.errorText = ex.Message;
            }
            return response;
        }

        [HttpPost("UpdateFAQ")]
        public async Task<CommanResponse> UpdateFAQ(FAQInput faqInput)
        {
            CommanResponse response = new CommanResponse();
            try
            {
                //var conn = new Npgsql.NpgsqlConnection(connectionString);
                //conn.Open();
                var sql = "Call update_faq({0},{1},{2},{3},{4});";
                await _context.Database.ExecuteSqlRawAsync
                (sql,
                faqInput.faqid,
                faqInput.question,
                faqInput.answer,
                faqInput.categoryid,
                faqInput.createdby);

                response.message = "FAQ updated successfully";
                response.isSuccess = true;
                response.errorText = string.Empty;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.message = "Failed to update FAQ";
                response.errorText = ex.Message;
            }
            return response;
        }

        [HttpPost("DeleteFAQ")]
        public async Task<CommanResponse> DeleteFAQ(FAQDeleteInput fAQDeleteInput)
        {
            CommanResponse response = new CommanResponse();
            try
            {
                var sql = "Call delete_faq({0});";
                await _context.Database.ExecuteSqlRawAsync
                (sql,
                fAQDeleteInput.faqid);

                response.message = "FAQ deleted successfully";
                response.isSuccess = true;
                response.errorText = string.Empty;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.message = "Failed to delete FAQ";
                response.errorText = ex.Message;
            }
            return response;
        }

        [HttpPost("AddCategory")]
        public async Task<CommanResponse> AddCategory(CategoryInput categoryInput)
        {
            CommanResponse response = new CommanResponse();
            try
            {
                //var conn = new Npgsql.NpgsqlConnection(connectionString);
                //conn.Open();
                var sql = "Call insert_category({0},{1});";
                await _context.Database.ExecuteSqlRawAsync
                (sql,
                categoryInput.categoryname,
                categoryInput.userid);


                response.message = "Category added successfully";
                response.isSuccess = true;
                response.errorText = string.Empty;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.message = "Failed to add FAQ";
                response.errorText = ex.Message;
            }
            return response;
        }

        [HttpGet("GetCategories")]
        public async Task<CategoryResponse> GetCategoriesAsync(int? pageNumber = null, int? pageSize = null)
        {
            var categories = new List<CategoryList>();
            CategoryResponse response = new CategoryResponse();
            try
            {
                var query = _context.category.AsQueryable();
                var totalCount = await query.CountAsync();

                var pagesCategories = (pageNumber.HasValue && pageSize.HasValue)
                    ? await query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value).ToListAsync()
                    : await query.ToListAsync();

                var result = pagesCategories; // await _context.category.ToListAsync();
                if (result.Count == 0)
                {
                    response.isSuccess = false;
                    response.errorText = "No Categories found";
                    return response;
                }
                categories = result; 
                response.categories = categories;
                response.isSuccess = true;
                response.errorText = string.Empty;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.errorText = ex.Message;
                   
            }
            return response;
        }

        private async Task<bool> UserExists(int id)
        {
            try
            {
                return await _context.users.AnyAsync(u => u.userid == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DB check failed: {ex.Message}");
                return false;
            }

        }

        // AI Intgreated FAQ methods can be added.
        [HttpPost("GetAIGeneratedFAQs")]
        public async Task<AIApiResponse> GetAIGeneratedFAQs(AIApiInput ai)
        {
            AIApiResponse response = new AIApiResponse();
            LLMService service = new LLMService(_configuration, _httpClient);
            try
            {
                Console.WriteLine(ai.prompt);
                response = service.GetSuggestionsAsync(ai);              
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.errorText = ex.Message;
            }
            return response;
        }   
    }

}