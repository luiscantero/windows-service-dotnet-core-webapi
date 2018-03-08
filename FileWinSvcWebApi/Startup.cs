using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FileWinSvcWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // https://docs.microsoft.com/en-us/aspnet/core/security/cors
            app.UseCors(builder =>
                        builder.WithOrigins("http://localhost", // Explicit domain and standard port.
                                            "http://localhost:5111")); // Explicit domain and port.

            app.UseMvc();

            MapWebSocketsApp(app);
        }

        private void MapWebSocketsApp(IApplicationBuilder app)
        {
            app.Map("/wsecho", managedWebSocketsApp =>
            {
                managedWebSocketsApp.UseWebSockets(new WebSocketOptions { });

                managedWebSocketsApp.Use(async (context, next) =>
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        Console.WriteLine("Echo server at: " + context.Request.PathBase);
                        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        await Echo(webSocket);
                        return;
                    }
                    await next();
                });
            });
        }

        private async Task Echo(WebSocket webSocket)
        {
            byte[] buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}