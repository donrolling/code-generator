using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models.Entities{
	public class DataType {
		[Key]
		[Required]
		public long Id { get; set; }
		[Required]
		[StringLength(150, ErrorMessage = "Name cannot be longer than 150 characters.")]
		public string Name { get; set; }
		[Required]
		public int DotNetEnumValue { get; set; }
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