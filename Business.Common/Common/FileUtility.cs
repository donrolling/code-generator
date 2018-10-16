using System.IO;

namespace Business.Common {
	public class FileUtility {
		/// <summary>
		/// Reads a text file in total.
		/// </summary>
		/// <typeparam name="T">A type contained in the source assembly from which you'd like to read the file.</typeparam>
		/// <param name="filename">Filename, duh</param>
		/// <param name="subDirectory">SubDirectory within the source assembly.</param>
		/// <returns></returns>
		public static string ReadTextFile<T>(string filename, string subDirectory) where T : class {
			var path = Path.Combine(GetBasePath<T>(), subDirectory, filename);
			return ReadTextFile(path);
		}

		public static string ReadTextFile(string filePath) {
			if (filePath.Contains(":\\")) {
				return File.ReadAllText(filePath);			
			}
			var path = string.Concat(GetBasePath<FileUtility>(), filePath);
			return File.ReadAllText(path);
		}

		public static void WriteFile(string path, string contents) {
			if (!Directory.Exists(path)) {
				var dirName = Path.GetDirectoryName(path);
				Directory.CreateDirectory(dirName);
			}
			File.WriteAllText(path, contents);
		}

		public static void WriteFile<T>(string filename, string subDirectory, string contents) where T : class {
			var path = GetFullPath_FromRelativePath<T>(filename, subDirectory);
			if (!Directory.Exists(path)) {
				var dirName = Path.GetDirectoryName(path);
				Directory.CreateDirectory(dirName);
			}
			File.WriteAllText(path, contents);
		}

		public static string GetFullPath_FromRelativePath<T>(string filename, string subDirectory) where T : class {
			var path = Path.Combine(GetBasePath<T>(), subDirectory, filename);
			return path;
		}

		public static string GetFullPath<T>(string subDirectory) where T : class { 
			var path = string.Concat(GetBasePath<T>(), subDirectory);
			return path;
		}

		public static string GetBasePath<T>() where T : class {
			var basePath = Path.GetDirectoryName(typeof(T).Assembly.Location);
			var pos = basePath.IndexOf("\\bin");
			if (pos >= 0) {
				basePath = basePath.Substring(0, pos);
			}
			return basePath;
		}
	}
}