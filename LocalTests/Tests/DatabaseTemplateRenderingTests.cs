using Business.Common;
using Business.Common.Configuration;
using Business.Conversion;
using Business.Services;
using CodeGenerator.CLI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using Test.Tests;

namespace LocalTests.Tests {
	[TestClass]
	public class DatabaseTemplateRenderingTests {
		public string ConnectionString { get; private set; }
		public DatabaseSchemaConverterService DatabaseSchemaConverterService { get; }
		public RazorTemplateRenderer RazorTemplateRenderer { get; set; }
		public TemplateConfiguration TemplateConfiguration { get; set; }
		private string _outputLocation = FileUtility.GetFullPath<EntityGenerator>("\\Output");
		private string _templateLocation = FileUtility.GetFullPath<EntityGenerator>("");
		private List<string> _templates = new List<string>();
		private List<EntityViewModel> _entities = new List<EntityViewModel>();
		private OutputProviderService OutputProviderService = new OutputProviderService();

		/// <summary>
		/// See App.Config for ConnectionString
		/// </summary>
		public DatabaseTemplateRenderingTests() {
			var configurationJson = FileUtility.ReadTextFile<DatabaseTemplateRenderingTests>("MainConfiguration.json", "Configurations");
			this.TemplateConfiguration = JsonNetConversion.Deserialize<TemplateConfiguration>(configurationJson);
			this.ConnectionString = Config.Setting("ConnectionString");
			this.RazorTemplateRenderer = new RazorTemplateRenderer();
			this.DatabaseSchemaConverterService = new DatabaseSchemaConverterService(this.ConnectionString);
			_entities = this.DatabaseSchemaConverterService.GetEntitiesFromDatabase().ToList();
			var processStubs = false;
			//stubs are for customizing, so sometimes we don't want to generate them because it will overwrite important stuff
			if (processStubs) {
				_templates = this.TemplateConfiguration.Templates.Select(a => a.Name).ToList();
			} else {
				_templates = this.TemplateConfiguration.Templates.Where(a => a.IsStub == false).Select(a => a.Name).ToList();
			}
		}

		[TestMethod]
		[TestCategory("Database")]
		public void Render_All_Templates_GivenDatabase() {
			var ignoreList = new List<string> { "User", "LocalProjectFilter" };
			var entities = _entities.Where(a => !ignoreList.Contains(a.Name.Value)).ToList();
			this.OutputProviderService.OutputToFiles(entities, this.TemplateConfiguration.Templates, _templates, _templateLocation, _outputLocation);
		}

		[TestMethod]
		[TestCategory("Database")]
		public void Render_All_Templates_SingleTable() {
			var entityName = "ApplicationUsage";
			var entities = _entities.Where(a => a.Name.Value == entityName).ToList();
			this.OutputProviderService.OutputToFiles(entities, this.TemplateConfiguration.Templates, _templates, _templateLocation, _outputLocation);
		}

		[TestMethod]
		[TestCategory("Database")]
		public void Render_All_Templates_SeveralTable() {
			var entityName = new List<string> { "ApplicationUsage", "User" };
			var entities = _entities.Where(a => entityName.Contains(a.Name.Value)).ToList();
			this.OutputProviderService.OutputToFiles(entities, this.TemplateConfiguration.Templates, _templates, _templateLocation, _outputLocation);
		}

		[TestMethod]
		[TestCategory("Database")]
		public void Render_All_Templates_ToZip_GivenDatabase() {
			var fileName = "Render_All_Templates_ToZip_" + DateTime.Now.Ticks.ToString() + ".zip";
			var output = $"{ _outputLocation }Output/{ fileName }";
			this.OutputProviderService.OutputToFile(_entities, this.TemplateConfiguration.Templates, _templates, _templateLocation, output);
		}

		[TestMethod]
		[TestCategory("Database")]
		public void Render_Single_Template_SingleTable() {			
			var entityName = "Contact";
			var templateName = "Service";
			var entities = _entities.Where(a => a.Name.Value == entityName).ToList();
			var templates = _templates.Where(a => a == templateName).ToList();
			this.OutputProviderService.OutputToFiles(entities, this.TemplateConfiguration.Templates, templates, _templateLocation, _outputLocation);
		}
	}
}