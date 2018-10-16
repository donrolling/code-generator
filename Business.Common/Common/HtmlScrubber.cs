using System.Text.RegularExpressions;

namespace Business.Common {
	public static class HtmlScrubber {
		public static string Scrub(string value) {
			var step1 = Regex.Replace(value, @"<[^>]+>|&nbsp;", "").Trim();
			var step2 = Regex.Replace(step1, @"\s{2,}", " ");
			return step2;
		}
	}
}