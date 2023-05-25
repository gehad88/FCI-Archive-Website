using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using modelfor_archive.Models;

namespace Archive2
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
            services.AddSingleton<Archive2Context>(); // Register Archive2Context as a singleton service
            services.AddControllersWithViews(); // Add MVC controllers and views
            services.AddCors(); // Add CORS support
            services.AddControllersWithViews(); // Add MVC controllers and views
            services.AddSession(
                options => {
                    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session idle timeout to 30 minutes
                    //options.Cookie.HttpOnly = true; // Set session cookie as HttpOnly
                    //options.Cookie.IsEssential = true; // Mark session cookie as essential
                }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Use developer exception page in the development environment
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // Use custom error page for handling exceptions
                app.UseHsts(); // Use HTTP Strict Transport Security (HSTS)
            }

            app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
            app.UseStaticFiles(); // Serve static files

            app.UseSession(); // Enable session support
            app.UseRouting(); // Enable routing
            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()); // Allow cross-origin requests
            app.UseAuthorization(); // Enable authorization

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}"); // Define the default controller and action route
            });
        }
    }
}
