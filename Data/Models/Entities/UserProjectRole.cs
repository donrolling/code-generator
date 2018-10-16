using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models.Entities {
	public class UserProjectRole { 
		[Key]
		[Required]
		public long UserId { get; set; }
		[Required]
		public long ProjectId { get; set; }
		[Required]
		public long RoleId { get; set; }
		[Required]
		public bool IsActive { get; set; }
		public long? CreatedById { get; set; }
		[Required]
		public DateTime CreatedDate { get; set; }
		public long? UpdatedById { get; set; }
		[Required]
		public DateTime UpdatedDate { get; set; }
		public const string DatabaseSchema = "dbo";
	}
}