using Business.Common;
using Business.Conversion;
using Business.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Application;

namespace Test.Tests {
	[TestClass]
	public class TemplateRenderingTests {
		public TemplateConfiguration TemplateConfiguration { get; set; }
		public RazorTemplateRenderer RazorTemplateRenderer { get; set; }
		public EntityViewModel Test_Contact_Entity { get; set; }

		public TemplateRenderingTests() {
			var configurationJson = FileUtility.ReadTextFile<TemplateRenderingTests>("WorkingGeneratedConfiguration.json", "Configurations");
			this.TemplateConfiguration = JsonNetConversion.Deserialize<TemplateConfiguration>(configurationJson);
			this.RazorTemplateRenderer = new RazorTemplateRenderer();
			//this.Test_Contact_Entity = this.RazorTemplateRenderer.GetEntity<EntityViewModel>("Contact1.json", "Data");
			//this.Test_Contact_Entity = this.RazorTemplateRenderer.GetEntity<EntityViewModel>("ActivityFeedStatus.json", "Data");
			//this.Test_Contact_Entity = this.RazorTemplateRenderer.GetEntity<EntityViewModel>("WallComments.json", "Data");
			//this.Test_Contact_Entity = this.RazorTemplateRenderer.GetEntity<EntityViewModel>("WallFacebookUsers.json", "Data");
			//this.Test_Contact_Entity = this.RazorTemplateRenderer.GetEntity<EntityViewModel>("WallPosts.json", "Data");
			//this.Test_Contact_Entity = this.RazorTemplateRenderer.GetEntity<EntityViewModel>("Walls.json", "Data");
			this.Test_Contact_Entity = this.RazorTemplateRenderer.GetEntity<EntityViewModel>("WallUserPermissions.json", "Data");
		}

		private void renderTemplate(string templateName) {
			var template = this.RazorTemplateRenderer.GetTemplate(templateName, this.TemplateConfiguration);
			EntityService.PrepEntityForTemplate(this.Test_Contact_Entity, template);
			var result = new RazorTemplateRenderer().ParseTemplate(template.TemplateText, template.Name, this.Test_Contact_Entity);
			FileUtility.WriteFile<TemplateRenderingTests>(this.Test_Contact_Entity.OutputFilename, "Output\\", result);
		}

		[TestMethod]
		public void Render_All_Templates() {
			Render_Model_Template();
			Render_Service_Template();
			Render_ServiceInterface_Template();
			Render_RepositoryBase_Template();
			Render_RepositoryBaseInterface_Template();
			Render_DapperRepository_Template();
			Render_RepositoryInterface_Template();
			Render_Table_Template();
			Render_SelectAllTableValuedFunction_Template();
			Render_SelectByIdTableValuedFunction_Template();
			Render_Insert_Procedure_Template();
			Render_Delete_Procedure_Template();
			Render_Update_Procedure_Template();
			Render_ReadAllFunction_Template();
			Render_SelectByIdFunction_Template();
		}

		[TestMethod]
		public void Render_Model_Template() {
			renderTemplate("Model");
		}

		[TestMethod]
		public void Render_Service_Template() {
			renderTemplate("Service");
		}

		[TestMethod]
		public void Render_ServiceInterface_Template() {
			renderTemplate("ServiceInterface");
		}

		[TestMethod]
		public void Render_RepositoryBase_Template() {
			renderTemplate("Repository");
		}

		[TestMethod]
		public void Render_RepositoryBaseInterface_Template() {
			renderTemplate("RepositoryBaseInterface");
		}

		[TestMethod]
		public void Render_DapperRepository_Template() {
			renderTemplate("DapperRepositoryBase");
		}

		[TestMethod]
		public void Render_RepositoryInterface_Template() {
			renderTemplate("RepositoryInterface");
		}

		[TestMethod]
		public void Render_Table_Template() {
			renderTemplate("Table");
		}

		[TestMethod]
		public void Render_SelectAllTableValuedFunction_Template() {
			renderTemplate("SelectAll");
		}

		[TestMethod]
		public void Render_SelectByIdTableValuedFunction_Template() {
			renderTemplate("SelectById");
		}

		[TestMethod]
		public void Render_Insert_Procedure_Template() {
			renderTemplate("Insert");
		}

		[TestMethod]
		public void Render_Delete_Procedure_Template() {
			renderTemplate("Delete");
		}

		[TestMethod]
		public void Render_Update_Procedure_Template() {
			renderTemplate("Update");
		}

		[TestMethod]
		public void Render_ReadAllFunction_Template() {
			renderTemplate("ReadAllFunction");
		}

		[TestMethod]
		public void Render_SelectByIdFunction_Template() {
			renderTemplate("SelectByIdFunction");
		}
	}
}
