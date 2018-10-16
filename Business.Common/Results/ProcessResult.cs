using System.Diagnostics;

namespace Business.Common.Results {
	public class ProcessResult {
		public bool Success { get; set; }
		public string ProcessName { get; set; }
		public string Message { get; set; }

		public ProcessResult(bool success, string message) {
			Success = success;
			Message = message;
			var st = new StackTrace();
			var sf = st.GetFrame(1);
			ProcessName = sf.GetMethod().Name;
		}
	}
}