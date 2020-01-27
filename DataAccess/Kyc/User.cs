using System;
using System.ComponentModel.DataAnnotations;
using DataAccess.Audit;

namespace DataAccess.Kyc
{
	public class User : IAudited
	{
		public Guid Id { get; set; }

		[StringLength(64)]
		public string FirstName { get; set; }

		[StringLength(64)]
		public string LastName { get; set; }

		public string ModifiedWith { get; set; }

		public string ModifiedBy { get; set; }
	}
}
