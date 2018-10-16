using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Business.Common.DataTables {
	public static class PagingHelper {
		public static PageInfo GetPageInfo(HttpContextBase context, SearchFilter filter, string orderBy) {
			var pageInfo = new PageInfo();
			pageInfo.Filters.Add(filter);
			setPageStartAndSize(context, pageInfo);
			pageInfo.OrderBy = orderBy;
			return pageInfo;
		}

		public static PageInfo GetPageInfo(HttpContextBase context, SearchFilter filter, List<string> orderBy) {
			var pageInfo = new PageInfo();
			pageInfo.Filters.Add(filter);
			setPageStartAndSize(context, pageInfo);
			if (orderBy != null && orderBy.Any()) {
				pageInfo.OrderBy = getOrderBy(context, orderBy);
			}
			return pageInfo;
		}

		public static PageInfo GetPageInfo(string orderBy) {
			return GetPageInfo(0, 0, orderBy);
		}

		public static PageInfo GetPageInfo(int pageStart, int pageSize, string orderBy) {
			var pageInfo = new PageInfo { 
				OrderBy = orderBy,
				PageStart = pageStart,
				PageSize = pageSize == 0 ? int.MaxValue : pageSize
			};
			return pageInfo;
		}

		public static PageInfo GetPageInfo(int pageStart, int pageSize, string orderBy, SearchFilter searchFilter) {
			var pageInfo = new PageInfo { 
				OrderBy = orderBy,
				PageStart = pageStart,
				PageSize = pageSize == 0 ? int.MaxValue : pageSize
			};
			pageInfo.Filters.Add(searchFilter);
			return pageInfo;
		}

		public static IDictionary<int, string> GetSortColumns(HttpContextBase context) {
			int i = 0;
			var dict = new Dictionary<int, string>();
			while (i >= 0) {
				var order = context.Request.Form["iSortCol_" + i.ToString()];
				if (!string.IsNullOrEmpty(order)) {
					var sortDir = context.Request.Form["iSortDir_" + i.ToString()];
					sortDir = sortDir ?? context.Request.Form["sSortDir_" + i.ToString()];
					dict.Add(int.Parse(order), sortDir);
					i++;
				} else{
					i = -1;
				}
			}
			return dict;
		}

		private static void setPageStartAndSize(HttpContextBase context, PageInfo pageInfo){
			int pageSize = 0;
			int pageStart = 0;
			if (int.TryParse(context.Request.Form["iDisplayStart"], out pageStart)){
				pageStart = int.Parse(context.Request.Form["iDisplayStart"]);
			}
			if (int.TryParse(context.Request.Form["iDisplayLength"], out pageSize)){
				pageSize = int.Parse(context.Request.Form["iDisplayLength"]);
			}
			pageInfo.PageStart = pageStart;
			pageInfo.PageSize = pageSize;
		}

		private static string getOrderBy(HttpContextBase context, List<string> columnNames){
			var sortColumns = GetSortColumns(context);
			var result = new StringBuilder();
			var i = 0;
			foreach (var sortColumn in sortColumns) {
				var sort = string.Concat(columnNames[sortColumn.Key], " ", sortColumn.Value);
				var sortEntry = i > 0 ? string.Concat(", ", sort) : sort;
				result.Append(sortEntry);
				i++;
			}
			return result.ToString();
		}
	}
}