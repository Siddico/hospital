﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_hospital_management_system.Models
{
    public class Prescription
    {
        [Key]
        [Display(Name = "Prescription ID")]
        public int PrescriptionId { get; set; }

        [Required(ErrorMessage = "Patient is required")]
        [Display(Name = "Patient")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Doctor is required")]
        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Medication is required")]
        [Display(Name = "Medication")]
        public int MedicationId { get; set; }

        [Required(ErrorMessage = "Date issued is required")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Issued")]
        public DateTime DateIssued { get; set; }

        [Required(ErrorMessage = "Instructions are required")]
        [StringLength(500, ErrorMessage = "Instructions cannot be longer than 500 characters")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Instructions")]
        public string Instructions { get; set; }

        // Navigation properties
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        [ForeignKey("MedicationId")]
        public virtual Medication Medication { get; set; }
    }
}
