using Project_EOS.Models.Dao;
using Project_EOS.Models.Entities;
using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_EOS.Areas.Admin.Controllers
{
    public class EmployeesManageController : Controller
    {
        // GET: Admin/EmployeesManage
        public ActionResult Index()
        {
            var employees = EmployeeDAO.Instance.GetAllEmployees();
            var services = ServiceDAO.Instance.GetAllServices(); // Lấy danh sách dịch vụ từ ServiceDAO
            ViewBag.employees = employees;
            ViewBag.Departments = new SelectList(EmployeeDAO.Instance.GetAllDepartments(), "DepartmentID", "DepartmentName");
            ViewBag.Services = new SelectList(services, "ServiceID", "ServiceName"); // Truyền danh sách dịch vụ vào ViewBag
            return View();
        }


        public ActionResult Create()
        {
            var departments = EmployeeDAO.Instance.GetAllDepartments();
            ViewBag.Departments = new SelectList(departments, "DepartmentID", "DepartmentName");
            return View();
        }

        [HttpPost]
        public JsonResult AddEmployee(NewEmployeeDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Email = HttpUtility.UrlDecode(model.Email);

                    var newEmployee = new Employee
                    {
                        EmployeeName = model.EmployeeName,
                        DepartmentID = model.DepartmentID,
                        Designation = model.Designation,
                        Email = model.Email,
                        Salary = model.Salary,
                        ServiceID = model.ServiceID,
                        JoinDate = model.JoinDate,
                        Is_Active = model.Is_Active,
                        // Tạo giá trị ngẫu nhiên cho Username và Password
                        Username = GenerateRandomString(6),
                        Password = GenerateRandomPassword(8)
                    };

                    EmployeeDAO.Instance.AddEmployee(newEmployee);

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            else
            {
                return Json(new { success = false, message = "Invalid input data" });
            }
        }

        public ActionResult EmployeeDetail(int employeeID)
        {
            var employee = EmployeeDAO.Instance.GetEmployeeById(employeeID);
            if (employee != null)
            {
                ViewBag.Employee = employee;

                // Lấy danh sách dịch vụ từ ServiceDAO
                var services = ServiceDAO.Instance.GetAllServices();
                ViewBag.Services = new SelectList(services, "ServiceID", "ServiceName");

                // Lấy danh sách phòng ban từ EmployeeDAO
                ViewBag.Departments = new SelectList(EmployeeDAO.Instance.GetAllDepartments(), "DepartmentID", "DepartmentName");

                return View(employee);
            }
            else
            {
                return HttpNotFound();
            }
        }


        [HttpPost]
        public JsonResult EditEmployee(EmployeeViewModel model)
        {
            try
            {
                Console.WriteLine("Start processing EditEmployee action");
                Console.WriteLine($"Employee ID: {model.EmployeeID}");
                Console.WriteLine($"Department ID: {model.DepartmentID}");
                Console.WriteLine($"Service ID: {model.ServiceID}");
                Console.WriteLine($"Designation: {model.Designation}");
                Console.WriteLine($"Salary: {model.Salary}");

                if (ModelState.IsValid)
                {
                    Console.WriteLine("ModelState is valid. Attempting to update employee...");
                    EmployeeDAO.Instance.UpdateEmployee(model);
                    Console.WriteLine("Employee updated successfully");
                    return Json(new { success = true });
                }
                else
                {
                    Console.WriteLine("ModelState is not valid. Sending error response...");

                    // Lấy danh sách các lỗi xác thực từ ModelState và chuyển đổi thành danh sách thông báo lỗi
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { success = false, message = "Invalid input data", errors = errors });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
            finally
            {
                Console.WriteLine("End processing EditEmployee action");
            }
        }




        // Hàm sinh chuỗi ngẫu nhiên
        private string GenerateRandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Hàm sinh mật khẩu ngẫu nhiên
        private string GenerateRandomPassword(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}