using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Agh.eSzachy.Data;
using Agh.eSzachy.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Agh;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Agh.eSzachy
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRoomService, RoomService>();
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowAny",
                    x =>
                    {
                        x.AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed(isOriginAllowed: _ => true)
                        .AllowCredentials();
                    });
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    this.Configuration.GetConnectionString("DefaultConnection"), x => x.ServerVersion(new Version(5, 5, 62), ServerType.MySql)));

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer(x =>
            {
                x.Discovery.ShowApiScopes = true;
                x.Discovery.ShowClaims = true;
            })
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>()
                .AddDeveloperSigningCredential();

            services.AddAuthentication()
                .AddGoogle(o =>
                {
                    var googleAuthNSection = this.Configuration.GetSection("Authentication:Google");
                    o.ClientId = googleAuthNSection["ClientId"];
                    o.ClientSecret = googleAuthNSection["ClientSecret"];
                    o.AuthorizationEndpoint += "?prompt=consent"; // Hack so we always get a refresh token, it only comes on the first authorization response
                    o.Scope.Add("openid");
                    o.Scope.Add("profile");
                    o.Scope.Add("email");
                    o.Scope.Add("https://www.googleapis.com/auth/userinfo.email");
                    o.ClaimActions.MapJsonKey(ClaimTypes.Email, "email", ClaimValueTypes.Email);
                    o.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                    o.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
                    o.ClaimActions.MapJsonSubKey("urn:google:image", "image", "url");
                })
                .AddIdentityServerJwt();
            services.AddControllersWithViews();
            services.AddRazorPages();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddSignalR(x =>
            {
                x.EnableDetailedErrors = true;
            }).AddJsonProtocol();
            services.AddTransient<IUserIdProvider, EmailBasedUserIdProvider>();

            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHub<RoomHub>("/room");
                endpoints.MapHub<GameHub>("/game");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
