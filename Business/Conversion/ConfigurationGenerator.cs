using System.Collections.Generic;
using System.IO;
using System.Linq;
using Business.Common;
using Business.Common;
using Model.Application;

namespace Business.Conversion {
	public class ConfigurationGenerator {
		public static List<string> GetFiles_FromFullPath(string path, string fileExtension) {
			if (!fileExtension.Contains('.')) { fileExtension = "." + fileExtension; }
			var entries = Directory.GetFileSystemEntries(path, "*" + fileExtension, SearchOption.AllDirectories);
			if (entries == null || entries.Length == 0) {
				return new List<string>();
			}
			return entries.ToArray().ToList();
		}

		public static List<string> GetFiles_FromProjectRelativePath(string subdirectory, string fileExtension) {
			var path = FileUtility.GetFullPath<ConfigurationGenerator>(subdirectory);
			if (!fileExtension.Contains('.')) { fileExtension = "." + fileExtension; }
			var entries = Directory.GetFileSystemEntries(path, "*" + fileExtension, SearchOption.AllDirectories);
			if (entries == null || entries.Length == 0) {
				return new List<string>();
			}
			return entries.ToArray().ToList();
		}

		public static TemplateConfiguration GetConfiguration_FromFiles(List<string> filePaths) {
			var templateConfiguration = new TemplateConfiguration();
			var result = new List<Template>();
			foreach (var filePath in filePaths) {
				var template = getTemplateFromFilePath(filePath);
				result.Add(template);
			}
			templateConfiguration.Templates = result;
			return templateConfiguration;
		}

		private static Template getTemplateFromFilePath(string filePath) {
			var filename = Path.GetFileNameWithoutExtension(filePath);
			var isBaseClass = filename.Contains("Base");
			var parentFolder = new DirectoryInfo(filePath).Parent.Name;
			var outputFilename = getOutputFilename(parentFolder, filename, isBaseClass);
			var location = getLocation(filePath);
			var relativeOutputPath = parentFolder == "Auxilary" ? "" : parentFolder;
			var language = getLanguage(parentFolder, isBaseClass);
			var template = new Template { 
				Name = filename,
				Type = parentFolder,
				Namespace = parentFolder,
				OutputFilename = outputFilename,
				RelativeOutputPath = relativeOutputPath,
				Location = location,
				Language = language,
				RunOnce = isBaseClass,
				Symbols = getSymbols(filePath),
				Imports = getDefaultImports(parentFolder)
			};
			return template;
		}

		private static string getLanguage(string parentFolder, bool isBaseClass) {
			if (isBaseClass) {
				return "csharp";
			}
			switch (parentFolder) {
				case "Procedure":
				case "TableValuedFunction":
					return "sql";
				case "Model":
				case "Repository":
				case "Service":
					return "csharp";
				case "Auxilary":
					return "";
				default:
					return "csharp";
			}
		}

		private static List<string> getSymbols(string filePath) {
			var template = FileUtility.ReadTextFile(filePath);
			var templateProperties = TemplateModelExtractor.Extract(template, @"\[\[", @"\]\]");
			if (templateProperties != null) {
				return templateProperties;
			}
			return new List<string>();
		}

		private static List<string> getDefaultImports(string parentFolder) {
			switch (parentFolder) {
				case "Procedure":
				case "TableValuedFunction":
				case "Auxilary":
					return new List<string>();
				default:
					return new List<string> { "System", "System.Collections.Generic" };
			}
		}

		private static string getLocation(string filePath) {
			var path = FileUtility.GetFullPath<ConfigurationGenerator>("");
			var location = filePath.Replace(path, "");
			return location;
		}

		private static string getOutputFilename(string parentFolder, string filename, bool isBaseClass) {
			var ext = getExtensionFromParentFolder(parentFolder, isBaseClass);
			var isInterfaceTemplate = filename.Contains("Interface");
			if (isInterfaceTemplate) {
				if (isBaseClass) {
					return string.Concat("I", parentFolder, ext);
				} else {
					return string.Concat("I{0}", parentFolder, ext);
				}
			}
			var isServiceTemplate = filename.Contains("Service");
			if (isServiceTemplate) {
				return string.Concat("{0}Service", ext);
			}
			var isRepositoryTemplate = filename.Contains("Repository");
			if (isRepositoryTemplate) {
				return string.Concat("{0}Repository", ext);
			}
			if (parentFolder == "Procedure" || parentFolder == "TableValuedFunction") {
				return string.Concat("{0}_", filename, ext);
			}
			return string.Concat("{0}", ext);
		}

		private static string getExtensionFromParentFolder(string parentFolder, bool isBaseClass) {
			if (isBaseClass) {
				return ".cs";
			}
			switch (parentFolder) {
				case "Procedure":
				case "TableValuedFunction":
					return ".sql";
				case "Model":
				case "Repository":
				case "Service":
					return ".cs";
				case "Auxilary":
					return "";
				default:
					return ".txt";
			}
		}
	}
}
