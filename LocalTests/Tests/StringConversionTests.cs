using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Business.Common;
using Business.Common;

namespace LocalTests.Tests {
	[TestClass]
	public class StringConversionTests {
		[TestMethod]
		public void ToLowerCamelCase() {
			var input = "MonitorActivityId";
			var expectedResult = "monitorActivityId";
			var actual = StringConversion.Convert(input, StringCase.LowerCamelCase);
			Assert.AreEqual(expectedResult, actual);
		}

		[TestMethod]
		public void ToPascalCase() {
			var input = "monitorActivityId";
			var expectedResult = "MonitorActivityId";
			var actual = StringConversion.Convert(input, StringCase.PascalCase);
			Assert.AreEqual(expectedResult, actual);
		}
	}
}
