using System;
using System.Web.Mvc;
using Project_EOS.Models.Dao;
using Project_EOS.Models.Entities;
using static Project_EOS.Models.Dao.SearchDAO;

namespace Project_EOS.Areas.Admin.Controllers
{
    public class SearchController : Controller
    {
        private readonly SearchDAO _dao;
        private readonly Excell_On_ServiceEntities _dbContext;

        public SearchController()
        {
            _dbContext = new Excell_On_ServiceEntities();
            _dao = new SearchDAO(_dbContext);
        }

        [HttpGet]

        public ActionResult Index(string query)
        {
            try
            {
                // Kiểm tra query có rỗng không
                if (string.IsNullOrWhiteSpace(query))
                {
                    // Nếu query rỗng, trả về view với thông báo "No query provided."
                    Console.WriteLine("No query provided.");
                    return View("Index", new SearchResultViewModel { Query = query });
                }

                // Gọi phương thức Search trong DAO để tìm kiếm
                var viewModel = _dao.Search(query);

                // Kiểm tra kết quả từ phương thức Search
                if (viewModel != null)
                {
                    // Ghi lại thông tin về kết quả tìm kiếm
                    Console.WriteLine($"Number of services found: {viewModel.Services.Count}");
                    Console.WriteLine($"Number of employees found: {viewModel.Employees.Count}");
                    Console.WriteLine($"Number of clients found: {viewModel.Clients.Count}");
                    Console.WriteLine($"Number of departments found: {viewModel.Departments.Count}");
                    Console.WriteLine($"Number of products found: {viewModel.Products.Count}");

                    // Trả về view với kết quả tìm kiếm
                    return View("Index", viewModel);
                }
                else
                {
                    // Trong trường hợp không tìm thấy kết quả, trả về view với thông báo "No results found."
                    Console.WriteLine("No results found.");
                    return View("Index", new SearchResultViewModel { Query = query });
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ nếu có
                Console.WriteLine($"Error: {ex.Message}");
                return View("Index", new SearchResultViewModel { Query = query });
            }
        }
    }
}