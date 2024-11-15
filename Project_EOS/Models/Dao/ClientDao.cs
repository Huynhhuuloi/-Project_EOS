using Project_EOS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace Project_EOS.Models.Dao
{
    public class ClientDao
    {
        private static ClientDao instance = null;

        private ClientDao() { }

        public static ClientDao Instance
        {
            get
            {
                if (instance == null) { instance = new ClientDao(); }
                return instance;
            }
        }
        public ICollection<Client> GetClients()
        {
          
                try
            {
                Excell_On_ServiceEntities en = new Excell_On_ServiceEntities();
                var res1 = en.Clients.ToList();
                return res1;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public void AddClient(Client model, HttpPostedFileBase imageFile)
        {
            try
            {
                Excell_On_ServiceEntities en = new Excell_On_ServiceEntities();

                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    string fileName = DateTime.Now.Ticks + Path.GetFileName(imageFile.FileName);
                    string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Image"), fileName);
                    imageFile.SaveAs(path);
                    model.Image = fileName;
                    // Debug
                    Debug.WriteLine($"File saved successfully to: {path}");
                }

                en.Clients.Add(model);
                en.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public int DeleteClient(int id)
        {
            try
            {
                Excell_On_ServiceEntities en = new Excell_On_ServiceEntities();
                var q = en.Clients.Where(d => d.ClientID.Equals(id)).FirstOrDefault();
                if (q != null)
                {
                    en.Clients.Remove(q);
                    en.SaveChanges();
                    return 1; //delete 
                }
                return 2; // not found the item by id
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException.Message);
                string err = ex.Message;
            }
            return 0; // failed

        }
        public IEnumerable<Client> GetClientById(int id)
        {
            try
            {
                Excell_On_ServiceEntities en = new Excell_On_ServiceEntities();
                var clientDetails = en.Clients.Where(c => c.ClientID == id).ToList();
                return clientDetails;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public Client GetClientDetailById(int id)
        {
            try
            {
                Excell_On_ServiceEntities en = new Excell_On_ServiceEntities();
                var client = en.Clients.FirstOrDefault(c => c.ClientID == id);
                return client;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public int UpdateClient(Client model)
        {
            try
            {
                using (Excell_On_ServiceEntities en = new Excell_On_ServiceEntities())
                {
                    // Fetch the existing client based on ClientID
                    var existingClient = en.Clients.FirstOrDefault(d => d.ClientID == model.ClientID);
                    if (existingClient != null)
                    {
                        // Update the existing client with the new values from the model
                        existingClient.ClientName = model.ClientName;
                        existingClient.CompanyName = model.CompanyName;
                        existingClient.ContactPerson = model.ContactPerson;
                        existingClient.ContactNumber = model.ContactNumber;
                        existingClient.Email = model.Email;
                        existingClient.Address = model.Address;
                        existingClient.Username = model.Username;
                        en.SaveChanges();
                        return 1; // Update successful
                    }
                    return 2; // Client not found
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Debug.WriteLine($"Error in UpdateClient method: {ex}");
                return 0; // An error occurred
            }
        }

        public int DeleteClient(int? id)
        {
            if (id == null)
            {
                return 2; // Invalid id (null)
            }

            try
            {
                using (Excell_On_ServiceEntities en = new Excell_On_ServiceEntities())
                {
                    var client = en.Clients.FirstOrDefault(d => d.ClientID == id.Value);
                    if (client != null)
                    {
                        en.Clients.Remove(client);
                        en.SaveChanges();
                        return 1; // Successfully deleted
                    }
                    return 2; // Client not found
                }
            }
            catch (Exception ex)
            {
                // Log the full exception details including inner exception if it exists
                Debug.WriteLine(ex.ToString());
                return 0; // Deletion failed
            }
        }



    }
}