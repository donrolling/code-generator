using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.Application {
	public class TemplateConfiguration {
		[Required]
		public string ConnectionString { get; set; }
		public List<string> ExcludeTheseTables { get; set; } = new List<string>();
		public List<string> ExcludeTheseTemplates { get; set; } = new List<string>();
		public List<string> IncludeTheseTablesOnly { get; set; } = new List<string>();
		public List<string> IncludeTheseTemplatesOnly { get; set; } = new List<string>();
		[Required]
		public string OutputDirectory { get; set; }
		public string OutputFilename { get; set; }
		public bool OutputToFile { get; set; }
		public List<string> PostProcessScripts { get; set; } = new List<string>();
		public bool ProcessTemplateStubs { get; set; } = true;
		public List<Template> Templates { get; set; } = new List<Template>();
	}
}