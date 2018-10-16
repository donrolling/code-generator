using System.Collections.Generic;

namespace Data.Presentation {
	public interface IPresentable<T> where T : class {
		IEnumerable<T> Collection { get; set; }
		int RecordsDiplayed { get; set; }
		int TotalRecords { get; set; }
	}
}
