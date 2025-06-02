using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using social.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using social.Chat;
using social.Utilities;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Owin.Cors;

namespace social
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            Configuration = configuration;
         
        }
        
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddCors();
            services.AddSignalR(c => {
                c.EnableDetailedErrors = true;
                c.ClientTimeoutInterval = TimeSpan.MaxValue;
                c.KeepAliveInterval = TimeSpan.MaxValue;
            });
            services.AddControllersWithViews();
            services.AddScoped<ChatHub>();
            services.AddScoped<ChatHub2>();
            services.AddScoped<ChatHub3>();
            services.AddScoped<ChatHub4>();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user cowwnsent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            
            });
            services.AddAuthentication(options =>

            {

                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(options =>

            {

                options.ExpireTimeSpan = TimeSpan.FromDays(360);



                options.SlidingExpiration = true;

            });

            var connectionString = Configuration.GetConnectionString("textonitContext");
            services.AddDbContext<Contextsocial>(options =>
                options.UseSqlServer(connectionString)
            );
            services.AddDbContext<Contextsocial>(opt =>
            {
                opt.EnableSensitiveDataLogging();
            });
            services.AddSingleton<IViewRenderService, RenderViewToString>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
         
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSignalR(route =>
            {
                route.MapHub<ChatHub>("/chatHub");
                route.MapHub<ChatHub2>("/chatHub2");
                route.MapHub<ChatHub3>("/chatHub3");
                route.MapHub<ChatHub4>("/chatHub4");


            });
            app.UseEndpoints(endpoints =>
            {
               
                endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller=Guest}/{action=guestt}/{id?}");
           
                endpoints.MapAreaControllerRoute(
            name: "areas",
            areaName: "AdminPanel",
            pattern: "{area:exists}/{controller=Admin}/{action=Index}");

            });
        }
    }
}
