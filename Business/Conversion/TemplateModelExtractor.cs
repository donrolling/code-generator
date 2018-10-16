using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Business.Conversion {
	public static class TemplateModelExtractor {
		private static string _innerPlaceholderRegex = @"[A-Za-z0-9]*";

		public static List<string> Extract(string template, string placeholderBeginChars, string placeholderEndChars) {
			var pattern = GetPattern(placeholderBeginChars, placeholderEndChars);
			return Extract(template, pattern);
		}

		public static List<string> Extract(string template, string pattern) {
			var result = new List<string>();
			foreach (Match m in Regex.Matches(template, pattern)) {
				var g = m.Groups[0];
				if (g != null) {
					result.Add(g.Value);
				}
			}
			return result.Select(a => a).Distinct().ToList();
		}

		public static string GetPattern(string placeholderBeginChars, string placeholderEndChars) {
			return string.Concat(placeholderBeginChars, _innerPlaceholderRegex, placeholderEndChars);
		}
	}
}
