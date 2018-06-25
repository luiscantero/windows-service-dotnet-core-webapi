using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace FileWinSvcWebApi
{
    public class WebSocketsMiddleware
    {
        private readonly RequestDelegate _next;

        public WebSocketsMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, IHostingEnvironment env)
        {
            if (context.Request.Path == "/wsecho")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    Console.WriteLine("Echo server at: " + context.Request.PathBase);
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await Echo(context, webSocket);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task Echo(HttpContext context, WebSocket webSocket)
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