using Project_EOS.Models.Dao;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_EOS.Areas.Admin.Controllers
{
    public class PaymentManageController : Controller
    {
        // GET: Admin/PaymentManage
        public ActionResult Index()
        {
            List<PaymentViewModel> payments = PaymentDAO.Instance.GetAllPayments().ToList();
            ViewBag.payments = payments;
            return View();
        }


        public ActionResult PaymentsByClient(int? clientID, string clientName)
        {
            try
            {
                if (clientID == null)
                {
                    // Nếu clientID không có giá trị, chuyển hướng đến một trang thông báo lỗi
                    return RedirectToAction("Index", "PaymentManage", new { area = "Admin" });
                }

                ViewBag.ClientName = clientName;
                List<PaymentViewModel> paymentsbyclient = PaymentDAO.Instance.GetPaymentsByClientID(clientID.Value).ToList();
                ViewBag.payments = paymentsbyclient;
                return View();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occurred in PaymentsByClient action: " + ex.Message);
                throw; // Re-throw the exception to propagate it further
            }
        }

        [HttpPost]
        public ActionResult UpdatePayment(int paymentID, DateTime paymentDate, string paymentMethod)
        {
            try
            {
                // Tìm thanh toán trong cơ sở dữ liệu bằng paymentID
                var payment = PaymentDAO.Instance.GetPaymentByID(paymentID);

                if (payment != null)
                {
                    // Cập nhật chi tiết thanh toán
                    payment.PaymentDate = paymentDate;
                    payment.PaymentMethod = paymentMethod;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    PaymentDAO.Instance.UpdatePayment(payment);

                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Payment not found" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occurred in UpdatePayment action: " + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}