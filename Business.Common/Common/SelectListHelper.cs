using System.Collections.Generic;
using System.Web.Mvc;

namespace Business.Common {
	public static class SelectListHelper {
		public static SelectList CreateSelectList(List<SelectListItem> selectListItems, string selectedValue, string emptySelectText = ""){
			if (string.IsNullOrEmpty(emptySelectText)) {
				emptySelectText = "Please Select";
			}
			selectListItems.Insert(0, new SelectListItem { Value = string.Empty, Text = emptySelectText });
			return new SelectList(selectListItems, "Value", "Text", selectedValue);
		}

		public static SelectList CreateSelectList_FromEnum<TEnum>(string selectedValue) {
			return EnumHelp.EnumToSelectList<TEnum>(selectedValue);
		}
	}
}