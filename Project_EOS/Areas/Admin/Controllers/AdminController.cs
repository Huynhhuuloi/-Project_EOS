using Project_EOS.Models.Dao;
using Project_EOS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Project_EOS.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoginAdmin() { return View(); }
        [HttpPost]
        public ActionResult LoginAdmin(string UserName, string Password)
        {
            try
            {
                if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
                {
                    ViewBag.Error = "Username and password are required fields.";
                    return View("Login");
                }

                using (var context = new Excell_On_ServiceEntities())
                {

                    var user = context.Employees.FirstOrDefault(u => u.Username == UserName);

                    if (user != null && user.Password == Password)
                    {

                        Session.Timeout = 30;
                        Session["Username"] = user.EmployeeName;
                        Session["UserID"] = user.EmployeeID;
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {

                        ViewBag.Error = "Invalid username or password.";
                        return View("Login");
                    }
                }
            }
            catch (Exception ex)
            {

                ViewBag.Error = "An error occurred while processing your request. Please try again later.";

                Console.WriteLine(ex.Message);

                return View("Error");
            }
        }
        public ActionResult Client()
        {
            ViewBag.client = ClientDao.Instance.GetClients();
            return View();
        }
        [HttpPost]
        public ActionResult Addclient(Client model, HttpPostedFileBase image)
        {
            ClientDao.Instance.AddClient(model, image);
            return RedirectToAction("Client");
        }
        public ActionResult ShowClient()
        {
            if (Session["ClientID"] != null)
            {
                ViewBag.client = ClientDao.Instance.GetClients();
                return View();
            }
            else
            {
                return View();
            }
        }

        public ActionResult ShowClientDetails(int? id)
        {
            ViewBag.client = ClientDao.Instance.GetClientById(id.Value);
            return View();
        }
        [HttpPost]
        public ActionResult UpdateClient(int? ClientID, Client model, HttpPostedFileBase image)
        {
            // Check if ClientID is null
            if (ClientID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Validate the model state
            if (ModelState.IsValid)
            {
                // Set the ClientID to the model
                model.ClientID = ClientID.Value;

                // Update the client using ClientDao and pass the image
                var result = ClientDao.Instance.UpdateClient(model);

                // Handle the result of the update operation
                if (result == 1)
                {
                    return RedirectToAction("Client", "Admin");
                }
                else if (result == 2)
                {
                    return HttpNotFound();
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update client info.");
                }
            }

            // If the model state is invalid, or update fails, return the view with the model
            return View(model);
        }


        [HttpPost]
        public ActionResult DeleteClient(int? ClientID)
        {
            // Check if ClientID is null
            if (ClientID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid client ID");
            }

            // Attempt to delete the client and store the result
            int result = ClientDao.Instance.DeleteClient(ClientID.Value);

            // Handle the result of the delete operation using a switch statement
            switch (result)
            {
                case 1:
                    // Client successfully deleted
                    TempData["Message"] = "Client deleted successfully.";
                    break;
                case 2:
                    // Client not found
                    TempData["Message"] = "Client not found.";
                    break;
                default:
                    // Deletion failed
                    TempData["Message"] = "An error occurred while trying to delete the client.";
                    break;
            }

            // Redirect to the Client action in the Admin controller
            return RedirectToAction("Client", "Admin");
        }
        //public ActionResult Employee()
        //{
        //    ViewBag.employee = EmployeeDao.Instance.GetEmployee();
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult AddEmployee(Employee model)
        //{
        //    EmployeeDao.Instance.AddEmployee(model);
        //    return RedirectToAction("Employee");
        //}
        //public ActionResult ShowEmployeeDetail(int? id)
        //{
        //    ViewBag.employee = EmployeeDao.Instance.GetEmployeeById(id.Value);
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult UpdateEmployee(int? EmployeeID, Employee model)
        //{
        //    // Check if EmployeeID is null
        //    if (EmployeeID == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    try
        //    {
        //        // Validate the model state
        //        if (ModelState.IsValid)
        //        {
        //            // Set the EmployeeID to the model
        //            model.EmployeeID = EmployeeID.Value;

        //            // Update the employee using EmployeeDao
        //            var result = EmployeeDao.Instance.UpdateEmployee(model);

        //            // Handle the result of the update operation
        //            switch (result)
        //            {
        //                case 1:
        //                    return RedirectToAction("Employee", "Admin");
        //                case 2:
        //                    return HttpNotFound();
        //                default:
        //                    ModelState.AddModelError("", "Failed to update employee info.");
        //                    return View(model);
        //            }
        //        }
        //        else
        //        {
        //            // If the model state is invalid, return the view with the model
        //            return View(model);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception for debugging purposes
        //        Debug.WriteLine($"Error in UpdateEmployee method: {ex}");
        //        ModelState.AddModelError("", "An error occurred while processing your request.");
        //        return View(model);
        //    }
        //}


        //[HttpPost]
        //public ActionResult DeleteEmployee(int? EmployeeID)
        //{
        //    // Check if ClientID is null
        //    if (EmployeeID == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid client ID");
        //    }

        //    // Attempt to delete the client and store the result
        //    int result = EmployeeDao.Instance.DeleteEmployee(EmployeeID.Value);

        //    // Handle the result of the delete operation using a switch statement
        //    switch (result)
        //    {
        //        case 1:
        //            // Client successfully deleted
        //            TempData["Message"] = "Employee deleted successfully.";
        //            break;
        //        case 2:
        //            // Client not found
        //            TempData["Message"] = "Employee not found.";
        //            break;
        //        default:
        //            // Deletion failed
        //            TempData["Message"] = "An error occurred while trying to delete the Employee.";
        //            break;
        //    }

        //    // Redirect to the Client action in the Admin controller
        //    return RedirectToAction("Employee", "Admin");
        //}
        public ActionResult Order()
        {
            return View();
        }
        public ActionResult ShowOrderDetails(int? id)
        {
            ViewBag.order = OrderDao.Instance.GetOrderByIdClient(id.Value);
            ViewBag.client = ClientDao.Instance.GetClientDetailById(id.Value);
            return View();
        }
    }
}