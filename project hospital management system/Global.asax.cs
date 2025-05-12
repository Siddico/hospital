using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using System.IO;
using project_hospital_management_system.Models;
using project_hospital_management_system.Migrations;

namespace project_hospital_management_system
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Set the DataDirectory to the App_Data folder
            string dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            if (string.IsNullOrEmpty(dataDirectory))
            {
                dataDirectory = HttpContext.Current.Server.MapPath("~/App_Data");
                AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory);
            }

            // Ensure App_Data directory exists
            if (!System.IO.Directory.Exists(dataDirectory))
            {
                System.IO.Directory.CreateDirectory(dataDirectory);
            }

            // Initialize the database with our custom initializer
            Database.SetInitializer(new Migrations.DatabaseInitializer());

            // Force the database to be created/updated at application start
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    // This will trigger the Seed method in DatabaseInitializer
                    db.Database.Initialize(force: true);
                    System.Diagnostics.Debug.WriteLine("Database initialized successfully.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error initializing database: " + ex.Message);
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine("Inner Exception: " + ex.InnerException.Message);
                    if (ex.InnerException.InnerException != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Inner Inner Exception: " + ex.InnerException.InnerException.Message);
                    }
                }
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Server.ClearError();

            // Log the exception
            System.Diagnostics.Debug.WriteLine("Global Error: " + exception.Message);

            // Redirect to error page
            Response.Redirect("~/Error");
        }
    }
}
