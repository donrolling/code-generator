using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
	public class Entity {
		public string Name { get; set; }

		private List<Property> _properties = new List<Property>();
		public List<Property> Properties {
			get { return _properties; }
			set { _properties = value; }
		}
	}
}
