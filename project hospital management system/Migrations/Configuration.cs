﻿namespace project_hospital_management_system.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<project_hospital_management_system.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
            ContextKey = "project_hospital_management_system.Models.ApplicationDbContext";
        }

        protected override void Seed(project_hospital_management_system.Models.ApplicationDbContext context)
        {
            // This method will be called after migrating to the latest version.
            // You can use the DbSet<T>.AddOrUpdate() helper extension method
            // to avoid creating duplicate seed data.

            // Add sample patients if the Patients table is empty
            if (!context.Patients.Any())
            {
                context.Patients.AddOrUpdate(
                    p => p.Name,
                    new Patient
                    {
                        Name = "John Smith",
                        Gender = "Male",
                        DateOfBirth = new DateTime(1980, 6, 15),
                        Phone = "555-123-4567",
                        Address = "123 Main St, Anytown, USA"
                    },
                    new Patient
                    {
                        Name = "Jane Doe",
                        Gender = "Female",
                        DateOfBirth = new DateTime(1992, 3, 22),
                        Phone = "555-987-6543",
                        Address = "456 Oak Ave, Somewhere, USA"
                    }
                );
            }
        }
    }
}
