﻿using System;
using System.ComponentModel.DataAnnotations;

namespace project_hospital_management_system.Models
{
    public class DashboardStatistic
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string StatisticName { get; set; }
        
        [Required]
        public string StatisticValue { get; set; }
        
        public string Category { get; set; }
        
        public string Icon { get; set; }
        
        public string ColorClass { get; set; }
        
        public DateTime LastUpdated { get; set; }
        
        public DashboardStatistic()
        {
            LastUpdated = DateTime.Now;
        }
    }
}
