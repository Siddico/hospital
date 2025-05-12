﻿using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using project_hospital_management_system.Models;

namespace project_hospital_management_system.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AdminAuthorizationFilter : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            // Check if user is authenticated
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new { controller = "Account", action = "Login", returnUrl = filterContext.HttpContext.Request.Url.PathAndQuery }
                    )
                );
                return;
            }

            // Get the current user
            var userManager = filterContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var userId = filterContext.HttpContext.User.Identity.GetUserId();
            var user = userManager.FindById(userId);

            // Check if user is admin and has the correct email
            bool isAdmin = filterContext.HttpContext.User.IsInRole("Admin");
            bool hasCorrectEmail = user != null && user.Email == "admin@gmail.com";

            if (!isAdmin || !hasCorrectEmail)
            {
                // User is not admin or doesn't have the correct email, redirect to access denied
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new { controller = "Home", action = "AccessDenied" }
                    )
                );
            }
        }
    }
}
