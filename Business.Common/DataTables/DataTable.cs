using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Web;
using Business.Common.DataTables.Interfaces;

namespace Business.Common.DataTables {
	public class DataTable : IDataTable {
		public string[] Properties { get; set; }
		public Array aaData { get; protected set; }
		public int iTotalDisplayRecords { get; protected set; }
		public int iTotalRecords { get; protected set; }

		private PropertyInfo[] propertyInfo;
		private PropertyInfo[] GetPropertyInfo(object obj) {
			if (propertyInfo != null) { 
				return propertyInfo;
			}
			Type type = obj.GetType();
			propertyInfo = type.GetProperties();
			return propertyInfo;
		}

		public DataTable() { }

		public DataTable(IEnumerable data, int totalDisplayRecords, int totalRecords) {
			aaData = (from d in data.OfType<object>() select GetRow(d)).ToArray();
			iTotalDisplayRecords = totalDisplayRecords;
			iTotalRecords = totalRecords;
		}

		protected virtual string[] GetRow(object obj) {
			return (
				from p in GetPropertyInfo(obj)
					let value = p.GetValue(obj, null)
					select value != null ? value.ToString() : string.Empty
				).ToArray();
		}
		
		protected static IDictionary<string, string> GetSortNames(DataTable dt, HttpContextBase context) {
			var orders = PagingHelper.GetSortColumns(context);
			var dict = new Dictionary<string, string>();
			foreach (var o in orders) {
				var sortName = GetSortName(dt, o.Key);
				if (!string.IsNullOrEmpty(sortName))
					dict.Add(sortName, o.Value);
			}
			return dict;
		}

		protected static string GetSortName(DataTable dt, int orderIndex) {
			if (dt.Properties == null || dt.Properties.Length == 0){
				return string.Empty;
			}
			return dt.Properties.GetValue(orderIndex).ToString();
		}
	}

	/// <summary>
	/// A jquery datatable helper.  Extend this abstract class and supply the columns/rows you need in the FormatData method.
	/// Remember column order is important for sorting, if you need to supply attribute custom column order override the GetSortName method.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class DataTable<T> : DataTable where T : class {
		protected int pageStart;
		protected int pageSize;
		protected IDictionary<string, string> sorts;

		public DataTable(HttpContextBase context, IEnumerable<T> source, int totalRecords, params string[] properties) {
			prepareData(context, source, totalRecords, properties);
		}

		private void prepareData(HttpContextBase context, IEnumerable<T> source, int totalRecords, params string[] properties){
			iTotalDisplayRecords = totalRecords; //idk why this value needs to be set to the same as totalRecords, but that is what works. Should go back and look at this later. - DR
			iTotalRecords = totalRecords;

			//var pagingTuple = DataTable.GetPageStartAndSize(context);

			Properties = properties;
			sorts = GetSortNames(this, context);
			if (source != null){
				SetData(source);
			}		
		}
		
		protected virtual Array FormatData(IEnumerable<T> source) {
			var selectString = String.Join(",", Properties);
			return source.AsQueryable().Select("new(" + selectString + ")").Cast<object>().ToArray();
		}
		protected void SetData(IEnumerable<T> source) {
			var data = source.AsQueryable();
			foreach (var s in sorts) {
				source = data.Sort(s.Key, s.Value);
			}
			aaData = (from s in FormatData(source.ToList()).OfType<object>() select GetRow(s)).ToArray();
		}
	}
}
