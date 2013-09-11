using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Four51.APISDK.Common
{
	public static class Utils
	{
		public static DateTime DateTimeParseCatch(string input)
		{
			try
			{
				return DateTime.ParseExact(input, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
			}
			catch
			{
				return DateTime.Now;
			}
		}

		public static Double DoubleParseCatch(string input)
		{
			try
			{
				return Double.Parse(input);
			}
			catch
			{
				return 0.00;
			}
		}

		public static Int32 Int32ParseCatch(string input)
		{
			try
			{
				return Int32.Parse(input);
			}
			catch
			{
				return 0;
			}
		}

		public static bool BoolParseCatch(string input)
		{
			try
			{
				return Boolean.Parse(input);
			}
			catch
			{
				return false;
			}
		}
	}
}
