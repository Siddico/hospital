﻿using System;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using System.Text;
using System.Diagnostics;

namespace project_hospital_management_system.Utilities
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            // Don't handle the exception if it's already been handled
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            // Only handle requests that are not AJAX requests
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                return;
            }

            Exception exception = filterContext.Exception;

            // Log the exception
            LogException(exception);

            // Handle different types of exceptions differently
            if (exception is DbEntityValidationException)
            {
                HandleDbEntityValidationException((DbEntityValidationException)exception, filterContext);
            }
            else if (exception is HttpException)
            {
                HandleHttpException((HttpException)exception, filterContext);
            }
            else
            {
                // Handle all other exceptions
                HandleGenericException(exception, filterContext);
            }

            // Mark the exception as handled
            filterContext.ExceptionHandled = true;

            // Clear the error from the server
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

        private void HandleDbEntityValidationException(DbEntityValidationException exception, ExceptionContext filterContext)
        {
            StringBuilder errorMessage = new StringBuilder("Entity Validation Errors:\n");

            foreach (var validationErrors in exception.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    errorMessage.AppendFormat("Property: {0} Error: {1}\n", validationError.PropertyName, validationError.ErrorMessage);
                }
            }

            // Log the detailed error
            Debug.WriteLine(errorMessage.ToString());

            // Create a simplified model for the error view
            var model = new HandleErrorInfo(exception, filterContext.RouteData.Values["controller"].ToString(), filterContext.RouteData.Values["action"].ToString());

            // Render the Error view
            filterContext.Result = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model)
            };
        }

        private void HandleHttpException(HttpException exception, ExceptionContext filterContext)
        {
            int statusCode = exception.GetHttpCode();

            // Render the appropriate error view based on status code
            switch (statusCode)
            {
                case 404:
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "NotFound",
                        ViewData = new ViewDataDictionary { { "Message", exception.Message } }
                    };
                    break;
                case 403:
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "Forbidden",
                        ViewData = new ViewDataDictionary { { "Message", exception.Message } }
                    };
                    break;
                default:
                    // For other HTTP errors, use the generic error view
                    var model = new HandleErrorInfo(exception, filterContext.RouteData.Values["controller"].ToString(), filterContext.RouteData.Values["action"].ToString());
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "Error",
                        ViewData = new ViewDataDictionary<HandleErrorInfo>(model)
                    };
                    break;
            }
        }

        private void HandleGenericException(Exception exception, ExceptionContext filterContext)
        {
            // Create a model for the error view
            var model = new HandleErrorInfo(exception, filterContext.RouteData.Values["controller"].ToString(), filterContext.RouteData.Values["action"].ToString());

            // Render the Error view
            filterContext.Result = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model)
            };
        }

        private void LogException(Exception exception)
        {
            // Log the exception - in a real application, you would use a proper logging framework
            Debug.WriteLine("Exception: " + exception.Message);
            Debug.WriteLine("Stack Trace: " + exception.StackTrace);

            if (exception.InnerException != null)
            {
                Debug.WriteLine("Inner Exception: " + exception.InnerException.Message);
                Debug.WriteLine("Inner Exception Stack Trace: " + exception.InnerException.StackTrace);
            }
        }
    }
}
