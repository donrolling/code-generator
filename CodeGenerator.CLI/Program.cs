using Business.Common;
using Business.Conversion;
using Model.Application;
using System;
using System.Threading;

namespace CodeGenerator.CLI {
	public class Program {
		private static void Main(string[] args) {
			Console.WriteLine("*************************************************");
			Console.WriteLine("Entity Generator v2");
			Console.WriteLine($"Please use config.json to configure the settings.");
			Console.WriteLine("*************************************************");
			var config = readConfig();
			if (config == null) {
				Console.WriteLine("Config was null. Sorry and goodbye.");
				Console.ReadLine();
			} else {
				try {
					Console.WriteLine("Generating files. This may take a moment. Please wait for a response.");
					var entityGenerator = new EntityGenerator(config);
					var result = entityGenerator.Generate();
					if (result.Failure) {
						Console.WriteLine($"There was an error: { result.Message }");
						Console.WriteLine("Goodbye");
						Console.ReadLine();
					}
					Console.WriteLine("Goodbye");
					Console.ReadLine();
				} catch (Exception ex) {
					Console.WriteLine($"There was an error: { ex.Message }");
					Console.WriteLine("Goodbye");
					Console.ReadLine();
				}
			}
		}

		private static TemplateConfiguration readConfig() {
			Console.WriteLine("Reading config.json file.");
			var configurationJson = string.Empty;
			try {
				configurationJson = FileUtility.ReadTextFile<Program>("config.json", "");
			} catch {
				Console.WriteLine("config.json file not found.");
			}
			return JsonNetConversion.Deserialize<TemplateConfiguration>(configurationJson);
		}
	}
}