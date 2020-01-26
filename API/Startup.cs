using DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
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
			// Use SQL Database if in Azure, otherwise, use SQLite
			//if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
			services.AddDbContext<SharedDbContext>(options =>
							options.UseSqlServer(Configuration.GetConnectionString("SharedDb")));
			//else
			//	services.AddDbContext<SharedDbContext>(options =>
			//					options.UseSqlite("Data Source=localdatabase.db"));

			// Automatically perform database migration
			services.BuildServiceProvider().GetService<SharedDbContext>().Database.Migrate();

			services.AddControllers();
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

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
