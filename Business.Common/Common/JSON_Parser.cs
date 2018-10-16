using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Business.Common {
	public static class JSON_Parser {
		public static List<T> ParseList<T>(string dataNodeName, string json) where T : class {
			var jsonSerializerSettings = new JsonSerializerSettings();
			return JsonConvert.DeserializeObject<List<T>>(json, jsonSerializerSettings);
		}

		public static T ParseEntity<T>(string json) where T : class {
			T result = default(T);
			result = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings());
			return result;
		}

		private static JToken getTokens(JObject jsonElement, string dataNodeName, string json){
			var result = string.IsNullOrEmpty(dataNodeName) ? jsonElement.SelectToken("data") : jsonElement[dataNodeName].SelectToken("data");
			return result;
		}
	}
}