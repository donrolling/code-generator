using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Smo;

namespace Model.Application {
	public class PropertyViewModel {
		public PropertyViewModel() {
			Id = new Guid();
			Association = new List<KeyValuePair<Guid, Guid>>();
		}

		public Guid Id { get; set; }

		public Name Name { get; set; }
		public bool PrimaryKey { get; set; }
		public SqlDataType SqlDataType { get; set; }
		public string DataType { get; set; }
		public bool Nullable { get; set; }
		public object DefaultValue { get; set; }
		public int Length { get; set; }

		/// <summary>
		/// the key is the entity name and the value is the property name
		/// </summary>
		public List<KeyValuePair<Guid, Guid>> Association { get; set; }
	}
}
