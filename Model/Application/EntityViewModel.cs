using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Application {
	public class EntityViewModel {
		public List<string> IgnoreProperties = new List<string> { "IsActive", "CreatedById", "CreatedDate", "UpdatedById", "UpdatedDate" };

		public string SQLPKSignature { get; set; }
		public string SQLPKWhere { get; set; }
		public string SQLSet { get; set; }
		public string SQLInsertSignature { get; set; }
		public string SQLUpdateSignature { get; set; }
		public string CSharpKeySignature { get; set; }
		public string CSharpKeyList { get; set; }
		public string CSharpKeyTypeList { get; set; }

		public Guid Id { get; set; } = Guid.NewGuid();
		public Name Name { get; set; } = new Name();
		public List<Key> Keys { get; set; } = new List<Key>();
		public string OutputFilename { get; set; }
		public string Namespace { get; set; }
		public string Schema { get; set; }
		public bool IsValid { get; set; } = true;

		public List<string> Imports { get; set; } = new List<string>();
		public List<PropertyViewModel> Properties { get; set; } = new List<PropertyViewModel>();
		public List<PropertyViewModel> NonAuditProperties {
			get {
				return this.Properties.Where(a => !this.IgnoreProperties.Contains(a.Name.Value)).ToList();
			}
		}
		public string CSharpInsertCallSignature { get; set; }
		public string CSharpUpdateCallSignature { get; set; }
		public string CSharpKeyCallSignature { get; set; }
		public object SQLPKCallSignature { get; set; }
	}
}