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
    public class DoctorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Doctor
        public ActionResult Index()
        {
            try
            {
                return View(db.Doctors.ToList());
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in DoctorController.Index: " + ex.Message);
                
                // Add error message to TempData to display to the user
                TempData["ErrorMessage"] = "An error occurred while retrieving doctor data. Please try again later.";
                
                // Return an empty list to avoid null reference exceptions in the view
                return View(new List<Doctor>());
            }
        }

        // GET: Doctor/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                Doctor doctor = db.Doctors.Find(id);
                
                if (doctor == null)
                {
                    return HttpNotFound();
                }
                
                return View(doctor);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in DoctorController.Details: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving doctor details. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // GET: Doctor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Doctor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DoctorId,Name,Specialization,Phone")] Doctor doctor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Doctors.Add(doctor);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Doctor created successfully!";
                    return RedirectToAction("Index");
                }
                
                return View(doctor);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in DoctorController.Create: " + ex.Message);
                
                // Add the error to ModelState
                ModelState.AddModelError("", "An error occurred while creating the doctor. Please try again.");
                
                return View(doctor);
            }
        }

        // GET: Doctor/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                Doctor doctor = db.Doctors.Find(id);
                
                if (doctor == null)
                {
                    return HttpNotFound();
                }
                
                return View(doctor);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in DoctorController.Edit: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving doctor data for editing. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // POST: Doctor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DoctorId,Name,Specialization,Phone")] Doctor doctor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(doctor).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Doctor updated successfully!";
                    return RedirectToAction("Index");
                }
                
                return View(doctor);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in DoctorController.Edit: " + ex.Message);
                
                // Add the error to ModelState
                ModelState.AddModelError("", "An error occurred while updating the doctor. Please try again.");
                
                return View(doctor);
            }
        }

        // GET: Doctor/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                Doctor doctor = db.Doctors.Find(id);
                
                if (doctor == null)
                {
                    return HttpNotFound();
                }
                
                return View(doctor);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in DoctorController.Delete: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving doctor data for deletion. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // POST: Doctor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Doctor doctor = db.Doctors.Find(id);
                db.Doctors.Remove(doctor);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Doctor deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in DoctorController.DeleteConfirmed: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while deleting the doctor. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // GET: Doctor/Search
        public ActionResult Search(string searchString)
        {
            try
            {
                var doctors = from d in db.Doctors
                              select d;

                if (!String.IsNullOrEmpty(searchString))
                {
                    doctors = doctors.Where(d => d.Name.Contains(searchString) 
                                              || d.Specialization.Contains(searchString)
                                              || d.Phone.Contains(searchString));
                }

                return View("Index", doctors.ToList());
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in DoctorController.Search: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while searching for doctors. Please try again later.";
                
                // Return an empty list to avoid null reference exceptions in the view
                return View("Index", new List<Doctor>());
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
