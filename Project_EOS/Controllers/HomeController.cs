using Microsoft.AspNetCore.Mvc;
using NLog;
using Project_EOS.Models;
using Project_EOS.Models.Dao;
using Project_EOS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Project_EOS.Controllers
{
    public class HomeController : Controller
    {
        Excell_On_ServiceEntities db = new Excell_On_ServiceEntities("");
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult LoginUser(string succes, string err)
        {
            ViewData["succes"] = null;
            ViewData["err"] = null;
            if (succes != null)
            {
                ViewData["succes"] = succes;
            }
            if (err != null)
            {
                ViewData["err"] = err;
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Remove("KH");

            return RedirectToAction("Index");

        }
        [HttpPost]
        public ActionResult Login(FormCollection formCollection)
        {
            string emailOrPhone = formCollection["loginInput"];
            string password = formCollection["passwordInput"];
            // Kiểm tra thông tin đăng nhập trong cơ sở dữ liệu
            var user = db.Clients
                .SingleOrDefault(u => (/*u.Username == emailOrPhone ||*/ u.Email == emailOrPhone) && u.Password == password);

            if (user != null)
            {
                Session["KH"] = user;
                if (Session["PreviousPage"] != null)
                {
                    var previousPage = Session["PreviousPage"].ToString();
                    Session.Remove("PreviousPage"); // Xóa thông tin trang trước đó sau khi sử dụng
                    return Redirect(previousPage);
                }

                // Nếu không có trang trước đó, chuyển hướng về trang mặc định
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Đăng nhập không thành công, trả về thông báo lỗi
                return RedirectToAction("LoginUser", new { err = "Thông tin đăng nhập không đúng" });
            }
        }
        // Hàm kiểm tra sự tồn tại của người dùng
        private bool CheckUserExists(string inputType, string inputInfo)
        {

            // Kiểm tra sự tồn tại của người dùng với số điện thoại hoặc email
            if (inputType.ToLower() == "phone")
            {
                // Kiểm tra số điện thoại
                return db.Employees.Any(u => u.Username == inputInfo);
            }
            else if (inputType.ToLower() == "email")
            {
                // Kiểm tra email
                return db.Employees.Any(u => u.Email == inputInfo);
            }

            // Trường hợp không xác định loại thông tin, trả về false
            return false;

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string currentPassword, string newPassword)
        {
            var nguoidung = Session["KH"] as Client;
            int userId = nguoidung.ClientID;

            var user = db.Clients.FirstOrDefault(u => u.ClientID == userId);

            // Kiểm tra mật khẩu hiện tại
            if (user != null && user.Password == currentPassword)
            {
                user.Password = newPassword;
                db.SaveChanges();
                return RedirectToAction("EditTT", new { id = nguoidung.ClientID, succes = "Đổi mật khẩu thành công." });
            }
            else
            {
                return RedirectToAction("EditTT", new { id = nguoidung.ClientID, err = "Mật khẩu hiện tại không đúng." });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(string ClientName, string email)
        {
            var nguoidung = Session["KH"] as Client;
            int userId = nguoidung.ClientID;

            var user = db.Clients.FirstOrDefault(u => u.ClientID == userId);

            // Kiểm tra mật khẩu hiện tại
            if (user != null)
            {
                user.ClientName = ClientName;
             
                user.Email = email;
                db.SaveChanges();
                return RedirectToAction("EditTT", new { id = nguoidung.ClientID, succes = "Đổi thông tin thành công." });
            }
            else
            {
                return RedirectToAction("EditTT", new { id = nguoidung.ClientID, err = "Đổi thông tin không thành công." });
            }
        }
        [HttpPost]
        public ActionResult DK(FormCollection form)
        {
            string hoTen = form["HoTen"];
            string email = form["Email"];
            string taikhoan = form["TaiKhoan"];
            string matKhau = form["MatKhau"];



            // Kiểm tra xem email đã tồn tại trong cơ sở dữ liệu hay chưa
            if (db.Clients.Any(x => x.Email == email))
            {
                return RedirectToAction("Login", new { err = "Email đã tồn tại" });
            }
            // Kiểm tra và xử lý dữ liệu nếu cần
            if (!string.IsNullOrEmpty(hoTen) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(matKhau))
            {
                // Mã hóa mật khẩu bằng SHA-256


                var thongTinCaNhan = new Client
                {
                    ClientName = hoTen,
                    Email = email,
                    Username = taikhoan,
                    Password = matKhau,

                };
                
                db.Clients.Add(thongTinCaNhan);
                db.SaveChanges();

                return RedirectToAction("LoginUser", "Home", new { succes = "Đăng ký thành công" });
            }

            // Nếu dữ liệu không hợp lệ, quay lại view đăng ký
            return RedirectToAction("DK", "Home", new { err = "Vui lòng nhập đầy đủ thông tin" });
        }



        public ActionResult ThongTin()
        {
            var nguoidung = Session["KH"] as Client;
            var tt = db.Clients.SingleOrDefault(u => u.ClientID == nguoidung.ClientID);
            return View(tt);
        }
        public ActionResult EditTT(int id, string succes, string err)
        {
            ViewData["succes"] = null;
            ViewData["err"] = null;
            if (succes != null)
            {
                ViewData["succes"] = succes;
            }
            if (err != null)
            {
                ViewData["err"] = err;
            }
            var tt = db.Clients.SingleOrDefault(u => u.ClientID == id);
            return View(tt);
        }
        private void UpdatePassword(Client user, string newPassword)
        {

            user.Password = newPassword;
            db.SaveChanges();

        }
        [HttpPost]
        public ActionResult UpdateAccount(int? ClientID, Client model)
        {
            if (ClientID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "ClientID is required.");
            }

            if (ModelState.IsValid)
            {
                model.ClientID = ClientID.Value;
                var result = ClientDao.Instance.UpdateClient(model);

                switch (result)
                {
                    case 1:
                        return RedirectToAction("ThongTin");
                    case 2:
                        return HttpNotFound();
                    default:
                        ModelState.AddModelError("", "Failed to update user info.");
                        break;
                }
            }

            // Return the view with the model and error messages if the update fails
            return RedirectToAction("ThongTin");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int ClientID, Client model)
        {
            using (var context = new Excell_On_ServiceEntities())
            {

                // Use of lambda expression to access
                // particular record from a database
                var data = context.Clients.FirstOrDefault(x => x.ClientID == ClientID);

                // Checking if any such record exist 
                if (data != null)
                {
                    data.ClientName = model.ClientName;
                    data.CompanyName = model.CompanyName;
                    data.ContactPerson = model.ContactPerson;
                    data.ContactNumber = model.ContactNumber;
                    data.Email = model.Email;
                    data.Address = model.Address;
                    data.Username = model.Username;
                    context.SaveChanges();

                    // It will redirect to 
                    // the Read method
                    return RedirectToAction("ThongTin");
                }
                else
                    return View();
            }
        }

    }
}