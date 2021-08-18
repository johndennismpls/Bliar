using Blazored.LocalStorage;
using Bliar.Areas.Identity;
using Bliar.Data;
using Bliar.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bliar
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
             .AddAuthentication("Identity.Application")
             .AddCookie();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services
                .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                 .AddDefaultTokenProviders();

            services.AddRazorPages();
            services.AddHttpClient();
            services.AddServerSideBlazor();
            services.AddBlazoredLocalStorage();

            //services.AddAuthentication(options =>
            //{
            //    // Identity made Cookie authentication the default.
            //    // However, we want JWT Bearer Auth to be the default.
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            // .AddJwtBearer(options =>
            // {
            //     // Configure the Authority to the expected value for your authentication provider
            //     // This ensures the token is appropriately validated
            //     options.Authority = /* TODO: Insert Authority URL here */;

            //     // We have to hook the OnMessageReceived event in order to
            //     // allow the JWT authentication handler to read the access
            //     // token from the query string when a WebSocket or 
            //     // Server-Sent Events request comes in.

            //     // Sending the access token in the query string is required due to
            //     // a limitation in Browser APIs. We restrict it to only calls to the
            //     // SignalR hub in this code.
            //     // See https://docs.microsoft.com/aspnet/core/signalr/security#access-token-logging
            //     // for more information about security considerations when using
            //     // the query string to transmit the access token.
            //     options.Events = new JwtBearerEvents
            //     {
            //         OnMessageReceived = context =>
            //         {
            //             var accessToken = context.Request.Query["access_token"];

            //             // If the request is for our hub...
            //             var path = context.HttpContext.Request.Path;
            //             if (!string.IsNullOrEmpty(accessToken) &&
            //                 (path.StartsWithSegments("/hubs/chat")))
            //             {
            //                 // Read the token out of the query string
            //                 context.Token = accessToken;
            //             }
            //             return Task.CompletedTask;
            //         }
            //     };
            // });


            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddSingleton<WeatherForecastService>();
            services.AddScoped<CookiesProvider>();

            //        services.AddAuthentication()
            //.AddIdentityServerJwt();
            //        services.TryAddEnumerable(
            //            ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>,
            //                ConfigureJwtBearerOptions>());
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            //app.UseCookiePolicy();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapHub<BlazorChatSampleHub>(BlazorChatSampleHub.HubUrl);
            });
        }
    }
}
