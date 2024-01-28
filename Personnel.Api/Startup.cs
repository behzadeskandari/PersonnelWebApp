using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Personnel.Infra.Data.Context;
using Personnel.Infra.IoC;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using System;
using Personnel.Domain.Entities.Identity;
using Personnel.Application.Interfaces;
using Personnel.Application.Services;
using Personnel.Infra.Data.Contracts;
using Personnel.Infra.Data.Interfaces;
using Personnel.Infra.Data.Stores;
using Personnel.Infra.Data.Factories;
using Personnel.Application.Mangers;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;

namespace Personnel.Api
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

            //var persianCulture = new CultureInfo("fa-IR");
            //persianCulture.NumberFormat.NumberDecimalSeparator = ".";
            //persianCulture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
            //CultureInfo[] supportedCultures = new[] { persianCulture, new CultureInfo("en-US") };

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-US");
                //options.SupportedCultures = supportedCultures;
                //options.SupportedUICultures = supportedCultures;
                options.SupportedCultures = new List<CultureInfo> { new CultureInfo("en-US") };
                options.SupportedUICultures = new List<CultureInfo> { new CultureInfo("en-US") };
                options.ApplyCurrentCultureToResponseHeaders = false;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider(),
                };
            });
            CultureInfo.CurrentCulture = CultureInfo.DefaultThreadCurrentCulture;
            CultureInfo.CurrentUICulture = CultureInfo.DefaultThreadCurrentUICulture;

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //services.AddDbContext<PersonnelDbContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.AddIdentity<User, Roles>()
            //     .AddEntityFrameworkStores<PersonnelDbContext>()
            //     .AddDefaultUI()
            //     .AddDefaultTokenProviders();

            services.AddCors(options =>
            {
                options.AddPolicy("DomainPolicy",
                    x =>
                    {
                        x.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            services.AddDbContext<PersonnelDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserClaimsPrincipalFactory<User>, BaseUserClaimsPrincipleFactory>();
            services.AddScoped<IRoleStore<Roles>, BaseRoleStore>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserStore<User>, BaseUserStore>();
            //services.AddScoped<IProfileService, ProfileService>();
            //services.AddScoped<IUserInRoleService, UserInRoleService>();
            //services.AddScoped<IPermissionRecordService, PermissionRecordService>();
            //services.AddAutoMapper(typeof(IoC));

            services.AddIdentity<User, Roles>(options =>
            {
                options.Stores.ProtectPersonalData = false;

                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.AllowedForNewUsers = false;
                options.User.RequireUniqueEmail = false;



            }).AddUserStore<BaseUserStore>().AddRoleStore<BaseRoleStore>().AddUserManager<BaseUserManager>().AddRoleManager<BaseRoleManager>().AddSignInManager<BaseSignInManager>()
            .AddClaimsPrincipalFactory<BaseUserClaimsPrincipleFactory>().AddDefaultTokenProviders().AddEntityFrameworkStores<PersonnelDbContext>();
            services.AddLogging(builder =>
            {
                builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Debug);
                builder.AddConsole();
            });
            MediatRServiceConfiguration mediat = new MediatRServiceConfiguration();
            mediat.MediatorImplementationType = typeof(Startup);

            services.AddMediatR(mediat);

            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "University Api V1");
            });

            app.UseMvc();
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});
        }

        private static void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);
        }
    }
}
