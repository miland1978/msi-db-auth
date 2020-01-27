using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace DataAccess.Audit
{
	public class AuditBuilder
	{
		private readonly SharedDbContext _context;

		public AuditBuilder(SharedDbContext context)
		{
			_context = context;
		}

		public Z.EntityFramework.Plus.Audit Create()
		{
			var audit = new Z.EntityFramework.Plus.Audit();

			audit.Configuration.Exclude(x => true);
			audit.Configuration.Include<IAudited>();
			audit.Configuration.IgnorePropertyUnchanged = true;
			audit.Configuration.AuditEntryFactory = AuditEntryFactory;
			audit.Configuration.AutoSavePreAction = AutoSavePreAction;

			return audit;
		}

		private AuditEntry AuditEntryFactory(AuditEntryFactoryArgs args)
		{
			var key = args.EntityEntry.Metadata.FindPrimaryKey();
			var keyValues = key.Properties.Select(prop =>
			{
				var currentValue = args.EntityEntry.Property(prop.Name).CurrentValue;
				return AuditManager.DefaultConfiguration.FormatValue(args.EntityEntry, prop.Name, currentValue);
			});

			var entry = new ExtendedAuditEntry
			{
				CreatedBy = "System",
				CreatedDate = DateTime.UtcNow,
				ModifiedWith = _context.AuditSystem,
				ModifiedBy = _context.AuditModifier,
				KeyValue = string.Join("|", keyValues)
			};

			return entry;
		}

		private static void AutoSavePreAction(DbContext context, Z.EntityFramework.Plus.Audit audit)
		{
			context.Set<ExtendedAuditEntry>().AddRange(audit.Entries.Cast<ExtendedAuditEntry>());
		}
	}
}
