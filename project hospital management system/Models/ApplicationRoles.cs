using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        // Define permissions for each role
        public static readonly Dictionary<string, List<string>> RolePermissions = new Dictionary<string, List<string>>
        {
            {
                Admin, new List<string>
                {
                    "ManageUsers",
                    "ManageRoles",
                    "ManagePatients",
                    "ManageDoctors",
                    "ManageAppointments",
                    "ManageMedicalRecords",
                    "ManageMedications",
                    "ManagePrescriptions",
                    "ManageInvoices",
                    "ViewSystemLogs",
                    "ManageSystemSettings"
                }
            },
            {
                Doctor, new List<string>
                {
                    "ViewPatients",
                    "ViewAppointments",
                    "ManageAppointments",
                    "ManageMedicalRecords",
                    "ViewMedications",
                    "ManagePrescriptions"
                }
            },
            {
                Nurse, new List<string>
                {
                    "ViewPatients",
                    "ViewAppointments",
                    "ViewMedicalRecords",
                    "ViewMedications",
                    "ViewPrescriptions"
                }
            },
            {
                Receptionist, new List<string>
                {
                    "ViewPatients",
                    "ManagePatients",
                    "ViewDoctors",
                    "ViewAppointments",
                    "ManageAppointments"
                }
            },
            {
                Accountant, new List<string>
                {
                    "ViewPatients",
                    "ViewInvoices",
                    "ManageInvoices"
                }
            },
            {
                Patient, new List<string>
                {
                    "ViewOwnAppointments",
                    "ManageOwnAppointments",
                    "ViewOwnMedicalRecords",
                    "ViewOwnPrescriptions",
                    "ViewOwnInvoices"
                }
            }
        };
    }
}
