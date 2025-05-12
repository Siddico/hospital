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
    public class MedicalRecordController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MedicalRecord
        public ActionResult Index()
        {
            try
            {
                var medicalRecords = db.MedicalRecords
                    .Include(m => m.Patient)
                    .Include(m => m.Doctor)
                    .OrderByDescending(m => m.RecordDate)
                    .ToList();

                return View(medicalRecords);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in MedicalRecordController.Index: " + ex.Message);

                // Add error message to TempData to display to the user
                TempData["ErrorMessage"] = "An error occurred while retrieving medical record data. Please try again later.";

                // Return an empty list to avoid null reference exceptions in the view
                return View(new List<MedicalRecord>());
            }
        }

        // GET: MedicalRecord/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                MedicalRecord medicalRecord = db.MedicalRecords
                    .Include(m => m.Patient)
                    .Include(m => m.Doctor)
                    .FirstOrDefault(m => m.RecordId == id);

                if (medicalRecord == null)
                {
                    return HttpNotFound();
                }

                // Get medications to display in the view
                ViewBag.Medications = db.Medications.OrderBy(m => m.Name).ToList();

                return View(medicalRecord);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in MedicalRecordController.Details: " + ex.Message);

                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving medical record details. Please try again later.";

                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // GET: MedicalRecord/Create
        public ActionResult Create()
        {
            try
            {
                // Populate dropdown lists for patients and doctors
                ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name");
                ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "Name");

                // Set default record date to current date and time
                var medicalRecord = new MedicalRecord
                {
                    RecordDate = DateTime.Now
                };

                return View(medicalRecord);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in MedicalRecordController.Create (GET): " + ex.Message);

                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while preparing the medical record form. Please try again later.";

                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // POST: MedicalRecord/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RecordId,PatientId,DoctorId,Diagnosis,Treatment,RecordDate")] MedicalRecord medicalRecord)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.MedicalRecords.Add(medicalRecord);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Medical record created successfully!";
                    return RedirectToAction("Index");
                }

                ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name", medicalRecord.PatientId);
                ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "Name", medicalRecord.DoctorId);
                return View(medicalRecord);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in MedicalRecordController.Create (POST): " + ex.Message);

                // Add the error to ModelState
                ModelState.AddModelError("", "An error occurred while creating the medical record. Please try again.");

                ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name", medicalRecord.PatientId);
                ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "Name", medicalRecord.DoctorId);
                return View(medicalRecord);
            }
        }

        // GET: MedicalRecord/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                MedicalRecord medicalRecord = db.MedicalRecords.Find(id);

                if (medicalRecord == null)
                {
                    return HttpNotFound();
                }

                ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name", medicalRecord.PatientId);
                ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "Name", medicalRecord.DoctorId);
                return View(medicalRecord);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in MedicalRecordController.Edit (GET): " + ex.Message);

                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving medical record data for editing. Please try again later.";

                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // POST: MedicalRecord/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RecordId,PatientId,DoctorId,Diagnosis,Treatment,RecordDate")] MedicalRecord medicalRecord)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(medicalRecord).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Medical record updated successfully!";
                    return RedirectToAction("Index");
                }

                ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name", medicalRecord.PatientId);
                ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "Name", medicalRecord.DoctorId);
                return View(medicalRecord);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in MedicalRecordController.Edit (POST): " + ex.Message);

                // Add the error to ModelState
                ModelState.AddModelError("", "An error occurred while updating the medical record. Please try again.");

                ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name", medicalRecord.PatientId);
                ViewBag.DoctorId = new SelectList(db.Doctors, "DoctorId", "Name", medicalRecord.DoctorId);
                return View(medicalRecord);
            }
        }

        // GET: MedicalRecord/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                MedicalRecord medicalRecord = db.MedicalRecords
                    .Include(m => m.Patient)
                    .Include(m => m.Doctor)
                    .FirstOrDefault(m => m.RecordId == id);

                if (medicalRecord == null)
                {
                    return HttpNotFound();
                }

                return View(medicalRecord);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in MedicalRecordController.Delete (GET): " + ex.Message);

                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving medical record data for deletion. Please try again later.";

                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // POST: MedicalRecord/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                MedicalRecord medicalRecord = db.MedicalRecords.Find(id);
                db.MedicalRecords.Remove(medicalRecord);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Medical record deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in MedicalRecordController.DeleteConfirmed: " + ex.Message);

                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while deleting the medical record. Please try again later.";

                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // GET: MedicalRecord/PatientRecords/5
        public ActionResult PatientRecords(int? id)
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

                var medicalRecords = db.MedicalRecords
                    .Include(m => m.Doctor)
                    .Where(m => m.PatientId == id)
                    .OrderByDescending(m => m.RecordDate)
                    .ToList();

                ViewBag.Patient = patient;
                return View(medicalRecords);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in MedicalRecordController.PatientRecords: " + ex.Message);

                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving patient medical records. Please try again later.";

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
