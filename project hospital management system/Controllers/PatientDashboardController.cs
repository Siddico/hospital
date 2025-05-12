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
    [Authorize(Roles = "Patient")]
    public class PatientDashboardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: PatientDashboard
        public ActionResult Index()
        {
            try
            {
                // Get the current user
                var userId = User.Identity.GetUserId();
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                
                if (user == null || !user.PatientId.HasValue)
                {
                    TempData["ErrorMessage"] = "Patient profile not found. Please contact the administrator.";
                    return View(new PatientDashboardViewModel());
                }
                
                var patientId = user.PatientId.Value;
                var patient = db.Patients.Find(patientId);
                
                if (patient == null)
                {
                    TempData["ErrorMessage"] = "Patient profile not found. Please contact the administrator.";
                    return View(new PatientDashboardViewModel());
                }
                
                // Get patient's appointments, prescriptions, medical records, and invoices
                var dashboardViewModel = new PatientDashboardViewModel
                {
                    Patient = patient,
                    
                    // Upcoming appointments
                    UpcomingAppointments = db.Appointments
                        .Include(a => a.Doctor)
                        .Where(a => a.PatientId == patientId && a.AppointmentDate > DateTime.Now)
                        .OrderBy(a => a.AppointmentDate)
                        .Take(5)
                        .ToList(),
                    
                    // Recent appointments
                    RecentAppointments = db.Appointments
                        .Include(a => a.Doctor)
                        .Where(a => a.PatientId == patientId && a.AppointmentDate <= DateTime.Now)
                        .OrderByDescending(a => a.AppointmentDate)
                        .Take(5)
                        .ToList(),
                    
                    // Recent prescriptions
                    RecentPrescriptions = db.Prescriptions
                        .Include(p => p.Doctor)
                        .Include(p => p.Medication)
                        .Where(p => p.PatientId == patientId)
                        .OrderByDescending(p => p.DateIssued)
                        .Take(5)
                        .ToList(),
                    
                    // Recent medical records
                    RecentMedicalRecords = db.MedicalRecords
                        .Include(m => m.Doctor)
                        .Where(m => m.PatientId == patientId)
                        .OrderByDescending(m => m.RecordDate)
                        .Take(5)
                        .ToList(),
                    
                    // Recent invoices
                    RecentInvoices = db.Invoices
                        .Where(i => i.PatientId == patientId)
                        .OrderByDescending(i => i.DateIssued)
                        .Take(5)
                        .ToList(),
                    
                    // Statistics
                    TotalAppointments = db.Appointments.Count(a => a.PatientId == patientId),
                    TotalPrescriptions = db.Prescriptions.Count(p => p.PatientId == patientId),
                    TotalMedicalRecords = db.MedicalRecords.Count(m => m.PatientId == patientId),
                    TotalInvoices = db.Invoices.Count(i => i.PatientId == patientId),
                    TotalPendingInvoices = db.Invoices.Count(i => i.PatientId == patientId && i.Status == "Pending"),
                    TotalPaidInvoices = db.Invoices.Count(i => i.PatientId == patientId && i.Status == "Paid"),
                    TotalOverdueInvoices = db.Invoices.Count(i => i.PatientId == patientId && i.Status == "Overdue")
                };
                
                return View(dashboardViewModel);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PatientDashboardController.Index: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while loading the dashboard. Please try again later.";
                
                // Return an empty view model
                return View(new PatientDashboardViewModel());
            }
        }
        
        // GET: PatientDashboard/MyAppointments
        public ActionResult MyAppointments()
        {
            try
            {
                // Get the current user
                var userId = User.Identity.GetUserId();
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                
                if (user == null || !user.PatientId.HasValue)
                {
                    TempData["ErrorMessage"] = "Patient profile not found. Please contact the administrator.";
                    return RedirectToAction("Index");
                }
                
                var patientId = user.PatientId.Value;
                
                // Get all appointments for this patient
                var appointments = db.Appointments
                    .Include(a => a.Doctor)
                    .Where(a => a.PatientId == patientId)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToList();
                
                return View(appointments);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PatientDashboardController.MyAppointments: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while loading appointment data. Please try again later.";
                
                // Return an empty list
                return View(new List<Appointment>());
            }
        }
        
        // GET: PatientDashboard/MyPrescriptions
        public ActionResult MyPrescriptions()
        {
            try
            {
                // Get the current user
                var userId = User.Identity.GetUserId();
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                
                if (user == null || !user.PatientId.HasValue)
                {
                    TempData["ErrorMessage"] = "Patient profile not found. Please contact the administrator.";
                    return RedirectToAction("Index");
                }
                
                var patientId = user.PatientId.Value;
                
                // Get all prescriptions for this patient
                var prescriptions = db.Prescriptions
                    .Include(p => p.Doctor)
                    .Include(p => p.Medication)
                    .Where(p => p.PatientId == patientId)
                    .OrderByDescending(p => p.DateIssued)
                    .ToList();
                
                return View(prescriptions);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PatientDashboardController.MyPrescriptions: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while loading prescription data. Please try again later.";
                
                // Return an empty list
                return View(new List<Prescription>());
            }
        }
        
        // GET: PatientDashboard/MyInvoices
        public ActionResult MyInvoices()
        {
            try
            {
                // Get the current user
                var userId = User.Identity.GetUserId();
                var user = db.Users.FirstOrDefault(u => u.Id == userId);
                
                if (user == null || !user.PatientId.HasValue)
                {
                    TempData["ErrorMessage"] = "Patient profile not found. Please contact the administrator.";
                    return RedirectToAction("Index");
                }
                
                var patientId = user.PatientId.Value;
                
                // Get all invoices for this patient
                var invoices = db.Invoices
                    .Where(i => i.PatientId == patientId)
                    .OrderByDescending(i => i.DateIssued)
                    .ToList();
                
                return View(invoices);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PatientDashboardController.MyInvoices: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while loading invoice data. Please try again later.";
                
                // Return an empty list
                return View(new List<Invoice>());
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
