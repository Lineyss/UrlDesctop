namespace UrlDesctopLinux
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMvc(options => options.EnableEndpointRouting = false);

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