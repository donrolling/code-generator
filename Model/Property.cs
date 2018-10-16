using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Smo;

namespace Model {
	public class Property {
		public string Name { get; set; }
		public bool PrimaryKey { get; set; }
		public SqlDataType SqlDataType { get; set; }
		public bool Nullable { get; set; }
		public object DefaultValue { get; set; }
		public int Length { get; set; }
	}
}
