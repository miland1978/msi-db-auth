using System;

namespace DataAccess.Audit
{
	public sealed class AuditScope : IDisposable
	{
		private readonly SharedDbContext _context;
		private readonly Z.EntityFramework.Plus.Audit _audit;

		public AuditScope(SharedDbContext context)
		{
			_context = context;
			_audit = new AuditBuilder(context).Create();
			_audit.PreSaveChanges(context);
		}

		public void Dispose()
		{
			_audit.PostSaveChanges();
			_audit.Configuration.AutoSavePreAction(_context, _audit);
		}
	}
}
