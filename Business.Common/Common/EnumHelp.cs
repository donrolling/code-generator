using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Business.Common.Extensions;

namespace Business.Common {
	public static class EnumHelp {
		public static string GetDesciption(object value) {
			string name = value.ToString();
			var enumType = value.GetType();
			var field = enumType.GetField(name);
			object[] attrs = field.GetCustomAttributes(true);
			if (attrs.Length > 0) {
				var da = (DisplayAttribute)attrs[0];
				return da.Description;
			}

			return name.UnCamelCase();
		}
		public static EnumNameDescriptionValue GetEnumNameAndDescription(object value) {
			var name = value.ToString();
			var enumType = value.GetType();
			var field = enumType.GetField(name);
			DisplayAttribute displayAttribute = field.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
			if (displayAttribute != null) {
				return new EnumNameDescriptionValue { Name = displayAttribute.Name, Description = displayAttribute.Description };
			}

			return new EnumNameDescriptionValue { Name = name.UnCamelCase(), Description = name.UnCamelCase() };
		}
		public static List<EnumNameDescriptionValue> GetListOfEnumNameAndDescription<T>() {
			string[] names = Enum.GetNames(typeof(T));
			var list = new List<EnumNameDescriptionValue>();

			foreach (string name in names) {
				var theEnum = (T)Enum.Parse(typeof(T), name);
				var enumType = theEnum.GetType();
				var field = enumType.GetField(name);

				var displayAttribute = field.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
				if (displayAttribute != null) {
					list.Add(
						new EnumNameDescriptionValue { 
							Name = displayAttribute.Name, 
							Description = displayAttribute.Description, 
							Value = name
						});
				} else {
					list.Add(
						new EnumNameDescriptionValue {
							Name = name.UnCamelCase(),
							Description = name.UnCamelCase(),
							Value = name
						});
				}
			}

			return list;
		}
		public static SelectList EnumToSelectList<TEnum>(string selectedValue){
			var values = GetListOfEnumNameAndDescription<TEnum>().Where(a => a.Name != "Invalid").OrderBy(a => a.Name);
            var items = values.Select(a => new SelectListItem { Text = a.Name, Value = a.Value }).ToList();
            var notSelected = new SelectListItem { Value = "Invalid", Text = "Please Select" };
            if (string.IsNullOrEmpty(selectedValue)) {
                selectedValue = notSelected.Value;
            }
            items.Insert(0, notSelected);
            var selectList = new SelectList(items, "Value", "Text", selectedValue);
			return selectList;
		}
		public static string GetName(object value) {
			string name = value.ToString();
			Type enumType = value.GetType();
			FieldInfo field = enumType.GetField(name);
			object[] attrs = field.GetCustomAttributes(true);
			if (attrs.Length > 0) {
				var da = (DisplayAttribute)attrs[0];
				return da.Name;
			}
			return name.UnCamelCase();
		}
	}
}