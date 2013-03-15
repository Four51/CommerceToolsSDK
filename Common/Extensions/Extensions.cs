using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Linq.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;

namespace Four51.APISDK.Common {
	public static class Extensions {
		public static bool IsNullable(this Type type)
		{
			// http://msdn.microsoft.com/en-us/library/ms366789.aspx
			return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
		}

		public static bool IsNullOrWhiteSpace(this string s)
		{
			try
			{
				return string.IsNullOrWhiteSpace(s);
			}
			catch (NullReferenceException)
			{
				return true;
			}
		}

		public static string Extrinsic(this IEnumerable<XElement> list, string field)
		{
			return (string)list.Elements("Extrinsic").Where(
				x => x.Attribute("name").Value == field).First();
		}

		public static string TryExtrinsicValue(this IEnumerable<XElement> list, string field) {
			try {
				return list.Elements("Extrinsic").Where(x => x.Attribute("name").Value == field).First().Value;
			}
			catch {
				return "";
			}
		}

		public static string FirstElement(this IEnumerable<XElement> list, string field) {
			try {
				return list.Descendants(field).First().Value;
			}
			catch {
				return "";
			}
		}

		public static Dictionary<string, string> OrderFields(this IEnumerable<XElement> list) {
			return list.Elements("Extrinsic")
				.Where(x => x.Attribute("name").Value == "OrderFields")
				.Elements("Extrinsic").ToDictionary(x => x.Attribute("name").Value, n => n.Value);
		}

		public static Dictionary<string, string> ProductSpecs(this IEnumerable<XElement> list) {
			return list.Elements("Extrinsic")
				.Where(x => x.Attribute("name").Value == "ProductSpecs")
				.Elements("Extrinsic").ToDictionary(x => x.Attribute("name").Value, n => n.Value);
		}

		public static string SuperSubstring(this string str, int begin, int len) {
			int l = len < str.Length ? len : str.Length;
			return str.Substring(begin, l);
		}

		public static string ToJsonObject(this object obj) {
			DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
			using (MemoryStream ms = new MemoryStream()) {
				serializer.WriteObject(ms, obj);
				StringBuilder sb = new StringBuilder();
				sb.Append(Encoding.UTF8.GetString(ms.ToArray()));
				return sb.ToString();
			}
		}

		public static string ToJson(this object obj)
		{
			return new JavaScriptSerializer().Serialize(obj);
		}

		public static T To<T>(this object obj, T defaultIfNull)
		{
			if (obj == null)
			{
				return defaultIfNull;
			}
			if (obj is T)
			{
				return (T)obj;
			}

			Type type = typeof(T);
			if (type.IsNullable())
			{
				type = type.GetGenericArguments()[0];
			}

			try
			{
				if (type.IsEnum)
				{
					return (T)Enum.Parse(type, obj.ToString(), true);
				}
				return (T)Convert.ChangeType(obj, type);
			}
			catch
			{
				return default(T);
			}
		}

		/// <summary>
		/// The most useful method known to mankind
		/// </summary>
		public static T To<T>(this object obj)
		{
			return obj.To(default(T));
		}

		// http://stackoverflow.com/questions/1141874/c-detecting-anonymoustype-newname-value-and-convert-into-dictionarystring-ob
		public static IDictionary<string, T> ToDictionary<T>(this object obj)
		{
			return obj.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(obj, null).To<T>());
		}
	}
}
