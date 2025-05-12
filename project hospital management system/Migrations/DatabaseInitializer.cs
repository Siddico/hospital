using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using project_hospital_management_system.Models;

namespace project_hospital_management_system.Migrations
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            try
            {
                // Initialize roles and admin user
                RoleInitializer.Initialize(context);

                // Call the base Seed method to run the Configuration.Seed method
                base.Seed(context);

                // Add dashboard statistics
                if (!context.DashboardStatistics.Any())
                {
                    context.DashboardStatistics.AddRange(new List<DashboardStatistic>
                    {
                        new DashboardStatistic
                        {
                            StatisticName = "Total Patients",
                            StatisticValue = context.Patients.Count().ToString(),
                            Category = "System",
                            Icon = "user",
                            ColorClass = "primary"
                        },
                        new DashboardStatistic
                        {
                            StatisticName = "Total Doctors",
                            StatisticValue = context.Doctors.Count().ToString(),
                            Category = "System",
                            Icon = "user-md",
                            ColorClass = "success"
                        },
                        new DashboardStatistic
                        {
                            StatisticName = "Total Appointments",
                            StatisticValue = context.Appointments.Count().ToString(),
                            Category = "System",
                            Icon = "calendar",
                            ColorClass = "info"
                        },
                        new DashboardStatistic
                        {
                            StatisticName = "Total Revenue",
                            StatisticValue = "$" + context.Invoices.Sum(i => i.TotalAmount).ToString("N2"),
                            Category = "Financial",
                            Icon = "money",
                            ColorClass = "success"
                        },
                        new DashboardStatistic
                        {
                            StatisticName = "Pending Payments",
                            StatisticValue = "$" + context.Invoices.Where(i => i.Status == "Pending").Sum(i => i.TotalAmount).ToString("N2"),
                            Category = "Financial",
                            Icon = "clock-o",
                            ColorClass = "warning"
                        },
                        new DashboardStatistic
                        {
                            StatisticName = "Overdue Payments",
                            StatisticValue = "$" + context.Invoices.Where(i => i.Status == "Overdue").Sum(i => i.TotalAmount).ToString("N2"),
                            Category = "Financial",
                            Icon = "exclamation-triangle",
                            ColorClass = "danger"
                        }
                    });

                    context.SaveChanges();
                }

                System.Diagnostics.Debug.WriteLine("Database initialized successfully.");
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine("Error in DatabaseInitializer.Seed: " + ex.Message);
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                
                // Rethrow the exception to prevent database initialization
                throw;
            }
        }
    }
}
