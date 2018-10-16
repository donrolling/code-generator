using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models.Entities {
	public class Template {
		[Key]
		[Required]
		public long Id { get; set; }
		[Required]
		public long ProjectId { get; set; }
		[Required]
		public long LanguageId { get; set; }
		public string Type { get; set; }
		public string Name { get; set; }
		public string Schema { get; set; }
		public string Namespace { get; set; }
		public string OutputFilename { get; set; }
		public string RelativeOutputPath { get; set; }
		public string Language { get; set; }
		public bool RunOnce { get; set; }
		public string Text { get; set; }
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