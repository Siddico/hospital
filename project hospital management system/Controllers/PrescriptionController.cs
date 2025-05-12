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
    public class PrescriptionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Prescription
        public ActionResult Index()
        {
            try
            {
                var prescriptions = db.Prescriptions
                    .Include(p => p.Patient)
                    .Include(p => p.Doctor)
                    .Include(p => p.Medication)
                    .OrderByDescending(p => p.DateIssued)
                    .ToList();
                
                return View(prescriptions);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PrescriptionController.Index: " + ex.Message);
                
                // Add error message to TempData to display to the user
                TempData["ErrorMessage"] = "An error occurred while retrieving prescription data. Please try again later.";
                
                // Return an empty list to avoid null reference exceptions in the view
                return View(new List<Prescription>());
            }
        }

        // GET: Prescription/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                Prescription prescription = db.Prescriptions
                    .Include(p => p.Patient)
                    .Include(p => p.Doctor)
                    .Include(p => p.Medication)
                    .FirstOrDefault(p => p.PrescriptionId == id);
                
                if (prescription == null)
                {
                    return HttpNotFound();
                }
                
                return View(prescription);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PrescriptionController.Details: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving prescription details. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // GET: Prescription/Create
        public ActionResult Create()
        {
            try
            {
                // Populate dropdown lists for patients, doctors, and medications
                ViewBag.PatientId = new SelectList(db.Patients.OrderBy(p => p.Name), "PatientId", "Name");
                ViewBag.DoctorId = new SelectList(db.Doctors.OrderBy(d => d.Name), "DoctorId", "Name");
                ViewBag.MedicationId = new SelectList(db.Medications.OrderBy(m => m.Name), "MedicationId", "Name");
                
                // Set default date issued to current date and time
                var prescription = new Prescription
                {
                    DateIssued = DateTime.Now
                };
                
                return View(prescription);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PrescriptionController.Create (GET): " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while preparing the prescription form. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // POST: Prescription/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PrescriptionId,PatientId,DoctorId,MedicationId,DateIssued,Instructions")] Prescription prescription)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Prescriptions.Add(prescription);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Prescription created successfully!";
                    return RedirectToAction("Index");
                }
                
                ViewBag.PatientId = new SelectList(db.Patients.OrderBy(p => p.Name), "PatientId", "Name", prescription.PatientId);
                ViewBag.DoctorId = new SelectList(db.Doctors.OrderBy(d => d.Name), "DoctorId", "Name", prescription.DoctorId);
                ViewBag.MedicationId = new SelectList(db.Medications.OrderBy(m => m.Name), "MedicationId", "Name", prescription.MedicationId);
                return View(prescription);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PrescriptionController.Create (POST): " + ex.Message);
                
                // Add the error to ModelState
                ModelState.AddModelError("", "An error occurred while creating the prescription. Please try again.");
                
                ViewBag.PatientId = new SelectList(db.Patients.OrderBy(p => p.Name), "PatientId", "Name", prescription.PatientId);
                ViewBag.DoctorId = new SelectList(db.Doctors.OrderBy(d => d.Name), "DoctorId", "Name", prescription.DoctorId);
                ViewBag.MedicationId = new SelectList(db.Medications.OrderBy(m => m.Name), "MedicationId", "Name", prescription.MedicationId);
                return View(prescription);
            }
        }

        // GET: Prescription/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                Prescription prescription = db.Prescriptions.Find(id);
                
                if (prescription == null)
                {
                    return HttpNotFound();
                }
                
                ViewBag.PatientId = new SelectList(db.Patients.OrderBy(p => p.Name), "PatientId", "Name", prescription.PatientId);
                ViewBag.DoctorId = new SelectList(db.Doctors.OrderBy(d => d.Name), "DoctorId", "Name", prescription.DoctorId);
                ViewBag.MedicationId = new SelectList(db.Medications.OrderBy(m => m.Name), "MedicationId", "Name", prescription.MedicationId);
                return View(prescription);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PrescriptionController.Edit (GET): " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving prescription data for editing. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // POST: Prescription/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PrescriptionId,PatientId,DoctorId,MedicationId,DateIssued,Instructions")] Prescription prescription)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(prescription).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Prescription updated successfully!";
                    return RedirectToAction("Index");
                }
                
                ViewBag.PatientId = new SelectList(db.Patients.OrderBy(p => p.Name), "PatientId", "Name", prescription.PatientId);
                ViewBag.DoctorId = new SelectList(db.Doctors.OrderBy(d => d.Name), "DoctorId", "Name", prescription.DoctorId);
                ViewBag.MedicationId = new SelectList(db.Medications.OrderBy(m => m.Name), "MedicationId", "Name", prescription.MedicationId);
                return View(prescription);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PrescriptionController.Edit (POST): " + ex.Message);
                
                // Add the error to ModelState
                ModelState.AddModelError("", "An error occurred while updating the prescription. Please try again.");
                
                ViewBag.PatientId = new SelectList(db.Patients.OrderBy(p => p.Name), "PatientId", "Name", prescription.PatientId);
                ViewBag.DoctorId = new SelectList(db.Doctors.OrderBy(d => d.Name), "DoctorId", "Name", prescription.DoctorId);
                ViewBag.MedicationId = new SelectList(db.Medications.OrderBy(m => m.Name), "MedicationId", "Name", prescription.MedicationId);
                return View(prescription);
            }
        }

        // GET: Prescription/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                Prescription prescription = db.Prescriptions
                    .Include(p => p.Patient)
                    .Include(p => p.Doctor)
                    .Include(p => p.Medication)
                    .FirstOrDefault(p => p.PrescriptionId == id);
                
                if (prescription == null)
                {
                    return HttpNotFound();
                }
                
                return View(prescription);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PrescriptionController.Delete (GET): " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving prescription data for deletion. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // POST: Prescription/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Prescription prescription = db.Prescriptions.Find(id);
                db.Prescriptions.Remove(prescription);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Prescription deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PrescriptionController.DeleteConfirmed: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while deleting the prescription. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }
        
        // GET: Prescription/PatientPrescriptions/5
        public ActionResult PatientPrescriptions(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                var patient = db.Patients.Find(id);
                if (patient == null)
                {
                    return HttpNotFound();
                }
                
                var prescriptions = db.Prescriptions
                    .Include(p => p.Doctor)
                    .Include(p => p.Medication)
                    .Where(p => p.PatientId == id)
                    .OrderByDescending(p => p.DateIssued)
                    .ToList();
                
                ViewBag.Patient = patient;
                return View(prescriptions);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PrescriptionController.PatientPrescriptions: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving patient prescriptions. Please try again later.";
                
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
