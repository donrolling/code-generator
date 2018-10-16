using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Business.Common {
	public static class StringConversion {
		private const string pattern = @"(?<=\w)(?=[A-Z])";
		
		 public static string Convert(string phrase, StringCase stringCase) {
			switch (stringCase) {
				case StringCase.PascalCase:
					return toPascalCase(phrase);
				case StringCase.LowerCamelCase:
					return toCamelCase(phrase);
				default:
					break;
			}
			return phrase;
		}
		
		// Convert the string to Pascal case.
		private static string toPascalCase(string s) {
			return s.Substring(0, 1).ToUpper() + s.Substring(1);
		}

		// Convert the string to camel case.
		private static string toCamelCase(string s) {
			return s.Substring(0, 1).ToLower() + s.Substring(1);
		}
	}
}