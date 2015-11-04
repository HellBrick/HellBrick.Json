using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Utils;
using Newtonsoft.Json;

namespace HellBrick.Json.Common
{
	internal static class JsonSerializerMembers<T>
	{
		public static readonly MethodInfo Serialize = Reflection.Method( ( JsonSerializer<T> s ) => s.Serialize( default( T ), default( JsonWriter ) ) );
	}
}
