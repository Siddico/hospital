﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using project_hospital_management_system.Models;

namespace project_hospital_management_system.Migrations
{
    public class RoleInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Create roles if they don't exist
            if (!roleManager.RoleExists(ApplicationRoles.Admin))
            {
                // Create Admin role
                var adminRole = new IdentityRole(ApplicationRoles.Admin);
                roleManager.Create(adminRole);

                // Create Doctor role
                var doctorRole = new IdentityRole(ApplicationRoles.Doctor);
                roleManager.Create(doctorRole);

                // Create Nurse role
                var nurseRole = new IdentityRole(ApplicationRoles.Nurse);
                roleManager.Create(nurseRole);

                // Create Receptionist role
                var receptionistRole = new IdentityRole(ApplicationRoles.Receptionist);
                roleManager.Create(receptionistRole);

                // Create Accountant role
                var accountantRole = new IdentityRole(ApplicationRoles.Accountant);
                roleManager.Create(accountantRole);

                // Create Patient role
                var patientRole = new IdentityRole(ApplicationRoles.Patient);
                roleManager.Create(patientRole);

                // Create default admin user with specific credentials
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    FirstName = "System",
                    LastName = "Administrator",
                    DateCreated = DateTime.Now,
                    IsActive = true
                };

                string adminPassword = "Admin12345678";
                var result = userManager.Create(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    // Add admin user to Admin role
                    userManager.AddToRole(adminUser.Id, ApplicationRoles.Admin);
                }

                // Create default doctor user
                if (context.Doctors.Any())
                {
                    var firstDoctor = context.Doctors.FirstOrDefault();
                    if (firstDoctor != null)
                    {
                        var doctorUser = new ApplicationUser
                        {
                            UserName = "doctor@hospital.com",
                            Email = "doctor@hospital.com",
                            FirstName = firstDoctor.Name.Split(' ')[0],
                            LastName = firstDoctor.Name.Contains(" ") ? firstDoctor.Name.Substring(firstDoctor.Name.IndexOf(" ") + 1) : "",
                            DateCreated = DateTime.Now,
                            IsActive = true,
                            DoctorId = firstDoctor.DoctorId
                        };

                        string doctorPassword = "Doctor12345678";
                        result = userManager.Create(doctorUser, doctorPassword);

                        if (result.Succeeded)
                        {
                            // Add doctor user to Doctor role
                            userManager.AddToRole(doctorUser.Id, ApplicationRoles.Doctor);
                        }
                    }
                }

                // Create default patient user
                if (context.Patients.Any())
                {
                    var firstPatient = context.Patients.FirstOrDefault();
                    if (firstPatient != null)
                    {
                        var patientUser = new ApplicationUser
                        {
                            UserName = "patient@hospital.com",
                            Email = "patient@hospital.com",
                            FirstName = firstPatient.Name.Split(' ')[0],
                            LastName = firstPatient.Name.Contains(" ") ? firstPatient.Name.Substring(firstPatient.Name.IndexOf(" ") + 1) : "",
                            DateCreated = DateTime.Now,
                            IsActive = true,
                            PatientId = firstPatient.PatientId
                        };

                        string patientPassword = "Patient12345678";
                        result = userManager.Create(patientUser, patientPassword);

                        if (result.Succeeded)
                        {
                            // Add patient user to Patient role
                            userManager.AddToRole(patientUser.Id, ApplicationRoles.Patient);
                        }
                    }
                }
            }
        }
    }
}
