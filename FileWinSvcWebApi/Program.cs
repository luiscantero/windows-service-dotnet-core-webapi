using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FileWinSvcWebApi
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("hosting.json", optional: false)
               .Build();

            var host = WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(config)
                //.UseUrls(new ConfigurationBuilder().AddEnvironmentVariables().Build()["server.urls"])
                .UseStartup<Startup>()
                .Build();

            if (Debugger.IsAttached || args.Contains("--debug"))
            {
                host.Run();
            }
            else
            {
                host.RunAsService();
            }

            return host;
        }
    }
}
