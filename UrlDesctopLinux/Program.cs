using System.Net;

namespace UrlDesctopLinux
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

            builder.Services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
                options.ExcludedHosts.Add("example.com");
                options.ExcludedHosts.Add("www.example.com");
            });

            builder.Services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = (int)HttpStatusCode.TemporaryRedirect;
                options.HttpsPort = 5001;
            });

            if (!builder.Environment.IsDevelopment())
            {
                builder.Services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
                    options.HttpsPort = 443;
                });
            }

            // Add services to the container.

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                     name: "default",
                     template: "{*url}",
                     defaults: new { controller = "Desctop", action = "Index" });
            });

            app.Run();
        }
    }
}