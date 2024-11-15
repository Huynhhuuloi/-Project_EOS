using Project_EOS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Services.Description;
using Service = Project_EOS.Models.Entities.Service;

namespace Project_EOS.Models.Dao
{
    public class ServiceDAO
    {
        private static ServiceDAO instance = null;
        private ServiceDAO() { }
        public static ServiceDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServiceDAO();
                }
                return instance;

            }
        }
        public ICollection<Service> GetAllServices()
        {
            try
            {
                Excell_On_ServiceEntities en = new Excell_On_ServiceEntities();
                var services = en.Services.ToList();
                return services;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public void AddServices(Service model)
        {
            try
            {
                Excell_On_ServiceEntities en = new Excell_On_ServiceEntities();
                en.Services.Add(model);
                en.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}");
            }
        }

        public IEnumerable<Service> GetAllServicesById(int id)
        {
            try
            {
                Excell_On_ServiceEntities en = new Excell_On_ServiceEntities();
                var serviceList = en.Services.Where(s => s.ServiceID == id).ToList();
                return serviceList;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public Service GetCampaignDetailById(int id)
        {
            try
            {
                Excell_On_ServiceEntities en = new Excell_On_ServiceEntities();
                var serviceDetails = en.Services.FirstOrDefault(s => s.ServiceID == id);
                return serviceDetails;
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}