﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_hospital_management_system.Models
{
    public class Patient
    {
        [Key]
        [Display(Name = "Patient ID")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Patient name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(10, ErrorMessage = "Gender cannot be longer than 10 characters")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(20, ErrorMessage = "Phone number cannot be longer than 20 characters")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        // Calculated property for age (not stored in database)
        [NotMapped]
        [Display(Name = "Age")]
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        // Navigation properties
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }

        public Patient()
        {
            Appointments = new HashSet<Appointment>();
            MedicalRecords = new HashSet<MedicalRecord>();
            Prescriptions = new HashSet<Prescription>();
            Invoices = new HashSet<Invoice>();
        }
    }
}
