using Microsoft.SqlServer.Management.Smo;

namespace Model.Application {
	public class Property {
		public string Name { get; set; }
		public bool PrimaryKey { get; set; }
		public SqlDataType SqlDataType { get; set; }
		public bool Nullable { get; set; }
		public object DefaultValue { get; set; }
		public int Length { get; set; }
	}
}
