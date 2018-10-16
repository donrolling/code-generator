using System.Collections.Generic;

namespace Data.Presentation {
	public class PresentationList<T> : IPresentable<T> where T : class {
		public IEnumerable<T> Collection { get; set; }
		public int RecordsDiplayed { get; set; }
		public int TotalRecords { get; set; }

		public PresentationList(IEnumerable<T> collection, int recordsDiplayed, int totalRecords) {
			this.Collection = collection == null ? new List<T>() : collection;
			this.RecordsDiplayed = recordsDiplayed;
			this.TotalRecords = totalRecords;
		}
	}
}