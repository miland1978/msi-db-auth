using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Audit
{
	public class ExtendedAuditEntry
	{
		public Guid Id { get; set; }

		[Required]
		[StringLength(256)]
		public string Key { get; set; }

		public string Changes { get; set; }
	}
}
