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
    public class MedicationController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Medication
        public ActionResult Index()
        {
            try
            {
                var medications = db.Medications.OrderBy(m => m.Name).ToList();
                return View(medications);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in MedicationController.Index: " + ex.Message);

                // Add error message to TempData to display to the user
                TempData["ErrorMessage"] = "An error occurred while retrieving medication data. Please try again later.";

                // Return an empty list to avoid null reference exceptions in the view
                return View(new List<Medication>());
            }
        }

        // GET: Medication/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Medication medication = db.Medications.Find(id);

                if (medication == null)
                {
                    return HttpNotFound();
                }

                // Get prescriptions that use this medication
                var prescriptions = db.Prescriptions
                    .Include(p => p.Patient)
                    .Include(p => p.Doctor)
                    .Where(p => p.MedicationId == id)
                    .OrderByDescending(p => p.DateIssued)
                    .ToList();

                ViewBag.Prescriptions = prescriptions;

                return View(medication);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in MedicationController.Details: " + ex.Message);

                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving medication details. Please try again later.";

                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // GET: Medication/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medication/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MedicationId,Name,Description,Dose")] Medication medication)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Medications.Add(medication);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Medication created successfully!";
                    return RedirectToAction("Index");
                }

                return View(medication);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in MedicationController.Create: " + ex.Message);

                // Add the error to ModelState
                ModelState.AddModelError("", "An error occurred while creating the medication. Please try again.");

                return View(medication);
            }
        }

        // GET: Medication/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Medication medication = db.Medications.Find(id);

                if (medication == null)
                {
                    return HttpNotFound();
                }

                return View(medication);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in MedicationController.Edit (GET): " + ex.Message);

                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving medication data for editing. Please try again later.";

                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // POST: Medication/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MedicationId,Name,Description,Dose")] Medication medication)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(medication).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Medication updated successfully!";
                    return RedirectToAction("Index");
                }

                return View(medication);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in MedicationController.Edit (POST): " + ex.Message);

                // Add the error to ModelState
                ModelState.AddModelError("", "An error occurred while updating the medication. Please try again.");

                return View(medication);
            }
        }

        // GET: Medication/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Medication medication = db.Medications.Find(id);

                if (medication == null)
                {
                    return HttpNotFound();
                }

                return View(medication);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in MedicationController.Delete (GET): " + ex.Message);

                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving medication data for deletion. Please try again later.";

                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // POST: Medication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Medication medication = db.Medications.Find(id);
                db.Medications.Remove(medication);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Medication deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in MedicationController.DeleteConfirmed: " + ex.Message);

                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while deleting the medication. Please try again later.";

                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // GET: Medication/Search
        public ActionResult Search(string searchString)
        {
            try
            {
                var medications = from m in db.Medications
                                 select m;

                if (!String.IsNullOrEmpty(searchString))
                {
                    medications = medications.Where(m => m.Name.Contains(searchString)
                                                     || m.Description.Contains(searchString)
                                                     || m.Dose.Contains(searchString));
                }

                return View("Index", medications.OrderBy(m => m.Name).ToList());
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in MedicationController.Search: " + ex.Message);

                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while searching for medications. Please try again later.";

                // Return an empty list to avoid null reference exceptions in the view
                return View("Index", new List<Medication>());
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
