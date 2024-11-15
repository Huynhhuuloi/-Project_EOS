using Project_EOS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Project_EOS.Models.Dao
{
    public class OrderDao
    {
        private static OrderDao instance = null;

        private OrderDao() { }

        public static OrderDao Instance
        {
            get
            {
                if (instance == null) { instance = new OrderDao(); }
                return instance;
            }
        }
        //public Order GetOrderByIdClients(int id)
        //{
        //    try
        //    {
        //        Excell_On_ServiceEntities en = new Excell_On_ServiceEntities();
        //        var order = en.Orders.FirstOrDefault(c => c.ClientID == id);
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //        return null;
        //    }
        //}
        public IEnumerable<Order> GetOrderByIdClient(int id)
        {
            try
            {
                using (var context = new Excell_On_ServiceEntities())
                {
                    var orders = context.Orders.Where(o => o.ClientID == id).ToList();
                    return orders;
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Debug.WriteLine($"Error in GetOrderByIdClient method: {ex.Message}");
                return null;
            }
        }

    }
}