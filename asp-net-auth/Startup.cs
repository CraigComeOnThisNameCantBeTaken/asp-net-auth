using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp_net_auth.Authorization;
using asp_net_auth.Authorization.Requirements;
using Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace asp_net_auth
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
            var databaseConnectionString = Configuration["DbConnection"];
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(databaseConnectionString);
            });

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

            // cookie scheme setup that efcore identity can use - is different to normal cookie scheme
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.Name = "MyCookie";
                options.ExpireTimeSpan = TimeSpan.FromHours(1);

                // by default the challenge will redirect to a login page
                // but this application does not have views so we will just return a 401
                options.Events.OnRedirectToLogin = (context) =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };

                options.Events.OnRedirectToAccessDenied = (context) =>
                {
                    context.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };
            });

            services.AddAuthorizationHandlers();
            // there is a default role authorisation approach by we can register an example to show checking for a specific
            // role via a policy - just for learning, not production code
            services.AddAuthorization(options =>
            {
                options.AddPolicy("adminLevel", policy =>
                    policy.Requirements.Add(new AdminRequirement()));
            });

            // normal authentication using cookie scheme
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //  .AddCookie(options =>
            //  {
            //      options.Cookie.HttpOnly = true;
            //      options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //      options.Cookie.SameSite = SameSiteMode.Lax;
            //      options.Cookie.Name = "MyCookie";
            //  });

            // allow controllers / actions to opt out of authorization instead of opt in
            services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
