using Business.Common;
using Business.Common.Responses;
using Business.Services;
using Model.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator.CLI {
	public class EntityGenerator {
		private TemplateConfiguration Config;

		public EntityGenerator(TemplateConfiguration config) {
			this.Config = config;
		}

		public Result Generate() {
			if (string.IsNullOrEmpty(Config.ConnectionString)) {
				return Result.Error("Connection String was empty.");
			}
			if (string.IsNullOrEmpty(Config.OutputDirectory)) {
				return Result.Error("Output Directory was empty.");
			}
			if (!Config.Templates.Any()) {
				return Result.Error("No templates to process.");
			}
			var selectedTemplates = this.filterTemplates(Config.Templates);
			if (!selectedTemplates.Any()) {
				return Result.Error("No templates selected to process.");
			}

			var entitiesGenerationResult = GenerateEntities(Config.ConnectionString);
			if (entitiesGenerationResult.Failure) {
				return entitiesGenerationResult;
			}
			if (!entitiesGenerationResult.Result.Any()) {
				return Result.Error("No entities to process.");
			}
			var entities = this.filterEntities(entitiesGenerationResult.Result);
			if (!entities.Any()) {
				return Result.Error("No entities to process.");
			}

			var outputProviderService = new OutputProviderService();
			var pathToTemplates = FileUtility.GetFullPath<EntityGenerator>("\\Templates");
			outputProviderService.OutputToFiles(entities, Config.Templates, selectedTemplates.Select(a => a.Name).ToList(), pathToTemplates, Config.OutputDirectory);
			foreach (var script in Config.PostProcessScripts) {
				var pathToScript = FileUtility.GetFullPath<EntityGenerator>($"\\Scripts\\{ script }");
				Ronz.PowerShell.PowerShellUtils.RunScriptFile(pathToScript);
			}
			return Result.Ok();
		}

		public Envelope<List<EntityViewModel>> GenerateEntities(string connectionString) {
			var databaseSchemaConverterService = new DatabaseSchemaConverterService(connectionString);
			var entities = new List<EntityViewModel>();
			try {
				entities = databaseSchemaConverterService.GetEntitiesFromDatabase();
			} catch (Exception ex) {
				return Envelope<List<EntityViewModel>>.Error($"Error generating enities: { ex.Message }");
			}
			return Envelope<List<EntityViewModel>>.Ok(entities);
		}

		private List<EntityViewModel> filterEntities(List<EntityViewModel> entities) {
			if (Config.IncludeTheseTablesOnly.Any()) {
				return (
					from e in entities
					join o in Config.IncludeTheseTablesOnly on e.Name.Value equals o
					select e
				).ToList();
			}

			if (Config.ExcludeTheseTables.Any()) {
				return (
						from e in entities
						where !Config.ExcludeTheseTables.Contains(e.Name.Value)
						select e
					).ToList();
			}

			return entities;
		}

		private List<Template> filterTemplates(List<Template> templates) {
			if (Config.IncludeTheseTemplatesOnly.Any()) {
				return (
					from t in templates
					join o in Config.IncludeTheseTemplatesOnly on t.Name equals o
					select t
				).ToList();
			}

			var tempTemplates = new List<Template>();
			if (!Config.ProcessTemplateStubs) {
				tempTemplates = templates.Where(a => a.IsStub == false).ToList();
			} else {
				tempTemplates = templates;
			}

			if (Config.ExcludeTheseTemplates.Any()) {
				var xs = (
						from t in templates
						where !Config.ExcludeTheseTemplates.Contains(t.Name)
						select t
					).ToList();
				if (xs.Any()) {
					tempTemplates.AddRange(xs);
				}
			}

			return tempTemplates;
		}
	}
}