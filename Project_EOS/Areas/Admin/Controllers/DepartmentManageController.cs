using Project_EOS.Models.Dao;
using Project_EOS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using static Project_EOS.Models.Dao.DepartmentDAO;

namespace Project_EOS.Areas.Admin.Controllers
{
    public class DepartmentManageController : Controller
    {
        // GET: Admin/PaymentManage
        public ActionResult Index()
        {
            try
            {
                List<DepartmentViewModel> departments = DepartmentDAO.Instance.GetAllDepartments().ToList();
                ViewBag.departments = departments;
                return View();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occurred in Index action: " + ex.Message);
                throw; // Re-throw the exception to propagate it further
            }

        }


        public ActionResult AllEmployeesByDepartment(int? DepartmentID, string departmentName)
        {
            try
            {

                if (DepartmentID == null)
                {
                    return RedirectToAction("Index", "DepartmentManage", new { area = "Admin" });
                }
                List<DepartmentViewModel> employees = DepartmentDAO.Instance.GetEmployeesByDepartmentID(DepartmentID.Value).ToList();
                int totalEmployees = employees.Count();
                ViewBag.DepartmentName = departmentName;
                ViewBag.employees = employees;
                ViewBag.totalEmployees = totalEmployees;

                return View();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occurred in Employees By DepartmentID action: " + ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        [HttpPost]
        public ActionResult AddDepartment(string DepartmentName, string DepartmentDescription)
        {
            try
            {
                Department newDepartment = new Department
                {
                    DepartmentName = DepartmentName,
                    DepartmentDescription = DepartmentDescription
                };
                DepartmentDAO.Instance.AddDepartment(newDepartment);

                // Trả về thông tin của phòng ban mới trong phản hồi của Ajax
                return Json(new
                {
                    success = true,
                    departmentID = newDepartment.DepartmentID,
                    departmentName = newDepartment.DepartmentName,
                    departmentDescription = newDepartment.DepartmentDescription
                });

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occurred in AddDepartment action: " + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult UpdateDepartment(int departmentID, string departmentDescription)
        {
            try
            {
                var department = DepartmentDAO.Instance.GetDepartmentByID(departmentID);

                if (department != null)
                {
                    department.DepartmentDescription = departmentDescription;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    DepartmentDAO.Instance.UpdateDepartment(department);

                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Department not found" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occurred in UpdatePayment action: " + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}