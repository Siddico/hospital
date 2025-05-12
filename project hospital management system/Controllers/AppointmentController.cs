﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using project_hospital_management_system.Models;

namespace project_hospital_management_system.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Appointment
        public ActionResult Index()
        {
            try
            {
                var appointments = db.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .OrderBy(a => a.AppointmentDate)
                    .ToList();
                
                return View(appointments);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in AppointmentController.Index: " + ex.Message);
                
                // Add error message to TempData to display to the user
                TempData["ErrorMessage"] = "An error occurred while retrieving appointment data. Please try again later.";
                
                // Return an empty list to avoid null reference exceptions in the view
                return View(new List<Appointment>());
            }
        }

        // GET: Appointment/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                Appointment appointment = db.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .FirstOrDefault(a => a.AppointmentId == id);
                
                if (appointment == null)
                {
                    return HttpNotFound();
                }
                
                return View(appointment);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in AppointmentController.Details: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving appointment details. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // GET: Appointment/Create
        public ActionResult Create()
        {
            try
            {
                // Populate dropdown lists for patients and doctors
                ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name");
                ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "Name");
                
                // Set default appointment date to tomorrow at 9 AM
                var appointment = new Appointment
                {
                    AppointmentDate = DateTime.Now.Date.AddDays(1).AddHours(9)
                };
                
                return View(appointment);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in AppointmentController.Create (GET): " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while preparing the appointment form. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // POST: Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AppointmentId,PatientId,DoctorId,AppointmentDate,Notes")] Appointment appointment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check for appointment conflicts
                    bool hasConflict = db.Appointments
                        .Any(a => a.DoctorId == appointment.DoctorId && 
                             a.AppointmentDate == appointment.AppointmentDate);
                    
                    if (hasConflict)
                    {
                        ModelState.AddModelError("AppointmentDate", "The doctor already has an appointment at this time. Please select a different time.");
                        ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name", appointment.PatientId);
                        ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "Name", appointment.DoctorId);
                        return View(appointment);
                    }
                    
                    db.Appointments.Add(appointment);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Appointment created successfully!";
                    return RedirectToAction("Index");
                }
                
                ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name", appointment.PatientId);
                ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "Name", appointment.DoctorId);
                return View(appointment);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in AppointmentController.Create (POST): " + ex.Message);
                
                // Add the error to ModelState
                ModelState.AddModelError("", "An error occurred while creating the appointment. Please try again.");
                
                ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name", appointment.PatientId);
                ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "Name", appointment.DoctorId);
                return View(appointment);
            }
        }

        // GET: Appointment/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                Appointment appointment = db.Appointments.Find(id);
                
                if (appointment == null)
                {
                    return HttpNotFound();
                }
                
                ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name", appointment.PatientId);
                ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "Name", appointment.DoctorId);
                return View(appointment);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in AppointmentController.Edit (GET): " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving appointment data for editing. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // POST: Appointment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AppointmentId,PatientId,DoctorId,AppointmentDate,Notes")] Appointment appointment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check for appointment conflicts (excluding this appointment)
                    bool hasConflict = db.Appointments
                        .Any(a => a.DoctorId == appointment.DoctorId && 
                             a.AppointmentDate == appointment.AppointmentDate &&
                             a.AppointmentId != appointment.AppointmentId);
                    
                    if (hasConflict)
                    {
                        ModelState.AddModelError("AppointmentDate", "The doctor already has an appointment at this time. Please select a different time.");
                        ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name", appointment.PatientId);
                        ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "Name", appointment.DoctorId);
                        return View(appointment);
                    }
                    
                    db.Entry(appointment).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Appointment updated successfully!";
                    return RedirectToAction("Index");
                }
                
                ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name", appointment.PatientId);
                ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "Name", appointment.DoctorId);
                return View(appointment);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in AppointmentController.Edit (POST): " + ex.Message);
                
                // Add the error to ModelState
                ModelState.AddModelError("", "An error occurred while updating the appointment. Please try again.");
                
                ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name", appointment.PatientId);
                ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "Name", appointment.DoctorId);
                return View(appointment);
            }
        }

        // GET: Appointment/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                Appointment appointment = db.Appointments
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .FirstOrDefault(a => a.AppointmentId == id);
                
                if (appointment == null)
                {
                    return HttpNotFound();
                }
                
                return View(appointment);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in AppointmentController.Delete (GET): " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving appointment data for deletion. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // POST: Appointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Appointment appointment = db.Appointments.Find(id);
                db.Appointments.Remove(appointment);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Appointment deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in AppointmentController.DeleteConfirmed: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while deleting the appointment. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
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
