using System.Collections.Generic;
using System.Diagnostics;
using System;
using Project_EOS.Models.Entities;
using System.Data.Entity.SqlServer;
using System.Linq;

namespace Project_EOS.Models.Dao
{
    public class SearchDAO
    {
        private readonly Excell_On_ServiceEntities _dbContext;

        public SearchDAO(Excell_On_ServiceEntities dbContext)
        {
            _dbContext = dbContext;
        }

        public SearchResultViewModel Search(string query)
        {
            try
            {
                // Ghi lại giá trị của biến query
                Console.WriteLine($"Query: {query}");

                var viewModel = new SearchResultViewModel
                {
                    Query = query,
                    Services = _dbContext.Services
                                .Where(s => s.ServiceName.Contains(query) || s.ServiceDescription.Contains(query))
                                .ToList(),

                    Employees = _dbContext.Employees
                                .Where(s => s.EmployeeName.Contains(query) || s.Designation.Contains(query))
                                .ToList(),

                    Clients = _dbContext.Clients
                                .Where(s => s.ClientName.Contains(query) || s.ContactPerson.Contains(query))
                                .ToList(),

                    Departments = _dbContext.Departments
                                .Where(s => s.DepartmentName.Contains(query) || s.DepartmentDescription.Contains(query))
                                .ToList(),

                    Products = _dbContext.Products
                                .Where(s => s.ProductName.Contains(query) || s.ProductDescription.Contains(query))
                                .ToList()

                };

                // Ghi lại số lượng kết quả tìm kiếm
                Console.WriteLine($"Services found: {viewModel.Services.Count}");
                Console.WriteLine($"Employees found: {viewModel.Employees.Count}");
                Console.WriteLine($"Clients found: {viewModel.Clients.Count}");
                Console.WriteLine($"Departments found: {viewModel.Departments.Count}");
                Console.WriteLine($"Products found: {viewModel.Products.Count}");

                return viewModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }


    }
    public class SearchResultViewModel
    {
        public List<Project_EOS.Models.Entities.Service> Services { get; set; } = new List<Project_EOS.Models.Entities.Service>();
        public List<Project_EOS.Models.Entities.Employee> Employees { get; set; } = new List<Project_EOS.Models.Entities.Employee>();
        public List<Project_EOS.Models.Entities.Client> Clients { get; set; } = new List<Project_EOS.Models.Entities.Client>();
        public List<Project_EOS.Models.Entities.Department> Departments { get; set; } = new List<Project_EOS.Models.Entities.Department>();
        public List<Project_EOS.Models.Entities.Product> Products { get; set; } = new List<Project_EOS.Models.Entities.Product>();
        public string Query { get; set; }
    }
}