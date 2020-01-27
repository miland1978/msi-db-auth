using System.ComponentModel.DataAnnotations;

namespace DataAccess.Audit
{
	internal interface IAudited
	{
		[Required]
		[StringLength(50)]
		string ModifiedWith { get; set; }

		[Required]
		[StringLength(50)]
		string ModifiedBy { get; set; }
	}
}
