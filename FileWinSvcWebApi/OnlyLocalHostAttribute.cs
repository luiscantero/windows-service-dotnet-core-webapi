using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace FileWinSvcWebApi.Filters
{
    public class OnlyLocalHostAttribute : Attribute, IResourceFilter
    {
        //string _allowedIP = null;

        //public OnlyLocalHostAttribute(string allowedIP)
        //{
        //    _allowedIP = allowedIP;
        //}

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            // If IP is not localhost, show 403.
            if (!IPAddress.IsLoopback(context.HttpContext.Connection.RemoteIpAddress))
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}