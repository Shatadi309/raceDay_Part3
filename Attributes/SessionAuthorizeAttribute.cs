using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace raceDay_Part3.Attributes
{//start of namespace
    public class SessionAuthorizeAttribute : Attribute, IAuthorizationFilter
    {//start of class
        private readonly string[] _roles;

        public SessionAuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {//start of method
            string? role = context.HttpContext.Session.GetString("Role");

            // Check whether the user is logged in
            if (string.IsNullOrEmpty(role))
            {
                context.Result = new RedirectToActionResult(
                    "Login",
                    "Account",
                    null
                );

                return;
            }

            // Check whether the user's role is permitted
            if (_roles.Length > 0 &&
                !_roles.Contains(role, StringComparer.OrdinalIgnoreCase))
            {
                context.Result = new RedirectToActionResult(
                    "AccessDenied",
                    "Home",
                    null
                );
            }
        }//end of method
    }//end of class
}//end of namespace