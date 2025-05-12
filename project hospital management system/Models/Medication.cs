﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_hospital_management_system.Models
{
    public class Medication
    {
        [Key]
        [Display(Name = "Medication ID")]
        public int MedicationId { get; set; }

        [Required(ErrorMessage = "Medication name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        [Display(Name = "Medication Name")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Dose information is required")]
        [StringLength(100, ErrorMessage = "Dose cannot be longer than 100 characters")]
        [Display(Name = "Dose")]
        public string Dose { get; set; }
    }
}
