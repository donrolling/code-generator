using System.Collections.Generic;
using Newtonsoft.Json;

namespace Business.Conversion {
	public static class JsonNetConversion {
		private static JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings {
			Formatting = Formatting.Indented,
			Converters = new List<JsonConverter> {  }//new StringEnumConverter()
		};
		public static JsonSerializerSettings JsonSerializerSettings {
			get {
				return _jsonSerializerSettings;
			}
		}

		public static string Serialize(object entities) {
			var json = JsonConvert.SerializeObject(entities, JsonSerializerSettings);
			return json;
		}
		
		public static T Deserialize<T>(string json) where T : class {
			var entity = JsonConvert.DeserializeObject<T>(json, JsonSerializerSettings);
			return entity;
		}
		
		public static List<T> DeserializeList<T>(string json) where T : class {
			var entity = JsonConvert.DeserializeObject<List<T>>(json, JsonSerializerSettings);
			return entity;
		}
	}
}
