using System;

namespace Business.Common.DatesAndTimes {
	public static class UTC_Formatter {
		private static string _defaultTimeZone = "en_us";

		public static DateTime GetLocalDateTimeFromUTC(DateTime dateTime) {
			var timeZoneInfo = getTimeZoneInfo();
			var theDate = dateTime == DateTime.MinValue ? DateTime.Now : dateTime;
			var dateKind = DateTime.SpecifyKind(theDate, DateTimeKind.Utc);
			var result = TimeZoneInfo.ConvertTimeFromUtc(dateKind, timeZoneInfo);
			return result;
		}

		public static DateTime GetLocalDateTimeFromUTC(DateTime dateTime, TimeZoneInfo timeZoneInfo) {
			if (timeZoneInfo == null){
				timeZoneInfo = getTimeZoneInfo();
			}
			var theDate = dateTime == DateTime.MinValue ? getNewDateTime() : dateTime;
			var dateKind = DateTime.SpecifyKind(theDate, DateTimeKind.Utc);
			var result = TimeZoneInfo.ConvertTimeFromUtc(dateKind, timeZoneInfo);
			return result;
		}

		private static DateTime getNewDateTime() {
			var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
			return now;
		}

		private static TimeZoneInfo getTimeZoneInfo(){
			var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(_defaultTimeZone);
			return timeZoneInfo;
		}
	}
}