using Business.Common;
using Business.Common.Extensions;
using Business.Common.Responses;
using Business.Conversion;
using Data.Repository.Interfaces;
using Microsoft.SqlServer.Management.Smo;
using Model.Application;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Entity = Data.Models.Entities.Entity;

namespace Business.Services {
	public class EntityService : IEntityService {
		public IEntityRepository EntityRepository { get; set; }
		public static Regex rgx = new Regex("[^a-zA-Z0-9 -]");

		private static List<string> ignoredProperties_Update = new List<string> { "CreatedById", "CreatedDate", };
		private static List<string> maxTypes = new List<string> {
			"nvarchar",
			"nvarcharmax",
			"varbinarymax"
		};

		public EntityService(IEntityRepository entityRepository) {
			this.EntityRepository = entityRepository;
		}

		public static void PrepEntityForTemplate(EntityViewModel entity, Model.Application.Template template) {
			entity.Namespace = template.Namespace;
			if (string.IsNullOrEmpty(entity.Schema)) {//only set this value if it is not already set
				entity.Schema = template.Schema;
			}
			var keys = entity.Properties.Where(a => a.PrimaryKey);
			foreach (var key in keys) {
				SetKey(entity, key.Name, key.SqlDataType);
			}
			//todo: does this need to be here?
			SetName(entity, entity.Name.Value);
			entity.OutputFilename = template.OutputFilename.Contains("{0}") ? string.Format(template.OutputFilename, entity.Name.Value) : template.OutputFilename;
			foreach (var property in entity.Properties) {
				SetName(property, property.Name);
				property.DataType = getDataType(template.Language, property.SqlDataType);
			}
			entity.OutputFilename = string.Concat(template.RelativeOutputPath, "\\", entity.OutputFilename);
			entity.CSharpKeySignature = getCSharpKeySignature(entity.Keys);
			entity.CSharpKeyList = getCSharpKeyList(entity.Keys);
			entity.CSharpKeyTypeList = getCSharpKeyTypeList(entity.Keys);
			entity.SQLInsertSignature = getSQLInsertSignature(entity.Properties);
			entity.SQLUpdateSignature = getSQLUpdateSignature(entity.Properties);
			entity.CSharpInsertCallSignature = getCSharpInsertCallSignature(entity.Properties);
			entity.CSharpUpdateCallSignature = getCSharpUpdateCallSignature(entity.Properties);
			entity.CSharpKeyCallSignature = getCSharpKeyCallSignature(entity.Keys);
			entity.SQLPKSignature = getSQLPKSignature(entity.Properties);
			entity.SQLPKWhere = getSQLPKWhere(entity.Properties);
			entity.SQLSet = getSQLSet(entity.Properties);
		}

		public static void SetKey(EntityViewModel entity, Name name, SqlDataType sqlDataType) {
			var dataType = DataTypeConversion.ConvertTo_CSDataType(sqlDataType, false);
			var dbType = DataTypeConversion.ConvertTo_CSDbType(sqlDataType);
			if (entity.Keys.Any(a => a.Name.Value == name.Value)) {
				return;
			}
			entity.Keys.Add(new Key {
				Name = name,
				SQLDataType = sqlDataType.ToString(),
				DataType = dataType,
				DbType = dbType
			});
		}

		public static void SetKey(EntityViewModel entity, string name, SqlDataType sqlDataType) {
			if (string.IsNullOrEmpty(name)) {
				return;
			}
			if (entity.Keys.Any(a => a.Name.Value == name)) {
				return;
			}
			var dataType = DataTypeConversion.ConvertTo_CSDataType(sqlDataType, false);
			var dbType = DataTypeConversion.ConvertTo_CSDbType(sqlDataType);
			entity.Keys.Add(new Key {
				Name = new Name {
					Value = name,
					LowerCamelCase = rgx.Replace(StringConversion.Convert(name, StringCase.LowerCamelCase), ""),
					NameWithSpaces = name.UnCamelCase(),
				},
				SQLDataType = sqlDataType.ToString(),
				DataType = dataType,
				DbType = dbType
			});
		}

		public static void SetName(EntityViewModel entity, string name) {
			if (string.IsNullOrEmpty(name)) {
				return;
			}
			entity.Name = new Name {
				Value = name,
				LowerCamelCase = rgx.Replace(StringConversion.Convert(name, StringCase.LowerCamelCase), ""),
				NameWithSpaces = name.UnCamelCase(),
			};
		}

		public static void SetName(PropertyViewModel property, Name name) {
			property.Name = name;
		}

		public static void SetName(PropertyViewModel property, string name) {
			property.Name = new Name {
				Value = name,
				LowerCamelCase = rgx.Replace(StringConversion.Convert(name, StringCase.LowerCamelCase), ""),
				NameWithSpaces = name.UnCamelCase(),
			};
		}

		public TransactionResponse Create(Entity entity) {
			var result = this.EntityRepository.Create(entity);
			return result;
		}

		public TransactionResponse Delete(long id) {
			var result = this.EntityRepository.Delete(id);
			return result;
		}

		public TransactionResponse Update(Entity entity) {
			var result = this.EntityRepository.Update(entity);
			return result;
		}

		private static string getCSharpInsertCallSignature(List<PropertyViewModel> properties) {
			var isAssociation = properties.Count(a => a.PrimaryKey) > 1;
			var values = new List<string>();
			//crappy code alert
			if (!isAssociation) {
				foreach (var p in properties.Where(a => !a.PrimaryKey)) {
					values.Add(string.Concat("@", p.Name.Value));
				}

				var outputParams = new List<string>();
				foreach (var p in properties.Where(a => a.PrimaryKey)) {
					outputParams.Add(string.Concat("@", p.Name.Value, " OUTPUT"));
				}
				return string.Concat(string.Join(", ", values), ", ", string.Join(", ", outputParams));
			}

			foreach (var p in properties) {
				values.Add(string.Concat("@", p.Name.Value));
			}
			return string.Join(", ", values);
		}

		private static string getCSharpKeyCallSignature(List<Key> keys) {
			var values = new List<string>();
			foreach (var k in keys) {
				values.Add("@" + k.Name.LowerCamelCase);
			}
			var result = string.Join(", ", values);
			return result;
		}

		private static string getCSharpKeyList(List<Key> keys) {
			var values = new List<string>();
			foreach (var k in keys) {
				values.Add(k.Name.LowerCamelCase);
			}
			var result = string.Join(", ", values);
			return result;
		}

		private static string getCSharpKeySignature(List<Key> keys) {
			var values = new List<string>();
			foreach (var k in keys) {
				values.Add(string.Concat(k.DataType, " ", k.Name.LowerCamelCase));
			}
			var result = string.Join(", ", values);
			return result;
		}

		private static string getCSharpKeyTypeList(List<Key> keys) {
			var values = new List<string>();
			foreach (var k in keys) {
				values.Add(k.DataType);
			}
			var result = string.Join(", ", values);
			return result;
		}

		private static string getCSharpUpdateCallSignature(List<PropertyViewModel> properties) {
			var values = new List<string>();
			foreach (var p in properties) {
				if (ignoredProperties_Update.Contains(p.Name.Value)) { continue; }
				values.Add(string.Concat("@", p.Name.Value));
			}
			var result = string.Join(", ", values);
			return result;
		}

		private static string getDataType(string language, SqlDataType sqlDataType) {
			switch (language) {
				case "csharp":
					return DataTypeConversion.ConvertTo_CSDataType(sqlDataType, false);
				case "sql":
					return sqlDataType.ToString();
				default:
					return "string";
			}
		}

		private static string getSQLInsertSignature(List<PropertyViewModel> properties) {
			var isAssociation = properties.Count(a => a.PrimaryKey) > 1;
			var values = new List<string>();
			//crappy code alert
			if (!isAssociation) {
				foreach (var p in properties.Where(a => !a.PrimaryKey)) {
					var dataType = handleSpecialDataTypes(p);
					values.Add(string.Concat("@", p.Name.LowerCamelCase, " ", dataType));
				}

				var outputParams = new List<string>();
				foreach (var p in properties.Where(a => a.PrimaryKey)) {
					var dataType = p.DataType.ToLower();
					if (p.DataType.ToLower() == "nvarchar") {
						dataType += "(" + p.Length + ")";
					}
					outputParams.Add(string.Concat("@", p.Name.LowerCamelCase, " ", dataType, " OUTPUT"));
				}
				return string.Concat(string.Join(",\r\n\t", values), ",\r\n\t", string.Join(",\r\n\t", outputParams));
			}

			foreach (var p in properties) {
				var dataType = handleSpecialDataTypes(p);
				values.Add(string.Concat("@", p.Name.LowerCamelCase, " ", dataType));
			}
			return string.Join(",\r\n\t", values);
		}

		private static string getSQLPKSignature(List<PropertyViewModel> properties) {
			var values = new List<string>();
			foreach (var p in properties.Where(a => a.PrimaryKey)) {
				var dataType = p.DataType.ToLower();
				if (p.DataType.ToLower() == "nvarchar") {
					dataType += "(" + p.Length + ")";
				}
				values.Add(string.Concat("@", p.Name.LowerCamelCase, " ", dataType));
			}
			var result = string.Join(", ", values);
			return result;
		}

		private static string getSQLPKWhere(List<PropertyViewModel> properties) {
			var values = new List<string>();
			foreach (var p in properties.Where(a => a.PrimaryKey)) {
				values.Add(string.Concat(p.Name.Value, " = @", p.Name.LowerCamelCase));
			}
			var result = string.Join(" and ", values);
			return result;
		}

		private static string getSQLSet(List<PropertyViewModel> properties) {
			var values = new List<string>();
			var deseProps = properties.Where(a => !a.PrimaryKey);
			foreach (var p in deseProps) {
				if (ignoredProperties_Update.Contains(p.Name.Value)) { continue; }
				values.Add(string.Concat("[", p.Name.Value, "] = @", p.Name.LowerCamelCase));
			}
			var result = string.Join(",\r\n\t\t", values);
			return result;
		}

		private static string getSQLUpdateSignature(List<PropertyViewModel> properties) {
			var values = new List<string>();
			foreach (var p in properties) {
				if (ignoredProperties_Update.Contains(p.Name.Value)) { continue; }
				var dataType = handleSpecialDataTypes(p);
				values.Add(string.Concat("@", p.Name.LowerCamelCase, " ", dataType));
			}
			var result = string.Join(", ", values);
			return result;
		}

		private static string handleMaxTypes(PropertyViewModel propertyViewModel, string dataType) {
			if (dataType.Contains("max")) {
				return $"{ dataType.Replace("max", "") }(max)";
			} else {
				return $"{ dataType }({ propertyViewModel.Length })";
			}
		}

		private static string handleSpecialDataTypes(PropertyViewModel propertyViewModel) {
			var dataType = propertyViewModel.DataType.ToLower();
			var isMaxType = maxTypes.Contains(dataType);
			if (isMaxType) {
				return handleMaxTypes(propertyViewModel, dataType);
			}
			return dataType;
		}
	}
}