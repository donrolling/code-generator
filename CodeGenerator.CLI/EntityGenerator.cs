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
			//just correcting a simple potential configuration oversight
			if (Config.OutputToFile && !string.IsNullOrEmpty(Config.OutputFilename) && !Config.OutputFilename.Contains(".zip")) {
				Config.OutputFilename = Config.OutputFilename + ".zip";
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
			if (Config.OutputToFile) {
				outputProviderService.OutputEntitiesToFile(entities, Config.OutputFilename);
			} else {
				outputProviderService.OutputToFiles(entities, Config.Templates, selectedTemplates.Select(a => a.Name).ToList(), "\\Templates", Config.OutputDirectory);
			}
			foreach (var script in Config.PostProcessScripts) {
				Ronz.PowerShell.PowerShellUtils.RunScriptFile($"Scripts\\{ script }");
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

		public void GenerateEntitiesAndOutputSQLInserts(string connectionString, string outputFilename, int projectId) {
			if (!outputFilename.Contains(".sql")) {
				outputFilename = outputFilename + ".sql";
			}
			var entitiesGenerationResult = GenerateEntities(connectionString);
			if (entitiesGenerationResult.Failure) {
				return;
			}
			var entities = entitiesGenerationResult.Result;
			var sb = new StringBuilder();
			sb.AppendLine("declare @entityId bigint = 0");

			foreach (var entity in entities) {
				var entitySql = $"INSERT INTO [dbo].[Entity] ([ProjectId],[Name]) VALUES({ projectId }, '{ entity.Name }')";
				sb.AppendLine(entitySql);
				sb.AppendLine("set @entityId = SCOPE_IDENTITY()");
				foreach (var property in entity.Properties) {
					var isPrimaryKey = property.PrimaryKey ? "1" : "0";
					var isNullable = property.Nullable ? "1" : "0";
					var dataType = property.SqlDataType.ToString("D");
					var propertySql = $"Insert Into Property (EntityId, Name, IsPrimaryKey, DataTypeId, IsNullable, Length) values (@entityId, '{ property.Name }', { isPrimaryKey }, (select top 1 Id from DataType where DotNetEnumValue = { dataType }), { isNullable }, { property.Length })";
					sb.AppendLine(propertySql);
				}
				sb.AppendLine();
				sb.AppendLine("--****************************************************");
				sb.AppendLine();
			}

			FileUtility.WriteFile<EntityGenerator>(outputFilename, "", sb.ToString());
		}

		public void GenerateEntitiesAndSaveToFile(string connectionString, string outputFilename) {
			if (!outputFilename.Contains(".zip")) {
				outputFilename = outputFilename + ".zip";
			}
			var entitiesGenerationResult = GenerateEntities(connectionString);
			if (entitiesGenerationResult.Failure) {
				return;
			}
			var entities = entitiesGenerationResult.Result;
			var outputProviderService = new OutputProviderService();
			outputProviderService.OutputEntitiesToFile(entities, outputFilename);
		}

		public void GenerateOutput(List<EntityViewModel> entities, TemplateConfiguration templateConfiguration, string templateLocation, string outputFilename) {
			if (!outputFilename.Contains(".zip")) {
				outputFilename = outputFilename + ".zip";
			}

			var selectedTemplates = templateConfiguration.Templates.Select(a => a.Name).ToList();
			var outputLocation = FileUtility.GetFullPath<EntityGenerator>("Output");
			var outputProviderService = new OutputProviderService();
			outputProviderService.OutputToFile(entities, templateConfiguration.Templates, selectedTemplates, templateLocation, outputLocation + outputFilename);
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