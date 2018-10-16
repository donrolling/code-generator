using System;

namespace Business.Common.DataTables.Interfaces {
	public interface IDataTable {
		Array aaData { get; }
		int iTotalDisplayRecords { get; }
		int iTotalRecords { get; }
	}
}
