using System;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using project_hospital_management_system.Migrations;

namespace project_hospital_management_system.Models
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);

            // Initialize the database with seed data
            var configuration = new Configuration();
            configuration.Seed(context);

            // Initialize roles and admin user
            InitializeRolesAndUsers(context);
        }

        private void InitializeRolesAndUsers(ApplicationDbContext context)
        {
            try
            {
                // Initialize roles and admin user using RoleInitializer
                RoleInitializer.Initialize(context);
                System.Diagnostics.Debug.WriteLine("Roles and admin user initialized successfully.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error initializing roles and users: " + ex.Message);
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                throw; // Re-throw the exception to be handled by the caller
            }
        }
    }
}