﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using project_hospital_management_system.Models;

namespace project_hospital_management_system.ViewModels
{
    public class AdminDashboardViewModel
    {
        // System statistics
        public int TotalPatients { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalAppointments { get; set; }
        public int TotalMedicalRecords { get; set; }
        public int TotalMedications { get; set; }
        public int TotalPrescriptions { get; set; }
        public int TotalInvoices { get; set; }
        public int TotalUsers { get; set; }
        
        // Financial statistics
        public decimal TotalRevenue { get; set; }
        public decimal PendingPayments { get; set; }
        public decimal OverduePayments { get; set; }
        
        // User statistics
        public Dictionary<string, int> UsersByRole { get; set; }
        
        // Recent activity
        public List<Patient> RecentPatients { get; set; }
        public List<Appointment> RecentAppointments { get; set; }
        public List<Invoice> RecentInvoices { get; set; }
        
        public AdminDashboardViewModel()
        {
            RecentPatients = new List<Patient>();
            RecentAppointments = new List<Appointment>();
            RecentInvoices = new List<Invoice>();
            UsersByRole = new Dictionary<string, int>();
        }
    }
    
    public class DoctorDashboardViewModel
    {
        // Doctor information
        public Doctor Doctor { get; set; }
        
        // Appointments
        public List<Appointment> TodayAppointments { get; set; }
        public List<Appointment> UpcomingAppointments { get; set; }
        
        // Patients
        public List<Patient> RecentPatients { get; set; }
        
        // Prescriptions
        public List<Prescription> RecentPrescriptions { get; set; }
        
        // Medical Records
        public List<MedicalRecord> RecentMedicalRecords { get; set; }
        
        // Statistics
        public int TotalAppointments { get; set; }
        public int TotalPatients { get; set; }
        public int TotalPrescriptions { get; set; }
        public int TotalMedicalRecords { get; set; }
        
        public DoctorDashboardViewModel()
        {
            TodayAppointments = new List<Appointment>();
            UpcomingAppointments = new List<Appointment>();
            RecentPatients = new List<Patient>();
            RecentPrescriptions = new List<Prescription>();
            RecentMedicalRecords = new List<MedicalRecord>();
        }
    }
    
    public class PatientDashboardViewModel
    {
        // Patient information
        public Patient Patient { get; set; }
        
        // Appointments
        public List<Appointment> UpcomingAppointments { get; set; }
        public List<Appointment> RecentAppointments { get; set; }
        
        // Prescriptions
        public List<Prescription> RecentPrescriptions { get; set; }
        
        // Medical Records
        public List<MedicalRecord> RecentMedicalRecords { get; set; }
        
        // Invoices
        public List<Invoice> RecentInvoices { get; set; }
        
        // Statistics
        public int TotalAppointments { get; set; }
        public int TotalPrescriptions { get; set; }
        public int TotalMedicalRecords { get; set; }
        public int TotalInvoices { get; set; }
        public int TotalPendingInvoices { get; set; }
        public int TotalPaidInvoices { get; set; }
        public int TotalOverdueInvoices { get; set; }
        
        public PatientDashboardViewModel()
        {
            UpcomingAppointments = new List<Appointment>();
            RecentAppointments = new List<Appointment>();
            RecentPrescriptions = new List<Prescription>();
            RecentMedicalRecords = new List<MedicalRecord>();
            RecentInvoices = new List<Invoice>();
        }
    }
    
    public class UserViewModel
    {
        public string UserId { get; set; }
        
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        [Display(Name = "Profile Picture")]
        public string ProfilePicture { get; set; }
        
        [Display(Name = "Date Created")]
        public DateTime? DateCreated { get; set; }
        
        [Display(Name = "Last Login")]
        public DateTime? LastLoginDate { get; set; }
        
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        
        [Display(Name = "Roles")]
        public List<string> Roles { get; set; }
        
        public UserViewModel()
        {
            Roles = new List<string>();
        }
    }
}
