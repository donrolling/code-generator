using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models.Entities {
	public class TemplateImport {
		[Required]
		public long Id { get; set; }
		[Required]
		public long TemplateId { get; set; }
		[Required]
		public bool IsActive { get; set; }
		public long CreatedById { get; set; }
		[Required]
		public DateTime CreatedDate { get; set; }
		public long UpdatedById { get; set; }
		[Required]
		public DateTime UpdatedDate { get; set; }
	}
}