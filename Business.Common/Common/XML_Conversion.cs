using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Business.Common {
	public static class XML_Conversion {
		public static string RemoveAllNamespaces(string xmlDocument) {
			var xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));
			return xmlDocumentWithoutNs.ToString();
		}

		private static XElement RemoveAllNamespaces(XElement xmlDocument) {
			if (!xmlDocument.HasElements) {
				XElement xElement = new XElement(xmlDocument.Name.LocalName);
				xElement.Value = xmlDocument.Value;
				foreach (XAttribute attribute in xmlDocument.Attributes()) {
					xElement.Add(attribute);
				}
				return xElement;
			}
			return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
		}

		public static string ToXmlString<T>(object rawData, bool removeNamespaces = false) where T : class {
			var xml = ToXmlDocument<T>(rawData);
			var xmlString = XmlToString(xml);
			if (!removeNamespaces) {
				return xmlString;
			}
			return RemoveAllNamespaces(xmlString);
		}

		public static string XmlToString(XmlDocument xmlDoc, bool removeNamespaces = false) {
			using (var stringWriter = new StringWriter()) {
				using (var xmlTextWriter = XmlWriter.Create(stringWriter)) {
					xmlDoc.WriteTo(xmlTextWriter);
					xmlTextWriter.Flush();
					if (!removeNamespaces) {
						return stringWriter.GetStringBuilder().ToString();
					}
					return RemoveAllNamespaces(stringWriter.GetStringBuilder().ToString());
				}
			}
		}

		public static XmlDocument ToXmlDocument<T>(object rawData) where T : class {
			var stringWriter = new StringWriter();
			var xmlWriter = XmlWriter.Create(stringWriter);
			var data = new XmlSerializer(typeof(T));
			data.Serialize(xmlWriter, rawData);
			var xml = new XmlDocument();
			var xmlString = stringWriter.ToString();
			xml.LoadXml(xmlString);
			return xml;
		}

		public static string ReplaceHexadecimalSymbols(string xml) {
			if (string.IsNullOrEmpty(xml)) {
				return string.Empty;
			}
			var regex = "[\x00-\x08\x0B\x0C\x0E-\x1F]";
			xml = Regex.Replace(xml, regex, String.Empty, RegexOptions.Compiled);
			return xml;
		}

		public static XmlDocument ToXmlDocument(string xml) {
			xml = ReplaceHexadecimalSymbols(xml);
			var xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml.ToString());
			return xmlDocument;
		}

		public static T ToObject<T>(XmlDocument xml) where T : class {
			var xs = new XmlSerializer(typeof(T));
			var result = (T)xs.Deserialize(new StringReader(xml.OuterXml));
			return result;
		}
	}
}