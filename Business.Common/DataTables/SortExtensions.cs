using System;
using System.Linq;
using System.Linq.Dynamic;

namespace Business.Common.DataTables {
	public static class SortExtensions {
		public static IQueryable<T> Sort<T>(this IQueryable<T> source, string sortColumn, string sortDirection) {
			Type type = typeof(T);
			var property = type.GetProperty(sortColumn);

			var isString = property == null ? false : property.PropertyType == typeof(string);

			var functionCall = !isString ? "" : ".ToUpper()";

			if (string.IsNullOrEmpty(sortColumn))
				return source;

			if (string.IsNullOrEmpty(sortDirection))
				return source.OrderBy(sortColumn + functionCall + " ASC");

			return source.OrderBy(sortColumn + functionCall + " " + sortDirection.ToUpper());
		}

		public static bool ContainsCaseInsensitive(this string source, string value) {
			int results = source.IndexOf(value, StringComparison.CurrentCultureIgnoreCase);
			return results != -1;
		}
	}
}