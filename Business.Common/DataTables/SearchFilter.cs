using System;

namespace Business.Common.DataTables {
	public class SearchFilter {
		public string Name { get; set; }
		public object Value { get; set; }
		public Type Type { get; set; }

		public SearchFilter(string name, object value) {
			Name = name;
			if (value == null) {
				Value = string.Empty;
				Type = typeof(string);
			} else { 
				Value = value;
				Type = value.GetType();
			}
		}
	}
}