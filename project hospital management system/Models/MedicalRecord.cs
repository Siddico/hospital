﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_hospital_management_system.Models
{
    public class MedicalRecord
    {
        [Key]
        [Display(Name = "Record ID")]
        public int RecordId { get; set; }

        [Required(ErrorMessage = "Patient is required")]
        [Display(Name = "Patient")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Doctor is required")]
        [Display(Name = "Doctor")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Diagnosis is required")]
        [StringLength(500, ErrorMessage = "Diagnosis cannot be longer than 500 characters")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Diagnosis")]
        public string Diagnosis { get; set; }

        [Required(ErrorMessage = "Treatment details are required")]
        [StringLength(1000, ErrorMessage = "Treatment details cannot be longer than 1000 characters")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Treatment")]
        public string Treatment { get; set; }

        [Required(ErrorMessage = "Record date is required")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Record Date")]
        public DateTime RecordDate { get; set; }

        // Navigation properties
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }
    }
}
