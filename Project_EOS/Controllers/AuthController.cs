using Project_EOS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_EOS.Controllers
{
    public class AuthController : Controller
    {
        Excell_On_ServiceEntities db = new Excell_On_ServiceEntities("");
        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection formCollection)
        {
            string emailOrPhone = formCollection["username"];
            string password = formCollection["password"];
            // Kiểm tra thông tin đăng nhập trong cơ sở dữ liệu
            var admin = db.Clients
                .SingleOrDefault(u => (u.Username == emailOrPhone || u.ClientName == emailOrPhone) && u.Password == password);

            if (admin != null)
            {
                Session["Admin"] = admin;
                if (Session["PreviousPage"] != null)
                {
                    var previousPage = Session["PreviousPage"].ToString();
                    Session.Remove("PreviousPage"); // Xóa thông tin trang trước đó sau khi sử dụng
                    return Redirect(previousPage);
                }

                // Nếu không có trang trước đó, chuyển hướng về trang mặc định
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                // Đăng nhập không thành công, trả về thông báo lỗi
                return RedirectToAction("Login", new { err = "Thông tin đăng nhập không đúng" });
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

                return RedirectToAction("Login", "Auth", new { succes = "Đăng ký thành công" });
            }

            // Nếu dữ liệu không hợp lệ, quay lại view đăng ký
            return RedirectToAction("Register", "Auth", new { err = "Vui lòng nhập đầy đủ thông tin" });
        }
    }
}