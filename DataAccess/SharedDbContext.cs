using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Audit;
using DataAccess.Kyc;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace DataAccess
{
	public class SharedDbContext : DbContext
	{
		public SharedDbContext(DbContextOptions<SharedDbContext> options)
			: base(options)
		{
			var conn = (Microsoft.Data.SqlClient.SqlConnection)Database.GetDbConnection();
			//conn.AccessToken = (new Microsoft.Azure.Services.AppAuthentication.AzureServiceTokenProvider()).GetAccessTokenAsync("https://database.windows.net/").Result;
		}

		public DbSet<User> Users { get; set; }

		public DbSet<ExtendedAuditEntry> AuditEntries { get; set; }

		public DbSet<AuditEntryProperty> AuditEntryProperties { get; set; }

		public string AuditSystem { get; set; }

		public string AuditModifier { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ExtendedAuditEntry>()
				.HasIndex(x => new
				{
					x.KeyValue,
					x.EntityTypeName
				});
		}

		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			UpdateValues();

			int rowsAffected;
			using (new AuditScope(this))
			{
				rowsAffected = base.SaveChanges(acceptAllChangesOnSuccess);
			}

			base.SaveChanges(acceptAllChangesOnSuccess);
			return rowsAffected;
		}

		public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
		{
			UpdateValues();

			int rowsAffected;
			using (new AuditScope(this))
			{
				rowsAffected = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
			}

			await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
			return rowsAffected;
		}

		private void UpdateValues()
		{
			foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
			{
				if (entry.Entity is IAudited audited)
				{
					audited.ModifiedWith = AuditSystem;
					audited.ModifiedBy = AuditModifier;
				}
			}
		}
	}
}
