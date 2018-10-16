using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Business.Common.Extensions;
using OfficeOpenXml;

namespace Business.Common {
	public class Excel_Conversion {		
		public void WriteToExcel<T>(IEnumerable rows, string fullPath) where T : class {
			WriteToExcel<T>(rows, null, fullPath);
		}

		public void WriteToExcel<T>(IEnumerable rows, string[] headerTitles, string fullPath) where T : class {
			var excelPackage = new ExcelPackage();
			var ws = excelPackage.Workbook.Worksheets.Add("Data");

			var headers = GetHeaders<T>().ToArray();
			if (headerTitles == null) {
				headerTitles = GetHeaderTitles<T>().ToArray();				
			}
			
			var row = 1;
			var col = 1;
			var cellName = string.Empty;

			foreach (string headerTitle in headerTitles) {
				cellName = this.getCellName(col, row);
				ws.Cells[cellName].Value = headerTitle;
				ws.Cells[cellName].Style.Font.Bold = true;
				col++;
			}

			col = 1;
			row += 1;

			foreach (object dataRow in rows) {
				foreach (string header in headers) {
					string strValue = string.Empty;
					try {
						strValue = dataRow.GetType().GetProperty(header).GetValue(dataRow, null).ToString();
						strValue = replaceSpecialCharacters(strValue);
					} catch {
					}

					cellName = this.getCellName(col, row);

					strValue = strValue.Replace("\n", " ").Replace("\r", " ").Replace("  ", " ");
					ws.Cells[cellName].Value = strValue;
					col++;
				}
				row++;
				col = 1;
			}

			this.formatWorksheet(ref ws, headers.Count(), 1);
			
			SaveToFile(excelPackage, fullPath);
		}

		public void SaveToFile(ExcelPackage excelPackage, string fullPath) { 
			var stream = File.Create(fullPath);
			excelPackage.SaveAs(stream);
			stream.Close();
		}

		private static List<string> GetHeaders<T>() where T : class {
			var propertyList = new List<string>();
			foreach (var p in typeof(T).GetProperties()) {
				propertyList.Add(p.Name);
			}
			return propertyList;
		}
				
		public static List<string> GetHeaderTitles<T>() where T : class {
			var headerTitleList = new List<string>();
			foreach (var p in typeof(T).GetProperties()) {
				headerTitleList.Add(getDisplayName(p));
			}
			return headerTitleList;			
		}

		private static string replaceSpecialCharacters(string value) {
			return value.Replace("’", "'").Replace("“", "\"").Replace("”", "\"").Replace("–", "-").Replace("…", "...");
		}

		private static string getDisplayName(PropertyInfo propertyInfo) {
			var textInfo = new CultureInfo("en-US", false).TextInfo;
			var title = propertyInfo.Name;
			var cAttr = propertyInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true);
			if (cAttr != null && cAttr.Count() > 0) {
				var attr = (DisplayNameAttribute)cAttr[0];
				title = attr.DisplayName;
			}
			title = title.UnCamelCase().Replace("  ", " ").Trim();
			return textInfo.ToTitleCase(title);
		}

		private void formatWorksheet(ref ExcelWorksheet ws, int totalColumns, int startRow) {
			for (int i = 1; i <= totalColumns; i++) {
				var endRow = ws.Dimension.End.Row - 1;
				endRow = endRow	< startRow ? startRow : endRow;//to correct for something wierd that I didn't take time to understand
				var columnCells = ws.Cells[startRow, i, endRow, i];
				var maxLength = columnCells.Max(cell => cell.Value.ToString().Count(c => char.IsLetterOrDigit(c)));
				ws.Column(i).Width = maxLength + 7;
			}
		}

		private string getCellName(int col, int row) {
			return this.numberToLetter(col) + row.ToString();
		}

		private int letterToNumber(string column) {
			if (!string.IsNullOrEmpty(column)) {
				var characters = column.ToUpperInvariant().ToCharArray();
				var sum = 0;
				for (int i = 0; i < characters.Length; i++) {
					sum *= 26;
					sum += characters[i] - 'A' + 1;
				}
				return sum;
			}
			return 0;
		}

		public string numberToLetter(int col) {
			var letter = col / 27;
			var remainder = col - (letter * 26);
			var retval = string.Empty;
			if (letter > 0) {
				retval = ((char)(letter + 64)).ToString();
			}
			if (remainder > 0) {
				retval = retval + ((char)(remainder + 64)).ToString();
			}
			return retval;
		}
	}
}