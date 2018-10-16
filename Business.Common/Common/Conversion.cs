using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Business.Common {
	public class Conversion {
		public static string ArrayToCsv<T>(T[] list) {
			string retval = list.Aggregate(string.Empty, (a, next) => string.Concat(a, ",", next.ToString()));
			return retval.Trim(',');
		}
		public static List<string> CsvToListOfString(string csv) {
			var list = new List<string>();
			try {
				list = new List<string>(csv.Split(Convert.ToChar(",")));
			} catch {
			}
			return list;
		}
		public static string GetQueryStringValue(HttpRequest request, string key) {
			if (request.QueryString[key] != null) {
				return request.QueryString[key];
			}
			return string.Empty;
		}
		public static string Humanize(bool b) {
			return b ? "Yes" : "No";
		}
		public static string ListOfMailAddressesCsv(MailAddressCollection listOfAddresses) {
			var retVal = new StringBuilder();
			int i = 0;
			foreach (MailAddress address in listOfAddresses) {
				if (i > 0) {
					retVal.Append(",");
				}
				retVal.Append(address.Address);
				i += 1;
			}
			return retVal.ToString();
		}
		public static string ListOfStringToCsv(List<string> list) {
			var retVal = new StringBuilder();
			int i = 0;
			foreach (string s in list) {
				if (i > 0) {
					retVal.Append(",");
				}
				retVal.Append(s);
				i += 1;
			}
			return retVal.ToString();
		}
		public static string ListToCsv<T>(List<T> list) {
			string retval = list.Aggregate(string.Empty, (a, next) => string.Concat(a, ",", next.ToString()));
			return retval.Trim(',');
		}
		public static string MVCUrlDecode(string s) {
			s = s.Replace("!", "%");
			s = HttpUtility.UrlDecode(s);
			return s;
		}
		public static string MVCUrlEncode(string s) {
			s = HttpUtility.UrlEncode(s);
			s = s.Replace("%", "!");
			return s;
		}
		public static List<string> OneDimensionalArrayToListOfString(string[] oneDimensionalArray) {
			var list = new List<string>();
			try {
				list = new List<string>(oneDimensionalArray);
			} catch {
			}
			return list;
		}
		private static object ChangeType(object value, Type conversionType) {
			if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>))) {
				if (value == null) {
					return null;
				}
				var nullableConverter = new NullableConverter(conversionType);
				conversionType = nullableConverter.UnderlyingType;
			}
			return Convert.ChangeType(value, conversionType);
		}
	}
}