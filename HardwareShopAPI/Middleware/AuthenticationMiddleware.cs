using Common.Models;
using DataLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace HardwareShopAPI.Middleware
{
    public static class AuthenticationMiddlewareExtension
    {
        public static IApplicationBuilder UseFakeAuthentication(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<AuthenticationMiddleware>();
        }
    }

    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate next;
        private readonly UserRepository userRepo;

        public AuthenticationMiddleware(RequestDelegate next, UserRepository userRepo)
        {
            this.next = next;
            this.userRepo = userRepo;
        }

        public Task InvokeAsync(HttpContext context)
        {
            FakeAuthenticate(context);
            return next.Invoke(context);
        }

        public void FakeAuthenticate(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"];

            User user = null;
            if (authHeader.Equals("admin"))
            {
                user = userRepo.Get("admin");
            }
            else
            {
                user = userRepo.Get("customer");
            }
            var userPrincipal = new GenericPrincipal(new GenericIdentity(user.Username), new string[] { user.Role.ToString() });
            context.User = userPrincipal;
        }
    }
}
