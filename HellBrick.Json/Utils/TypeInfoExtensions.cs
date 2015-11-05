using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HellBrick.Json.Utils
{
	public static class TypeInfoExtensions
	{
		public static bool IsNullable( this TypeInfo typeInfo ) => typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof( Nullable<> );
	}
}
