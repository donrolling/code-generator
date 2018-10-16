using System.Collections.Generic;

namespace Model.Application {
	public class Entity {
		public string Name { get; set; }

		private List<Property> _properties = new List<Property>();
		public List<Property> Properties {
			get { return _properties; }
			set { _properties = value; }
		}
	}
}
