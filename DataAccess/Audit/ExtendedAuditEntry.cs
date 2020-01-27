using System.ComponentModel.DataAnnotations;
using Z.EntityFramework.Plus;

namespace DataAccess.Audit
{
	public class ExtendedAuditEntry : AuditEntry
	{
		[Required]
		[StringLength(128)]
		public string KeyValue { get; set; }

		[Required]
		[StringLength(50)]
		public string ModifiedWith { get; set; }

		[Required]
		[StringLength(50)]
		public string ModifiedBy { get; set; }
	}
}
