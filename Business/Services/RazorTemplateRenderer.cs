using System.Linq;
using System.Text.RegularExpressions;
using Business.Common;
using Business.Conversion;
using Model.Application;
using RazorEngine;
using System;

namespace Business.Services {
	public class RazorTemplateRenderer {
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
				var newEx = new Exception("Error in Template: " + templateName + "\r\n" + ex.Message);
				throw newEx;
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
	}
}