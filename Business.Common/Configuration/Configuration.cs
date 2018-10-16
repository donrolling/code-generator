using System;
using System.ComponentModel;
using System.Configuration;

namespace Business.Common.Configuration {
	public static class Config { //todo: decouple this from system.configuration?

		/// <summary>
		/// Retrieves attribute value from the string input for attribute given key. Values can be found in either the machine config or the web config.
		/// Caution! Null values retrieved from this method will result in Exceptions.
		/// </summary>
		/// <param name="applicationSetting">A string that represents the name of the requested key.</param>
		/// <returns>String value of the requested key.</returns>
		public static string Setting(string key, bool emptyOk = false) {
			string value = ConfigurationManager.AppSettings[key];
			if (string.IsNullOrEmpty(value)) {
				if (!emptyOk) {
					throw new Exception("Value was empty for this key: " + key);
				}
			}
			return value;
		}
		/// <summary>
		/// Retrieves attribute typed value from the given key. Values can be found in either the machine config or the web config.
		/// Caution! Null values retrieved from this method will result in Exceptions.
		/// </summary>
		/// <param name="applicationSetting">A string that represents the name of the requested key.</param>
		/// <returns>A typed value of the requested key.</returns>
		public static T Setting<T>(string key) {
			object value = ConfigurationManager.AppSettings[key];
			T item = Type<T>(value);
			return item;
		}
		/// <summary>
		/// Intended to be used to deal with generic typing from inside attribute generic method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static T Type<T>(object value, bool emptyOk = false) {
			T retval = default(T);
			if (value != null) {
				var converter = TypeDescriptor.GetConverter(typeof(T));
				if (converter != null) {
					return (T)converter.ConvertFromString(value.ToString());
				}
			} else {
				if (!emptyOk) {
					throw new Exception("Configuration value is null");
				}
			}
			return retval;
		}

		/// <summary>
		/// The connection string.
		/// </summary>
		/// <param name="connectionStringKey">
		/// The connection string key.
		/// </param>
		/// <returns>
		/// The <see cref="string"/>.
		/// </returns>
		/// <exception cref="Exception">
		/// </exception>
		public static string ConnectionString(string connectionStringKey) {
			string value = ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString; 
			if (string.IsNullOrEmpty(value)) {
				throw new Exception("Value was empty for this key: " + connectionStringKey);
			}
			return value;
		}
	}
}