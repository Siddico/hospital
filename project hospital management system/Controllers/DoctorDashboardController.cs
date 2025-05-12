﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using project_hospital_management_system.Models;
using project_hospital_management_system.ViewModels;

namespace project_hospital_management_system.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorDashboardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: DoctorDashboard
        public ActionResult Index()
        {
            try
            {
                // Get the current user
                var userId = User.Identity.GetUserId();
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                
                if (user == null || !user.DoctorId.HasValue)
                {
                    TempData["ErrorMessage"] = "Doctor profile not found. Please contact the administrator.";
                    return View(new DoctorDashboardViewModel());
                }
                
                var doctorId = user.DoctorId.Value;
                var doctor = db.Doctors.Find(doctorId);
                
                if (doctor == null)
                {
                    TempData["ErrorMessage"] = "Doctor profile not found. Please contact the administrator.";
                    return View(new DoctorDashboardViewModel());
                }
                
                // Get doctor's appointments, patients, and prescriptions
                var dashboardViewModel = new DoctorDashboardViewModel
                {
                    Doctor = doctor,
                    
                    // Today's appointments
                    TodayAppointments = db.Appointments
                        .Include(a => a.Patient)
                        .Where(a => a.DoctorId == doctorId && DbFunctions.TruncateTime(a.AppointmentDate) == DbFunctions.TruncateTime(DateTime.Today))
                        .OrderBy(a => a.AppointmentDate)
                        .ToList(),
                    
                    // Upcoming appointments
                    UpcomingAppointments = db.Appointments
                        .Include(a => a.Patient)
                        .Where(a => a.DoctorId == doctorId && a.AppointmentDate > DateTime.Now)
                        .OrderBy(a => a.AppointmentDate)
                        .Take(10)
                        .ToList(),
                    
                    // Recent patients
                    RecentPatients = db.Appointments
                        .Include(a => a.Patient)
                        .Where(a => a.DoctorId == doctorId)
                        .OrderByDescending(a => a.AppointmentDate)
                        .Select(a => a.Patient)
                        .Distinct()
                        .Take(10)
                        .ToList(),
                    
                    // Recent prescriptions
                    RecentPrescriptions = db.Prescriptions
                        .Include(p => p.Patient)
                        .Include(p => p.Medication)
                        .Where(p => p.DoctorId == doctorId)
                        .OrderByDescending(p => p.DateIssued)
                        .Take(10)
                        .ToList(),
                    
                    // Recent medical records
                    RecentMedicalRecords = db.MedicalRecords
                        .Include(m => m.Patient)
                        .Where(m => m.DoctorId == doctorId)
                        .OrderByDescending(m => m.RecordDate)
                        .Take(10)
                        .ToList(),
                    
                    // Statistics
                    TotalAppointments = db.Appointments.Count(a => a.DoctorId == doctorId),
                    TotalPatients = db.Appointments.Where(a => a.DoctorId == doctorId).Select(a => a.PatientId).Distinct().Count(),
                    TotalPrescriptions = db.Prescriptions.Count(p => p.DoctorId == doctorId),
                    TotalMedicalRecords = db.MedicalRecords.Count(m => m.DoctorId == doctorId)
                };
                
                return View(dashboardViewModel);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in DoctorDashboardController.Index: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while loading the dashboard. Please try again later.";
                
                // Return an empty view model
                return View(new DoctorDashboardViewModel());
            }
        }
        
        // GET: DoctorDashboard/MyPatients
        public ActionResult MyPatients()
        {
            try
            {
                // Get the current user
                var userId = User.Identity.GetUserId();
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                
                if (user == null || !user.DoctorId.HasValue)
                {
                    TempData["ErrorMessage"] = "Doctor profile not found. Please contact the administrator.";
                    return RedirectToAction("Index");
                }
                
                var doctorId = user.DoctorId.Value;
                
                // Get all patients who have had appointments with this doctor
                var patients = db.Appointments
                    .Where(a => a.DoctorId == doctorId)
                    .Select(a => a.Patient)
                    .Distinct()
                    .ToList();
                
                return View(patients);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in DoctorDashboardController.MyPatients: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while loading patient data. Please try again later.";
                
                // Return an empty list
                return View(new List<Patient>());
            }
        }
        
        // GET: DoctorDashboard/MyAppointments
        public ActionResult MyAppointments()
        {
            try
            {
                // Get the current user
                var userId = User.Identity.GetUserId();
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                
                if (user == null || !user.DoctorId.HasValue)
                {
                    TempData["ErrorMessage"] = "Doctor profile not found. Please contact the administrator.";
                    return RedirectToAction("Index");
                }
                
                var doctorId = user.DoctorId.Value;
                
                // Get all appointments for this doctor
                var appointments = db.Appointments
                    .Include(a => a.Patient)
                    .Where(a => a.DoctorId == doctorId)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToList();
                
                return View(appointments);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in DoctorDashboardController.MyAppointments: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while loading appointment data. Please try again later.";
                
                // Return an empty list
                return View(new List<Appointment>());
            }
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
