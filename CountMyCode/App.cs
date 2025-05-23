using CountMyCode.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CountMyCode.Utils;
using Microsoft.Extensions.FileProviders;

namespace CountMyCode
{
    internal class App
    {
        private readonly string[] args;
        private readonly FileItem _initialFolder;

        internal App(string[] args, string initialPath)
        {
            this.args = args;
            _initialFolder = new FileItem(initialPath, Status.Folder);
        }

        internal async Task<bool> Run(int portNumber)
        {
            while (true)
            {
                bool? menuResult = _initialFolder.RunMenu();
                if (menuResult == null)
                    return false;

                LoadingScreen();

                _initialFolder.FilterItems();

                LoadingScreen();

                bool extensionsResult = _initialFolder.RunExtensionsMenu();
                if (!extensionsResult)
                    continue;

                LoadingScreen();

                AuditStats stats = await AuditUtil.RunAudit(_initialFolder);

                LaunchAudit(stats, portNumber);

                break;
            }

            return true;
        }

        internal void LoadingScreen()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Loading...");
        }

        internal void LaunchAudit(AuditStats audit, int portNumber)
        {
            Console.WriteLine("Audit complete. Spinning up web server...");

            var builder = Host.CreateDefaultBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders(); 
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseUrls($"http://localhost:{portNumber}")
                        .ConfigureServices(services =>
                        {
                            services.AddSingleton(audit);
                            services.AddControllers();
                        })
                        .Configure(app =>
                        {
                            var staticFilePath = Path.Combine(Directory.GetCurrentDirectory(), "WebFiles");

                            app.UseStaticFiles(new StaticFileOptions
                            {
                                FileProvider = new PhysicalFileProvider(staticFilePath),
                                RequestPath = "" // Serve at root
                            });

                            app.UseRouting();

                            app.Use(async (context, next) =>
                            {
                                if (context.Request.Path == "/audit")
                                {
                                    context.Response.Redirect("/index.html");
                                    return;
                                }

                                await next();
                            });

                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapControllers();
                            });
                        });

                });

            var host = builder.Build();

            _ = host.StartAsync(); 

            Console.WriteLine("Web server started. Opening browser...");

            string url = $"http://localhost:{portNumber}/audit";
            BrowserUtils.OpenUrl(url); 
        }
    }
}
