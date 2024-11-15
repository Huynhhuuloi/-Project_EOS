using Project_EOS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace Project_EOS.Models.Dao
{
    public class DepartmentDAO
    {
        private static DepartmentDAO instance = null;
        private Excell_On_ServiceEntities _dbContext;

        private DepartmentDAO()
        {
            _dbContext = new Excell_On_ServiceEntities();

        }

        public static DepartmentDAO Instance
        {
            get
            {
                if (instance == null) { instance = new DepartmentDAO(); }
                return instance;
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
                                          DepartmentName = d.DepartmentName,
                                          DepartmentDescription = d.DepartmentDescription
                                      }).ToList();
                return allDepartments;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }


        public ICollection<DepartmentViewModel> GetEmployeesByDepartmentID(int departmentID)
        {
            try
            {
                var allEmployees = (from e in _dbContext.Employees
                                    join d in _dbContext.Departments on e.DepartmentID equals d.DepartmentID
                                    where e.DepartmentID == departmentID
                                    select new DepartmentViewModel
                                    {
                                        EmployeeID = e.EmployeeID,
                                        EmployeeName = e.EmployeeName,
                                        Is_Active = e.Is_Active,
                                        DepartmentName = d.DepartmentName,
                                        DepartmentDescription = d.DepartmentDescription
                                    }).ToList();
                return allEmployees;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public void AddDepartment(Department model)
        {
            try
            {
                Excell_On_ServiceEntities en = new Excell_On_ServiceEntities();
                en.Departments.Add(model);
                en.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        public Department GetDepartmentByID(int departmentID)
        {
            try
            {
                return _dbContext.Departments.Find(departmentID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public void UpdateDepartment(Department department)
        {
            try
            {
                _dbContext.Entry(department).State = System.Data.Entity.EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


    }
    public class DepartmentViewModel
    {
        public int DepartmentID { get; set; }
        public int? EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentDescription { get; set; }
        public bool? Is_Active { get; set; }

    }
}