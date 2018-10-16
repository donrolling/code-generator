using System.Web.Mvc;
using Business.Common.DataTables.Interfaces;

namespace Business.Common.DataTables {
	public class DataSet : JsonResult {
		public DataSet(IDataTable dataTable) {
			base.Data = dataTable;
			base.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
		}
	}
}