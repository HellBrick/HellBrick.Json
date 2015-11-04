using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Utils;

namespace HellBrick.Json.Common
{
	internal static class JsonFactoryMembers<T>
	{
		public static readonly MethodInfo SerializerFor = Reflection.Method( () => JsonFactory.SerializerFor<T>() );
		public static readonly MethodInfo DeserializerFor = Reflection.Method( () => JsonFactory.DeserializerFor<T>() );
	}
}
