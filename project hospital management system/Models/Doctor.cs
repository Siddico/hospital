﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_hospital_management_system.Models
{
    public class Doctor
    {
        [Key]
        [Display(Name = "Doctor ID")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Doctor name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Specialization is required")]
        [StringLength(50, ErrorMessage = "Specialization cannot be longer than 50 characters")]
        [Display(Name = "Specialization")]
        public string Specialization { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [StringLength(20, ErrorMessage = "Phone number cannot be longer than 20 characters")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Contact Number")]
        public string Phone { get; set; }

        // Navigation properties
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }

        public Doctor()
        {
            Appointments = new HashSet<Appointment>();
            MedicalRecords = new HashSet<MedicalRecord>();
            Prescriptions = new HashSet<Prescription>();
        }
    }
}
