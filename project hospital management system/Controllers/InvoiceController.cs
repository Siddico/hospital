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
    public class InvoiceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Invoice
        public ActionResult Index()
        {
            try
            {
                var invoices = db.Invoices
                    .Include(i => i.Patient)
                    .OrderByDescending(i => i.DateIssued)
                    .ToList();
                
                // Calculate summary statistics
                ViewBag.TotalPaid = invoices.Where(i => i.Status == "Paid").Sum(i => i.TotalAmount);
                ViewBag.TotalPending = invoices.Where(i => i.Status == "Pending").Sum(i => i.TotalAmount);
                ViewBag.TotalOverdue = invoices.Where(i => i.Status == "Overdue").Sum(i => i.TotalAmount);
                ViewBag.TotalAmount = invoices.Sum(i => i.TotalAmount);
                
                return View(invoices);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in InvoiceController.Index: " + ex.Message);
                
                // Add error message to TempData to display to the user
                TempData["ErrorMessage"] = "An error occurred while retrieving invoice data. Please try again later.";
                
                // Return an empty list to avoid null reference exceptions in the view
                return View(new List<Invoice>());
            }
        }

        // GET: Invoice/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                Invoice invoice = db.Invoices
                    .Include(i => i.Patient)
                    .FirstOrDefault(i => i.InvoiceId == id);
                
                if (invoice == null)
                {
                    return HttpNotFound();
                }
                
                return View(invoice);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in InvoiceController.Details: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving invoice details. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // GET: Invoice/Create
        public ActionResult Create()
        {
            try
            {
                // Populate dropdown list for patients
                ViewBag.PatientId = new SelectList(db.Patients.OrderBy(p => p.Name), "PatientId", "Name");
                
                // Set default date issued to current date
                var invoice = new Invoice
                {
                    DateIssued = DateTime.Now.Date,
                    Status = "Pending" // Default status
                };
                
                return View(invoice);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in InvoiceController.Create (GET): " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while preparing the invoice form. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // POST: Invoice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceId,PatientId,DateIssued,TotalAmount,Status")] Invoice invoice)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Invoices.Add(invoice);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Invoice created successfully!";
                    return RedirectToAction("Index");
                }
                
                ViewBag.PatientId = new SelectList(db.Patients.OrderBy(p => p.Name), "PatientId", "Name", invoice.PatientId);
                return View(invoice);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in InvoiceController.Create (POST): " + ex.Message);
                
                // Add the error to ModelState
                ModelState.AddModelError("", "An error occurred while creating the invoice. Please try again.");
                
                ViewBag.PatientId = new SelectList(db.Patients.OrderBy(p => p.Name), "PatientId", "Name", invoice.PatientId);
                return View(invoice);
            }
        }

        // GET: Invoice/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                Invoice invoice = db.Invoices.Find(id);
                
                if (invoice == null)
                {
                    return HttpNotFound();
                }
                
                ViewBag.PatientId = new SelectList(db.Patients.OrderBy(p => p.Name), "PatientId", "Name", invoice.PatientId);
                
                // Prepare status dropdown options
                ViewBag.StatusOptions = new SelectList(new[] { "Paid", "Pending", "Overdue" }, invoice.Status);
                
                return View(invoice);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in InvoiceController.Edit (GET): " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving invoice data for editing. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // POST: Invoice/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceId,PatientId,DateIssued,TotalAmount,Status")] Invoice invoice)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(invoice).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Invoice updated successfully!";
                    return RedirectToAction("Index");
                }
                
                ViewBag.PatientId = new SelectList(db.Patients.OrderBy(p => p.Name), "PatientId", "Name", invoice.PatientId);
                ViewBag.StatusOptions = new SelectList(new[] { "Paid", "Pending", "Overdue" }, invoice.Status);
                return View(invoice);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in InvoiceController.Edit (POST): " + ex.Message);
                
                // Add the error to ModelState
                ModelState.AddModelError("", "An error occurred while updating the invoice. Please try again.");
                
                ViewBag.PatientId = new SelectList(db.Patients.OrderBy(p => p.Name), "PatientId", "Name", invoice.PatientId);
                ViewBag.StatusOptions = new SelectList(new[] { "Paid", "Pending", "Overdue" }, invoice.Status);
                return View(invoice);
            }
        }

        // GET: Invoice/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
                Invoice invoice = db.Invoices
                    .Include(i => i.Patient)
                    .FirstOrDefault(i => i.InvoiceId == id);
                
                if (invoice == null)
                {
                    return HttpNotFound();
                }
                
                return View(invoice);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in InvoiceController.Delete (GET): " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving invoice data for deletion. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }

        // POST: Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Invoice invoice = db.Invoices.Find(id);
                db.Invoices.Remove(invoice);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Invoice deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in InvoiceController.DeleteConfirmed: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while deleting the invoice. Please try again later.";
                
                // Redirect to index
                return RedirectToAction("Index");
            }
        }
        
        // GET: Invoice/PatientInvoices/5
        public ActionResult PatientInvoices(int? id)
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
                
                var invoices = db.Invoices
                    .Where(i => i.PatientId == id)
                    .OrderByDescending(i => i.DateIssued)
                    .ToList();
                
                ViewBag.Patient = patient;
                ViewBag.TotalPaid = invoices.Where(i => i.Status == "Paid").Sum(i => i.TotalAmount);
                ViewBag.TotalPending = invoices.Where(i => i.Status == "Pending").Sum(i => i.TotalAmount);
                ViewBag.TotalOverdue = invoices.Where(i => i.Status == "Overdue").Sum(i => i.TotalAmount);
                ViewBag.TotalAmount = invoices.Sum(i => i.TotalAmount);
                
                return View(invoices);
            }
            catch (Exception ex)
            {
                // Log the exception
                System.Diagnostics.Debug.WriteLine("Error in InvoiceController.PatientInvoices: " + ex.Message);
                
                // Add error message to TempData
                TempData["ErrorMessage"] = "An error occurred while retrieving patient invoices. Please try again later.";
                
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
