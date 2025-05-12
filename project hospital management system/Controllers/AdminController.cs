﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using project_hospital_management_system.Models;
using project_hospital_management_system.ViewModels;
using project_hospital_management_system.Filters;

namespace project_hospital_management_system.Controllers
{
    [Authorize(Roles = "Admin")]
    [RequireHttps]
    [AdminAuthorizationFilter]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public AdminController()
        {
        }

        public AdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        // GET: Admin/Dashboard
        public ActionResult Dashboard()
        {
            try
            {
                // Get system statistics
                var dashboardViewModel = new AdminDashboardViewModel
                {
                    TotalPatients = db.Patients.Count(),
                    TotalDoctors = db.Doctors.Count(),
                    TotalAppointments = db.Appointments.Count(),
                    TotalMedicalRecords = db.MedicalRecords.Count(),
                    TotalMedications = db.Medications.Count(),
                    TotalPrescriptions = db.Prescriptions.Count(),
                    TotalInvoices = db.Invoices.Count(),
                    TotalUsers = UserManager.Users.Count(),

                    // Financial statistics
                    TotalRevenue = db.Invoices.Sum(i => i.TotalAmount),
                    PendingPayments = db.Invoices.Where(i => i.Status == "Pending").Sum(i => i.TotalAmount),
                    OverduePayments = db.Invoices.Where(i => i.Status == "Overdue").Sum(i => i.TotalAmount),

                    // Recent activity
                    RecentPatients = db.Patients.OrderByDescending(p => p.PatientId).Take(5).ToList(),
                    RecentAppointments = db.Appointments
                        .Include(a => a.Patient)
                        .Include(a => a.Doctor)
                        .OrderByDescending(a => a.AppointmentId)
                        .Take(5)
                        .ToList(),
                    RecentInvoices = db.Invoices
                        .Include(i => i.Patient)
                        .OrderByDescending(i => i.InvoiceId)
                        .Take(5)
                        .ToList()
                };

                // Get user counts by role
                dashboardViewModel.UsersByRole = new Dictionary<string, int>();
                foreach (var role in RoleManager.Roles.ToList())
                {
                    var usersInRole = UserManager.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id)).Count();
                    dashboardViewModel.UsersByRole.Add(role.Name, usersInRole);
                }

                return View(dashboardViewModel);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in AdminController.Dashboard: " + ex.Message);

                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while loading the dashboard. Please try again later.";

                // Return an empty view model
                return View(new AdminDashboardViewModel());
            }
        }

        // GET: Admin/UserManagement
        public ActionResult UserManagement()
        {
            try
            {
                var users = UserManager.Users.ToList();
                var userViewModels = new List<UserViewModel>();

                foreach (var user in users)
                {
                    var roles = UserManager.GetRoles(user.Id);
                    userViewModels.Add(new UserViewModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        IsActive = user.IsActive,
                        DateCreated = user.DateCreated,
                        LastLoginDate = user.LastLoginDate,
                        Roles = roles.ToList(),
                        ProfilePicture = user.ProfilePicture
                    });
                }

                return View(userViewModels);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in AdminController.UserManagement: " + ex.Message);

                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while loading user data. Please try again later.";

                // Return an empty list
                return View(new List<UserViewModel>());
            }
        }

        // GET: Admin/RoleManagement
        public ActionResult RoleManagement()
        {
            try
            {
                var roles = RoleManager.Roles.ToList();
                var roleViewModels = new List<RoleViewModel>();

                foreach (var role in roles)
                {
                    roleViewModels.Add(new RoleViewModel
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Permissions = ApplicationRoles.RolePermissions.ContainsKey(role.Name)
                            ? ApplicationRoles.RolePermissions[role.Name]
                            : new List<string>()
                    });
                }

                return View(roleViewModels);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in AdminController.RoleManagement: " + ex.Message);

                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while loading role data. Please try again later.";

                // Return an empty list
                return View(new List<RoleViewModel>());
            }
        }

        // GET: Admin/SystemLogs
        public ActionResult SystemLogs()
        {
            // In a real application, this would retrieve system logs from a database or log files
            return View();
        }

        // GET: Admin/SystemSettings
        public ActionResult SystemSettings()
        {
            // In a real application, this would retrieve system settings from a database
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
                if (_roleManager != null)
                {
                    _roleManager.Dispose();
                    _roleManager = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
