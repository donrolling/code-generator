using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model {
	public class PropertyTypeDescription {
		public bool Nullable { get; set; }
		public int Length { get; set; }
		public object DefaultValue { get; set; }
	}
}
