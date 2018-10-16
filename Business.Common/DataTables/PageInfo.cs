using System.Collections.Generic;

namespace Business.Common.DataTables {
	public class PageInfo {
		public int PageSize { get; set; }
		public int PageStart { get; set; }
		public string OrderBy { get; set; }
		
		private List<SearchFilter> _filters = new List<SearchFilter>();
		public List<SearchFilter> Filters { get { return _filters;  } set { _filters = value; } }
	}
}