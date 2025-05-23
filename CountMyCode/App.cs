using CountMyCode.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CountMyCode.Utils;
using Microsoft.Extensions.FileProviders;

namespace CountMyCode;

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

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = Array.Empty<string>(),
            ApplicationName = System.Reflection.Assembly.GetExecutingAssembly().FullName,
            ContentRootPath = Directory.GetCurrentDirectory(),
        });

        // Configure logging
        builder.Logging.ClearProviders();

        // Set up services
        builder.Services.AddSingleton(audit);
        builder.Services.AddControllers();

        var app = builder.Build();

        var staticFilePath = Path.Combine(Directory.GetCurrentDirectory(), "WebFiles");

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(staticFilePath),
            RequestPath = ""
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

        // Run the app on specified port
        app.Urls.Add($"http://localhost:{portNumber}");
        _ = app.StartAsync();

        Console.WriteLine("Web server started. Opening browser...");

        string url = $"http://localhost:{portNumber}/audit";
        BrowserUtils.OpenUrl(url);
    }
}
