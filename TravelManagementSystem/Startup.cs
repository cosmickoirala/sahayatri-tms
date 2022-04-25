using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using TravelManagementSystem.Models;
using TravelManagementSystem.Data;
using TravelManagementSystem.Infrastructure.ApplicationUserClaims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using TravelManagementSystem.Infrastructure.AppSettingsModels;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TravelManagementSystem
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
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(5);//You can set Time   
            });
            services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("ConnectionStringUser")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, ApplicationUserClaimsPrincipalFactory>();

            services.AddControllersWithViews();
           

            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // Default Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                // Default User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "Shahayatri.Cookies";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                // You might want to only set the application cookies over a secure connection:
                // options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.SlidingExpiration = true;
            });
            services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
            {
                //options.AccessDeniedPath = "/account/accessdenied";
                options.LoginPath = "/account/index";
                options.LogoutPath = "/account/logout";
                options.ReturnUrlParameter = "place/index";
            });

            services.AddMvc()
           .AddRazorPagesOptions(options =>
           {
               options.Conventions.AuthorizePage("/Account/Index");
           });





            services.AddAntiforgery();

            services.Configure<ScriptTags>(Configuration.GetSection(nameof(ScriptTags)));

        

        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
