using System;

namespace Business.Common.DatesAndTimes {
	public static class TimeHelper {
		private const int _milliSecondsInAMinute = 60000;
		private const int _milliSecondsInASecond = 1000;
		private const int _secondsInAMinute = 60;

		public static int RoundTimeToNearestInterval(int runIntervalInMinutes, DateTime currentTime) {
			var milliSecondsToDelay = 0;
			var currentMinute = currentTime.Minute;
			if (currentMinute != runIntervalInMinutes) {
				var diff = 0;
				if (currentMinute < runIntervalInMinutes) {
					diff = runIntervalInMinutes - currentMinute;
				} else {
					diff = runIntervalInMinutes - (currentMinute % runIntervalInMinutes);
				}
				milliSecondsToDelay = diff * _milliSecondsInAMinute;
			}
			var secondsToDelay = _secondsInAMinute - currentTime.Second;
			milliSecondsToDelay += secondsToDelay * _milliSecondsInASecond;
			return milliSecondsToDelay;
		}

		public static bool Between(DateTime input, DateTime date1, DateTime date2) {
			return (input > date1 && input < date2);
		}

		public static DateTime UtcToLocal(DateTime timestampUTC, string timeZone) {
			TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
			return TimeZoneInfo.ConvertTimeFromUtc(timestampUTC, localTimeZone);
		}

		public static DateTime UnixTimeStampToDateTime(double unixTimeStamp) {
			var result = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			result = result.AddSeconds(unixTimeStamp);
			return result;
		}

		public static int MinutesToSeconds(double minutes) {
			return (int)(minutes * 60);
		}

		public static int SecondsToMilliseconds(double seconds) {
			return (int)(seconds * 1000);
		}
	}
}