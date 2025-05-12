﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_hospital_management_system.Models
{
    public class Invoice
    {
        [Key]
        [Display(Name = "Invoice ID")]
        public int InvoiceId { get; set; }

        [Required(ErrorMessage = "Patient is required")]
        [Display(Name = "Patient")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Date issued is required")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Issued")]
        public DateTime DateIssued { get; set; }

        [Required(ErrorMessage = "Total amount is required")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be a positive number")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [StringLength(20, ErrorMessage = "Status cannot be longer than 20 characters")]
        [Display(Name = "Status")]
        public string Status { get; set; }

        // Navigation property
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
        
        // Helper method to get status color for UI
        [NotMapped]
        public string StatusColor
        {
            get
            {
                switch (Status.ToLower())
                {
                    case "paid":
                        return "success";
                    case "pending":
                        return "warning";
                    case "overdue":
                        return "danger";
                    default:
                        return "default";
                }
            }
        }
    }
}
