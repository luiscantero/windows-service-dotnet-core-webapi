using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace FileWinSvcWebApi.Filters
{
    public class OnlyLocalHostAttribute : Attribute, IAuthorizationFilter
    {
        //string _allowedIP = null;

        //public OnlyLocalHostFilter(string allowedIP)
        //{
        //    _allowedIP = allowedIP;
        //}

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // If IP is not localhost, show 403.
            bool isLocalHost = IPAddress.IsLoopback(context.HttpContext.Connection.RemoteIpAddress);
            if (!isLocalHost)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            }
        }
    }
}