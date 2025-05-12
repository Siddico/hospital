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
    public class PatientController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Patient
        public ActionResult Index()
        {
            try
            {
                return View(db.Patients.ToList());
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PatientController.Index: " + ex.Message);
                
                // Add error message to TempData to display to the user
                TempData["ErrorMessage"] = "An error occurred while retrieving patient data. Please try again later.";
                
                // Return an empty list to avoid null reference exceptions in the view
                return View(new List<Patient>());
            }
        }

        // GET: Patient/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                Patient patient = db.Patients.Find(id);
                
                if (patient == null)
                {
                    return HttpNotFound();
                }
                
                return View(patient);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PatientController.Details: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving patient details. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // GET: Patient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientId,Name,Gender,DateOfBirth,Phone,Address")] Patient patient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Patients.Add(patient);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Patient created successfully!";
                    return RedirectToAction("Index");
                }
                
                return View(patient);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PatientController.Create: " + ex.Message);
                
                // Add the error to ModelState
                ModelState.AddModelError("", "An error occurred while creating the patient. Please try again.");
                
                return View(patient);
            }
        }

        // GET: Patient/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                Patient patient = db.Patients.Find(id);
                
                if (patient == null)
                {
                    return HttpNotFound();
                }
                
                return View(patient);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PatientController.Edit: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving patient data for editing. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // POST: Patient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatientId,Name,Gender,DateOfBirth,Phone,Address")] Patient patient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(patient).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Patient updated successfully!";
                    return RedirectToAction("Index");
                }
                
                return View(patient);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency conflicts
                System.Diagnostics.Debug.WriteLine("Concurrency error in PatientController.Edit: " + ex.Message);
                
                var entry = ex.Entries.Single();
                var clientValues = (Patient)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                
                if (databaseEntry == null)
                {
                    ModelState.AddModelError("", "Unable to save changes. The patient was deleted by another user.");
                }
                else
                {
                    ModelState.AddModelError("", "The record you attempted to edit was modified by another user after you. Please refresh and try again.");
                }
                
                return View(patient);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PatientController.Edit: " + ex.Message);
                
                // Add the error to ModelState
                ModelState.AddModelError("", "An error occurred while updating the patient. Please try again.");
                
                return View(patient);
            }
        }

        // GET: Patient/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                if (saveChangesError.GetValueOrDefault())
                {
                    ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
                }
                
                Patient patient = db.Patients.Find(id);
                
                if (patient == null)
                {
                    return HttpNotFound();
                }
                
                return View(patient);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PatientController.Delete: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving patient data for deletion. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Patient patient = db.Patients.Find(id);
                db.Patients.Remove(patient);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Patient deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PatientController.DeleteConfirmed: " + ex.Message);
                
                // Redirect to Delete GET action with error flag
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
        }

        // GET: Patient/Search
        public ActionResult Search(string searchString)
        {
            try
            {
                var patients = from p in db.Patients
                               select p;

                if (!String.IsNullOrEmpty(searchString))
                {
                    patients = patients.Where(p => p.Name.Contains(searchString) 
                                                || p.Phone.Contains(searchString)
                                                || p.Address.Contains(searchString));
                }

                return View("Index", patients.ToList());
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in PatientController.Search: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while searching for patients. Please try again later.";
                
                // Return an empty list to avoid null reference exceptions in the view
                return View("Index", new List<Patient>());
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
