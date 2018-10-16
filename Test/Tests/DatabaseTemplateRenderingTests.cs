using System;
using System.Collections.Generic;
using System.Linq;
using Business.Common;
using Business.Conversion;
using Business.Services;
using CodeGenerator.V2.Business.Conversion;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Omu.ValueInjecter;
using Business.Common.Extensions;

namespace Test.Tests {
	[TestClass]
	public class DatabaseTemplateRenderingTests {
		public TemplateConfiguration TemplateConfiguration { get; set; }
		public RazorTemplateRenderer RazorTemplateRenderer { get; set; }

		public DatabaseTemplateRenderingTests() {
			var configurationJson = FileUtility.ReadTextFile<TemplateRenderingTests>("WorkingGeneratedConfiguration.json", "Configurations");
			this.TemplateConfiguration = JsonNetConversion.Deserialize<TemplateConfiguration>(configurationJson);
			this.RazorTemplateRenderer = new RazorTemplateRenderer();
		}

		private void renderTemplate(Template template, EntityViewModel entity) { 
			template = this.RazorTemplateRenderer.GetTemplate(template.Name, this.TemplateConfiguration);
			EntityService.PrepEntityForTemplate(entity, template);
			var result = new RazorTemplateRenderer().ParseTemplate(template.TemplateText, template.Name, entity);
			FileUtility.WriteFile<TemplateRenderingTests>(entity.OutputFilename, "Output\\", result);		
		}
		
		[TestMethod]
		[TestCategory("Database")]
		public void Render_All_Templates_GivenDatabase() {
			var connectionString = "Data Source=drolling;Initial Catalog=SpaceJackal_ContentPrototype;User Id=SpaceJackal_WebsiteUser;Password=SpaceJackal_WebsiteUser;";
			var databaseSchemaConverterService = new DatabaseSchemaConverterService(connectionString);
			var entities = databaseSchemaConverterService.GetEntitiesFromDatabase();
			foreach (var entity in entities) {
				foreach (var template in this.TemplateConfiguration.Templates) {
					renderTemplate(template, entity);
				}
			}
		}
		
		[TestMethod]
		[TestCategory("Database")]
		public void Render_Single_Template_GivenDatabase() {
			var connectionString = "Data Source=drolling;Initial Catalog=SpaceJackal_ContentPrototype;User Id=SpaceJackal_WebsiteUser;Password=SpaceJackal_WebsiteUser;";
			var databaseSchemaConverterService = new DatabaseSchemaConverterService(connectionString);
			var template = this.TemplateConfiguration.Templates.Where(a => a.Name == "Insert").FirstOrDefault();
			var entities = databaseSchemaConverterService.GetEntitiesFromDatabase();
			var entity = entities.Where(a => a.Name == "AppCodeSet").FirstOrDefault();
			renderTemplate(template, entity);
		}

		[TestMethod]
		[TestCategory("Database")]
		public void Render_All_Templates_ToZip_GivenDatabase() { 
			var connectionString = "Data Source=drolling;Initial Catalog=SpaceJackal_ContentPrototype;User Id=SpaceJackal_WebsiteUser;Password=SpaceJackal_WebsiteUser;";
			var databaseSchemaConverterService = new DatabaseSchemaConverterService(connectionString);
			var entities = databaseSchemaConverterService.GetEntitiesFromDatabase();
			var outputProviderService = new OutputProviderService();
			var selectedTemplates = this.TemplateConfiguration.Templates.Select(a => a.Name).ToList();
			var templateLocation = @"C:\Users\drolling\Documents\GitHub\CodeGenerator.V2\Test\";
			var outputLocation = @"C:\Users\drolling\Documents\GitHub\CodeGenerator.V2\Test\Output\";
			var fileName = "Render_All_Templates_ToZip_" + DateTime.Now.Ticks.ToString() + ".zip";
			outputProviderService.OutputToFile(entities, this.TemplateConfiguration, selectedTemplates, templateLocation, outputLocation + fileName);
		}
	}
}
