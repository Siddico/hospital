﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace project_hospital_management_system.Models
{
    public static class ApplicationRoles
    {
        public const string Admin = "Admin";
        public const string Doctor = "Doctor";
        public const string Nurse = "Nurse";
        public const string Receptionist = "Receptionist";
        public const string Accountant = "Accountant";
        public const string Patient = "Patient";
        
        public static List<string> GetAllRoles()
        {
            return new List<string>
            {
                Admin,
                Doctor,
                Nurse,
                Receptionist,
                Accountant,
                Patient
            };
        }
        
        // Define permissions for each role
        public static Dictionary<string, List<string>> RolePermissions = new Dictionary<string, List<string>>
        {
            {
                Admin, new List<string>
                {
                    "ManageUsers",
                    "ManageRoles",
                    "ViewAllPatients",
                    "EditAllPatients",
                    "ViewAllDoctors",
                    "EditAllDoctors",
                    "ViewAllAppointments",
                    "EditAllAppointments",
                    "ViewAllMedicalRecords",
                    "EditAllMedicalRecords",
                    "ViewAllMedications",
                    "EditAllMedications",
                    "ViewAllPrescriptions",
                    "EditAllPrescriptions",
                    "ViewAllInvoices",
                    "EditAllInvoices",
                    "ViewDashboard",
                    "ViewReports"
                }
            },
            {
                Doctor, new List<string>
                {
                    "ViewAssignedPatients",
                    "ViewAllPatients",
                    "ViewAssignedAppointments",
                    "EditAssignedAppointments",
                    "ViewAssignedMedicalRecords",
                    "EditAssignedMedicalRecords",
                    "ViewAllMedications",
                    "CreatePrescriptions",
                    "ViewCreatedPrescriptions",
                    "EditCreatedPrescriptions",
                    "ViewDoctorDashboard"
                }
            },
            {
                Nurse, new List<string>
                {
                    "ViewAllPatients",
                    "ViewAssignedAppointments",
                    "ViewAllMedicalRecords",
                    "EditAssignedMedicalRecords",
                    "ViewAllMedications",
                    "ViewAllPrescriptions",
                    "ViewNurseDashboard"
                }
            },
            {
                Receptionist, new List<string>
                {
                    "ViewAllPatients",
                    "CreatePatients",
                    "EditPatients",
                    "ViewAllAppointments",
                    "CreateAppointments",
                    "EditAppointments",
                    "ViewReceptionistDashboard"
                }
            },
            {
                Accountant, new List<string>
                {
                    "ViewAllPatients",
                    "ViewAllInvoices",
                    "CreateInvoices",
                    "EditInvoices",
                    "ViewAccountantDashboard",
                    "ViewFinancialReports"
                }
            },
            {
                Patient, new List<string>
                {
                    "ViewOwnProfile",
                    "EditOwnProfile",
                    "ViewOwnAppointments",
                    "ViewOwnMedicalRecords",
                    "ViewOwnPrescriptions",
                    "ViewOwnInvoices",
                    "ViewPatientDashboard"
                }
            }
        };
    }
    
    public class RoleViewModel
    {
        public string Id { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
        
        public List<string> Permissions { get; set; }
        
        public RoleViewModel()
        {
            Permissions = new List<string>();
        }
    }
    
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        
        public UserRoleViewModel()
        {
            Roles = new List<string>();
        }
    }
}
