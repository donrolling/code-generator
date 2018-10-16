using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models.Entities {
	public class PropertyRelationship { 
		[Key]
		[Required]
		public long Property1Id { get; set; }
		[Required]
		public long Property2Id { get; set; }
		[Required]
		public string Name { get; set; }
		public string Type { get; set; }
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