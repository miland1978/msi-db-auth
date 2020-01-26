using DataAccess.Audit;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
	public class SharedDbContext : DbContext
	{
		public SharedDbContext(DbContextOptions<SharedDbContext> options)
			: base(options)
		{
			var conn = (Microsoft.Data.SqlClient.SqlConnection)Database.GetDbConnection();
			conn.AccessToken = (new Microsoft.Azure.Services.AppAuthentication.AzureServiceTokenProvider()).GetAccessTokenAsync("https://database.windows.net/").Result;
		}

		public DbSet<ExtendedAuditEntry> AuditEntries { get; set; }
	}
}
