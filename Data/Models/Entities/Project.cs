using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models.Entities {
	public class Project {
		[Key]
		[Required]
		public long Id { get; set; }
		[Required]
		[StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
		public string Name { get; set; }
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