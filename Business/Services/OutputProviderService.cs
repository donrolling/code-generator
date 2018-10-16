using Business.Common;
using Business.Conversion;
using Model.Application;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Business.Services {
	public class OutputProviderService {
		private RazorTemplateRenderer RazorTemplateRenderer = new RazorTemplateRenderer();
		private List<string> ignoreDeleteExtensions = new List<string> { ".bat", ".ps1" };

		//public byte[] Output(EntityViewModel entity, List<Template> templates, List<string> selectedTemplates, string pathToTemplates) {
		//	var dict = new Dictionary<string, string>();
		//	var runOnceTemplates = new List<string>();
		//	traverseTemplatesAndEntities(dict, runOnceTemplates, new List<EntityViewModel> { entity }, templates, selectedTemplates, pathToTemplates);
		//	return zip(dict);
		//}

		//public byte[] Output(List<EntityViewModel> entities, List<Template> templates, List<string> selectedTemplates, string pathToTemplates) {
		//	var dict = new Dictionary<string, string>();
		//	var runOnceTemplates = new List<string>();
		//	traverseTemplatesAndEntities(dict, runOnceTemplates, entities, templates, selectedTemplates, pathToTemplates);
		//	return zip(dict);
		//}

		//public void OutputToFile(List<EntityViewModel> entities, List<Template> templates, List<string> selectedTemplates, string pathToTemplates, string outputFilename) {
		//	var dict = new Dictionary<string, string>();
		//	var runOnceTemplates = new List<string>();
		//	traverseTemplatesAndEntities(dict, runOnceTemplates, entities, templates, selectedTemplates, pathToTemplates);
		//	zip(dict, outputFilename);
		//}

		public void OutputToFiles(List<EntityViewModel> entities, List<Template> templates, List<string> selectedTemplates, string pathToTemplates, string outputPath) {
			cleanDir(new DirectoryInfo(outputPath));
			var dict = new Dictionary<string, string>();
			var runOnceTemplates = new List<string>();
			traverseTemplatesAndEntities(dict, runOnceTemplates, entities, templates, selectedTemplates, pathToTemplates);
			write(dict, outputPath);
		}

		private void cleanDir(DirectoryInfo folder) {
			var files = folder.GetFiles();
			var ignoredFiles = files.Where(a => ignoreDeleteExtensions.Contains(a.Extension));
			var filesToDelete = files.Where(a => !ignoreDeleteExtensions.Contains(a.Extension));
			foreach (var file in filesToDelete) {
				file.Delete();
			}
			foreach (var subFolder in folder.GetDirectories()) {
				cleanDir(subFolder);
			}
			if (ignoredFiles == null || !ignoredFiles.Any()) {
				try {
					folder.Delete(true);
				} catch (System.Exception) {

				}
			}
		}

		private void traverseTemplatesAndEntities(Dictionary<string, string> dict, List<string> runOnceTemplates, List<EntityViewModel> entities, List<Template> templates, List<string> selectedTemplates, string pathToTemplates) {
			//setup templates
			setupTemplates(templates, selectedTemplates, runOnceTemplates, pathToTemplates);

			//run the standard templates for each entity
			foreach (var template in templates.Where(a => selectedTemplates.Contains(a.Name) && !runOnceTemplates.Contains(a.Name) && a.Type != "Setup")) {
				foreach (var entity in entities) {
					runEntityThroughTemplate(dict, template, entity);
				}
			}

			//run the run once templates
			var runOnceEntity = entities.First();
			foreach (var template in templates.Where(a => selectedTemplates.Contains(a.Name) && runOnceTemplates.Contains(a.Name) && a.Type != "Setup")) {
				EntityService.PrepEntityForTemplate(runOnceEntity, template);
				//template needs an entity even if it has no properties, this is because the imports are stored in the entity object. this could probably use a refactor, but is not a big problem.
				runTemplate(dict, template, runOnceEntity);
			}

			//run the setup templates
			var setupEntity = new EntityViewModel();
			foreach (var entity in entities) {
				setupEntity.Properties.Add(new PropertyViewModel { Name = entity.Name });
			}
			foreach (var template in templates.Where(a => selectedTemplates.Contains(a.Name) && runOnceTemplates.Contains(a.Name) && a.Type == "Setup")) {
				setupEntity.OutputFilename = string.Concat(template.RelativeOutputPath, "\\", template.OutputFilename);
				setupEntity.Namespace = template.Namespace;
				//EntityService.PrepEntityForTemplate(setupEntity, template);
				//template needs an entity even if it has no properties, this is because the imports are stored in the entity object. this could probably use a refactor, but is not a big problem.
				runTemplate(dict, template, setupEntity);
			}
		}

		//public byte[] OutputEntitiesAsBytes(List<EntityViewModel> entities) {
		//	var dict = new Dictionary<string, string>();
		//	prepareEntities(dict, entities);
		//	return zip(dict);
		//}

		//public void OutputEntitiesToFile(List<EntityViewModel> entities, string outputFilename) {
		//	var dict = new Dictionary<string, string>();
		//	prepareEntities(dict, entities);
		//	zip(dict, outputFilename);
		//}

		private void prepareEntities(Dictionary<string, string> dict, List<EntityViewModel> entities) {
			foreach (var entity in entities) {
				var jsonEntity = JsonNetConversion.Serialize(entity);
				dict.Add(entity.Name + ".json", jsonEntity);
			}
		}

		private void runEntityThroughTemplate(Dictionary<string, string> dict, Template template, EntityViewModel entity) {
			EntityService.PrepEntityForTemplate(entity, template);
			if (dict.ContainsKey(entity.OutputFilename)) {
				return;
			}
			runTemplate(dict, template, entity);
		}

		private void runTemplate(Dictionary<string, string> dict, Template template, EntityViewModel entity) {
			var renderedText = this.RazorTemplateRenderer.ParseTemplate(template.TemplateText, template.Name, entity);
			dict.Add(entity.OutputFilename, renderedText);
		}

		private void write(Dictionary<string, string> dict, string outputPath) {
			foreach (var item in dict) {
				FileUtility.WriteFile<OutputProviderService>(item.Key, outputPath, item.Value);
			}
		}		
		
		private void setupTemplates(List<Template> templates, List<string> selectedTemplates, List<string> runOnceTemplates, string pathToTemplates) {
			foreach (var template in templates.Where(a => selectedTemplates.Contains(a.Name))) {
				var path = string.Concat(pathToTemplates, template.Location);
				var templateText = File.ReadAllText(path);
				template.TemplateText = templateText;
				if (template.RunOnce) {
					if (!runOnceTemplates.Contains(template.Name)) {
						runOnceTemplates.Add(template.Name);
					}
				}
			}
		}
	}
}