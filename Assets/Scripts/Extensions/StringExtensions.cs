using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Extensions
{
	public static class StringExtensions
	{
		public static string FormatWith(this string format, params object[] args)
		{
			return String.Format(format, args);
		}
	}
}
