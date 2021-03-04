using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Contentful.AspNetCore;
using Web.Db;
using Web.Infrastructure;
using Web.Snippets;
using Web.Snippets.Microsoft.Extensions.Configuration;
using Web.Services;
using Contentful.Core.Models;
using Web.Infrastructure.Contentful.Renderers;

namespace Web
{
	public class Startup
	{
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _currentEnvironment = env;
            _configuration = configuration;
        }
        
        IWebHostEnvironment _currentEnvironment { get; }
        IConfiguration _configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			ConfigMan configMan = new ConfigMan(_configuration);
			services.AddDbContext<DataContext>(options => options.UseSqlServer(Settings.ConnectionStrings.DataContext));

			services.AddDistributedMemoryCache();
			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(20);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});

            if(_currentEnvironment.IsDevelopment())
            {
                services.AddControllersWithViews().AddRazorRuntimeCompilation().AddSessionStateTempDataProvider(); 
            }
			else
            {
                services.AddControllersWithViews().AddSessionStateTempDataProvider(); 
            }

            services.AddSingleton<IEmailSender, IdentityEmailSender>();

			services.AddContentful(_configuration);
			services.AddTransient((c) => {
				var renderer = new HtmlRenderer();
				renderer.AddRenderer(new SimpleHyperlinkRenderer() { Order = 10 });
				return renderer;
			});

			services.AddRazorPages() 
                .AddRazorPagesOptions(options =>  //locks down following folder and page for only users that are logged in (regardless of Role)
                {
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage"); //folder for managing own identity
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout"); //page that makes no sense unless logged in
                });

            services.AddAuthorization(options => //setup policy we can use on MVC controllers, actions and razor pages 
            {
                options.AddPolicy(Settings.Authorization.EditRights, policy => policy.RequireRole(Settings.Authorization.EditRightsRoles));
            });

            services.AddTransient<UsersFacade>();
            services.AddScoped<IContentfulService, ContentfulService>();
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseGuidAuth();

			if(_currentEnvironment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				if(_currentEnvironment.IsProduction())
				{
					app.UseHttpsRedirection();
				}
				app.UseExceptionHandler("/Home/Error");
			}
			
			app.UseStaticFiles();
			app.UseRouting();
            app.UseAuthentication();//Identity
			app.UseAuthorization();
			app.UseSession(); 
			app.UseEndpoints(endpoints => {
				endpoints.MapControllerRoute(
					name: "controller_less_default",
					pattern: "{action}",
					defaults: new { controller = "Home", action = "Index" });
				endpoints.MapRazorPages(); //Identity
			});
		}
	}
}