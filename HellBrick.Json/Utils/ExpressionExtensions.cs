using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HellBrick.Json.Utils
{
	internal static class ExpressionExtensions
	{
		public static string ToDebugView( this Expression expression )
		{
			using ( StringWriter textWriter = new StringWriter() )
			{
				DebugViewWriter.WriteTo( expression, textWriter );
				return textWriter.ToString();
			}
		}
	}
}
