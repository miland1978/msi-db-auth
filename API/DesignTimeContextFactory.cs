//using DataAccess;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;

//namespace API
//{
//	public class DesignTimeContextFactory
//	{
//		public SharedDbContext CreateDbContext(string[] args)
//		{
//			IConfigurationRoot config = new ConfigurationBuilder()
//				.AddJsonFile("appsettings.json", optional: false)
//				.AddUserSecrets<Startup>()
//				.AddEnvironmentVariables()
//				.Build();

//			string connString = config.GetConnectionString("SharedDb");

//			var optionsBuilder = new DbContextOptionsBuilder<SharedDbContext>();
//			optionsBuilder.UseSqlServer(connString/*, x => x.UseNetTopologySuite()*/);

//			var context = new SharedDbContext(optionsBuilder.Options);
//			context.Database.SetCommandTimeout(System.TimeSpan.FromMinutes(5));
//			return context;
//		}
//	}
//}
