using System.Collections.Generic;
using System.IO;
using Business.Common;
using Business.Conversion;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Application;
using Property = Model.Application.Property;

namespace Test.Tests {
	[TestClass]
	public class JsonTests {
		[TestMethod]
		public void CreateJsonEntity() {
			var contact = new Entity {
				Name = "Contact",
				Properties = new List<Property> { 
					new Property {
						Name = "Id",
						SqlDataType = SqlDataType.BigInt,
						Nullable = false,
						PrimaryKey = true
					},
					new Property {
						Name = "FirstName",
						SqlDataType = SqlDataType.NVarChar,
						Nullable = false,
						Length = 50
					},
					new Property {
						Name = "LastName",
						SqlDataType = SqlDataType.NVarChar,
						Nullable = false,
						Length = 50
					},
					new Property {
						Name = "Address",
						SqlDataType = SqlDataType.NVarChar,
						Nullable = false,
						Length = 50
					},
					new Property {
						Name = "Address2",
						SqlDataType = SqlDataType.NVarChar,
						Nullable = false,
						Length = 50
					},
					new Property {
						Name = "City",
						SqlDataType = SqlDataType.NVarChar,
						Nullable = false,
						Length = 50
					},
					new Property {
						Name = "State",
						SqlDataType = SqlDataType.NVarChar,
						Nullable = false,
						Length = 50
					},
					new Property {
						Name = "Zip",
						SqlDataType = SqlDataType.NVarChar,
						Nullable = false,
						Length = 50
					}
				}
			};
			var json = JsonNetConversion.Serialize(contact);
			FileUtility.WriteFile<JsonTests>("Contact1.json", "Data", json);
		}

		[TestMethod]
		public void ReadJsonObject() {
			var json = FileUtility.ReadTextFile<JsonTests>("Contact1.json", "Data");
			var entity = JsonNetConversion.Deserialize<Entity>(json);
			Assert.IsNotNull(entity);
			Assert.IsTrue(entity.Properties[0].SqlDataType == SqlDataType.BigInt);
		}

		[TestMethod]
		public void TemplateExtractor() {
			var template = FileUtility.ReadTextFile<JsonTests>("Service.txt", "Templates\\OldTemplates\\Service");
			var templateProperties = TemplateModelExtractor.Extract(template, @"\[\[", @"\]\]");
			Assert.IsNotNull(templateProperties);
			Assert.IsTrue(templateProperties.Count > 0);
		}
		
		[TestMethod]
		public void GetAllFilesInDirectory() {
			var path = FileUtility.GetFullPath<JsonTests>("Templates\\OldTemplates");
			var entries = Directory.GetFileSystemEntries(path, "*.txt", SearchOption.AllDirectories);
			Assert.IsNotNull(entries);
			Assert.IsTrue(entries.Length > 0);
		}
		
		[TestMethod]
		public void GenerateConfiguration() {
			var entries = ConfigurationGenerator.GetFiles_FromProjectRelativePath("Templates\\NewTemplates", ".txt");
			Assert.IsNotNull(entries);
			Assert.IsTrue(entries.Count > 0);
			var templates = ConfigurationGenerator.GetConfiguration_FromFiles(entries);
			Assert.IsNotNull(templates);
			Assert.IsTrue(templates.Templates.Count > 0);
			var json = JsonNetConversion.Serialize(templates);
			FileUtility.WriteFile<JsonTests>("GeneratedConfiguration.json", "Configurations", json);
		}
	}
}
