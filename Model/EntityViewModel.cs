using System;
using System.Collections.Generic;

namespace Model {
	public class EntityViewModel {
		public EntityViewModel() {
			IsValid = true;
			Id = new Guid();
		}

		public Guid Id { get; set; }

		public string Name { get; set; }
		public string NameWithSpaces { get; set; }
		public string NameLowerCamelCase { get; set; }
		public string OutputFilename { get; set; }
		public string Namespace { get; set; }
		public string Schema { get; set; }
		public string KeyName { get; set; }
		public string KeyNameLowerCamelCase { get; set; }
		public string KeyDataType { get; set; }	
		public List<string> Imports { get; set; }
		public bool IsValid { get; set; }

		private List<PropertyViewModel> _properties = new List<PropertyViewModel>();
		public List<PropertyViewModel> Properties {
			get { return _properties; }
			set { _properties = value; }
		}
	}
}