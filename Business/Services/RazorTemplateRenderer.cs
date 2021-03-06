﻿using Business.Common;
using Business.Conversion;
using Model.Application;
using RazorEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Business.Services {
	public class RazorTemplateRenderer {

		public void Cleanup() {
			var tempPath = Path.GetTempPath();
			var missedDelete = new List<DirectoryInfo>();
			foreach (var directory in Directory.GetDirectories(tempPath, "RazorEngine_*")) {
				var dir = new DirectoryInfo(directory);
				try {
					cleanDir(dir);
				} catch {
					missedDelete.Add(dir);
				}
			}
			foreach (var missed in missedDelete) {
				try {
					cleanDir(missed);
				} catch {}
			}
		}

		//todo: this should be a Test helper method, not part of the service
		public T GetEntity<T>(string fileName, string subDirectory) where T : class {
			var json = FileUtility.ReadTextFile<RazorTemplateRenderer>(fileName, subDirectory);
			var entity = JsonNetConversion.Deserialize<T>(json);
			return entity;
		}

		//todo: this should be a Test helper method, not part of the service
		public Template GetTemplate(string templateName, TemplateConfiguration templateConfiguration) {
			var template = templateConfiguration.Templates.Where(a => a.Name == templateName).FirstOrDefault();
			template.TemplateText = FileUtility.ReadTextFile(template.Location);
			return template;
		}

		public string ParseTemplate(string template, string templateName, EntityViewModel entity) {
			if (string.IsNullOrEmpty(template)) {
				return string.Empty;
			}
			var cache_name = "CacheKey_" + templateName;
			try {
				var result = Engine.Razor.RunCompile(template, cache_name, typeof(EntityViewModel), entity);
				result = Regex.Replace(result, @"(\<s\>)|(\<\/s\>)", "");
				return result;
			} catch (Exception ex) {
				return $"Error in Template: { templateName }\r\n{ ex.Message }";
			}
		}

		private void cleanDir(DirectoryInfo folder) {
			var files = folder.GetFiles();
			foreach (var file in files) {
				file.Delete();
			}
			foreach (var subFolder in folder.GetDirectories()) {
				cleanDir(subFolder);
			}
			folder.Delete();
		}
	}
}