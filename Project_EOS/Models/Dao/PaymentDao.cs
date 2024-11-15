using Project_EOS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Project_EOS.Models.Dao
{
    public class PaymentDAO
    {
        private static PaymentDAO instance = null;
        private Excell_On_ServiceEntities _dbContext;

        private PaymentDAO()
        {
            _dbContext = new Excell_On_ServiceEntities();

        }

        public static PaymentDAO Instance
        {
            get
            {
                if (instance == null) { instance = new PaymentDAO(); }
                return instance;
            }
        }

        public ICollection<PaymentViewModel> GetAllPayments()
        {
            try
            {
                var allPayments = (from p in _dbContext.Payments
                                   join c in _dbContext.Clients on p.ClientID equals c.ClientID
                                   select new PaymentViewModel
                                   {
                                       PaymentID = p.PaymentID,
                                       ClientID = c.ClientID,
                                       ClientName = c.ClientName,
                                       PaymentAmount = p.PaymentAmount,
                                       PaymentDate = p.PaymentDate,
                                       PaymentMethod = p.PaymentMethod,
                                   }).ToList();
                return allPayments;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public ICollection<PaymentViewModel> GetPaymentsByClientID(int clientID)
        {
            try
            {
                var paymentsByClient = _dbContext.Payments
                                                 .Where(p => p.ClientID == clientID)
                                                 .Select(p => new PaymentViewModel
                                                 {
                                                     PaymentID = p.PaymentID,
                                                     ClientID = p.ClientID,
                                                     PaymentAmount = p.PaymentAmount,
                                                     PaymentDate = p.PaymentDate,
                                                     PaymentMethod = p.PaymentMethod
                                                 })
                                                 .ToList();
                return paymentsByClient;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }



        public void AddPayment(Payment model)
        {
            try
            {
                _dbContext.Payments.Add(model);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        //internal object GetPaymentByClientID(int clientID)
        //{
        //    var paymentsbyclient = _dbContext.Payments.Where(c => c.ClientID == clientID).ToList();
        //    return paymentsbyclient;
        //}

        public Payment GetPaymentByID(int paymentID)
        {
            try
            {
                return _dbContext.Payments.Find(paymentID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public void UpdatePayment(Payment payment)
        {
            try
            {
                _dbContext.Entry(payment).State = System.Data.Entity.EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


    }
    public class PaymentViewModel
    {
        public int PaymentID { get; set; }
        public int? ClientID { get; set; }
        public string ClientName { get; set; }
        public decimal? PaymentAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
    }

}