using Project_EOS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace Project_EOS.Models.Dao
{
    public class EmployeeDAO
    {
        private static EmployeeDAO instance = null;
        private Excell_On_ServiceEntities _dbContext;

        private EmployeeDAO()
        {
            _dbContext = new Excell_On_ServiceEntities();
        }

        public static EmployeeDAO Instance
        {
            get
            {
                if (instance == null) { instance = new EmployeeDAO(); }
                return instance;
            }
        }

        public ICollection<EmployeeViewModel> GetAllEmployees()
        {
            try
            {
                var allEmployees = (from e in _dbContext.Employees
                                    join d in _dbContext.Departments on e.DepartmentID equals d.DepartmentID
                                    join s in _dbContext.Services on e.ServiceID equals s.ServiceID
                                    select new EmployeeViewModel
                                    {
                                        EmployeeID = e.EmployeeID,
                                        EmployeeName = e.EmployeeName,
                                        DepartmentID = e.DepartmentID,
                                        DepartmentName = d.DepartmentName,
                                        Designation = e.Designation,
                                        ServiceID = e.ServiceID,
                                        ServiceName = s.ServiceName,
                                        Salary = e.Salary,
                                        Email = e.Email,
                                        JoinDate = e.JoinDate,
                                        Is_Active = e.Is_Active
                                    }).ToList();
                return allEmployees;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public ICollection<DepartmentViewModel> GetAllDepartments()
        {
            try
            {
                var allDepartments = (from d in _dbContext.Departments
                                      select new DepartmentViewModel
                                      {
                                          DepartmentID = d.DepartmentID,
                                          DepartmentName = d.DepartmentName
                                      }).ToList();
                return allDepartments;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public class DepartmentViewModel
        {
            public int DepartmentID { get; set; }
            public string DepartmentName { get; set; }
        }


        public void AddEmployee(Employee model)
        {
            try
            {
                _dbContext.Employees.Add(model);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
        public void UpdateEmployee(EmployeeViewModel model)
        {
            try
            {
                var employeeToUpdate = _dbContext.Employees.FirstOrDefault(e => e.EmployeeID == model.EmployeeID);
                if (employeeToUpdate != null)
                {
                    employeeToUpdate.DepartmentID = model.DepartmentID;
                    employeeToUpdate.ServiceID = model.ServiceID;
                    employeeToUpdate.Designation = model.Designation;
                    employeeToUpdate.Salary = model.Salary;

                    _dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("Employee not found.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }




        internal object GetEmployeeByDepartmentID(int departmentID)
        {
            var employees = _dbContext.Employees.Where(e => e.DepartmentID == departmentID).ToList();
            return employees;
        }
        public string GetDepartmentName(int departmentID)
        {
            return _dbContext.Departments.Where(d => d.DepartmentID == departmentID).Select(d => d.DepartmentName).FirstOrDefault();
        }

        public string GetServiceName(int serviceID)
        {
            return _dbContext.Services.Where(s => s.ServiceID == serviceID).Select(s => s.ServiceName).FirstOrDefault();
        }

        public EmployeeViewModel GetEmployeeById(int employeeID)
        {
            try
            {
                var employee = (from e in _dbContext.Employees
                                join d in _dbContext.Departments on e.DepartmentID equals d.DepartmentID
                                join s in _dbContext.Services on e.ServiceID equals s.ServiceID
                                where e.EmployeeID == employeeID
                                select new EmployeeViewModel
                                {
                                    EmployeeID = e.EmployeeID,
                                    EmployeeName = e.EmployeeName,
                                    DepartmentID = e.DepartmentID,
                                    DepartmentName = d.DepartmentName,
                                    Designation = e.Designation,
                                    ServiceID = e.ServiceID,
                                    ServiceName = s.ServiceName,
                                    Salary = e.Salary,
                                    Email = e.Email,
                                    JoinDate = e.JoinDate,
                                    Is_Active = e.Is_Active
                                }).FirstOrDefault();
                return employee;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public void NewClient(Client model, HttpPostedFileBase imageFile)
        {
            try
            {
                Excell_On_ServiceEntities en = new Excell_On_ServiceEntities();


                // Gán giá trị cho các trường cần thiết
           
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    string fileName = DateTime.Now.Ticks + Path.GetFileName(imageFile.FileName);
                    string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/asset/Image"), fileName);
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
    }




    public class EmployeeViewModel
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string Designation { get; set; }

        public int ServiceID { get; set; }

        public string ServiceName { get; set; }
        public decimal Salary { get; set; }
        public string Email { get; set; }
        public DateTime JoinDate { get; set; }
        public bool? Is_Active { get; set; }
    }

    public class NewEmployeeDTO
    {
        public string EmployeeName { get; set; }
        public int DepartmentID { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public decimal Salary { get; set; }
        public int ServiceID { get; set; }
        public DateTime JoinDate { get; set; }
        public bool Is_Active { get; set; }
    }

}