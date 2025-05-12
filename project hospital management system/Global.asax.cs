using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
<<<<<<< HEAD
using System.Data.Entity;
using System.IO;
using project_hospital_management_system.Models; // Make sure this line is included
=======
>>>>>>> 00a11ec54d2ce5d42722424882e545991dc44544

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
<<<<<<< HEAD

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

            // Initialize the database
            Database.SetInitializer(new DatabaseInitializer()); // Use it directly since you've included the namespace

            // Force the database to be created/updated at application start
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    if (!db.Database.Exists())
                    {
                        db.Database.Create();
                        System.Diagnostics.Debug.WriteLine("Database created successfully.");
                    }
                    else
                    {
                        db.Database.Initialize(force: false);
                        System.Diagnostics.Debug.WriteLine("Database initialized successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error initializing database: " + ex.Message);
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine("Inner Exception: " + ex.InnerException.Message);
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
=======
        }
    }
}
>>>>>>> 00a11ec54d2ce5d42722424882e545991dc44544
