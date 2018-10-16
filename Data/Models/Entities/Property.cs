using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models.Entities {
	public class Property {
		[Key]
		[Required]
		public long Id { get; set; }
		[Required]
		public long EntityId { get; set; }
		[Required]
		public long DataTypeId { get; set; }
		[Required]
		[StringLength(150, ErrorMessage = "Name cannot be longer than 150 characters.")]
		public string Name { get; set; }
		[Required]
		public bool IsPrimaryKey { get; set; }
		[Required]
		public bool IsNullable { get; set; }
		public int Length { get; set; }
		public string DefaultValue { get; set; }
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