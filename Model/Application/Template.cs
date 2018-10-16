using System.Collections.Generic;

namespace Model.Application {
	public class Template {
		public string Type { get; set; }
		public string Name { get; set; }
		public string Schema { get; set; }
		public string Namespace { get; set; }
		public string Location { get; set; }
		public string OutputFilename { get; set; }
		public string RelativeOutputPath { get; set; }
		public string Language { get; set; }
		public bool RunOnce { get; set; }
		public bool IsStub { get; set; }
		public List<string> Imports { get; set; }
		public List<string> Symbols { get; set; }
		public string TemplateText { get; set; }
	}
}
