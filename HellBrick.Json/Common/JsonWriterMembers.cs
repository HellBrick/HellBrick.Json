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
	internal static class JsonWriterMembers
	{
		public static readonly MethodInfo WriteStartArray = Reflection.Method( ( JsonWriter w ) => w.WriteStartArray() );
		public static readonly MethodInfo WriteEndArray = Reflection.Method( ( JsonWriter w ) => w.WriteEndArray() );
		public static readonly MethodInfo WriteStartObject = Reflection.Method( ( JsonWriter w ) => w.WriteStartObject() );
		public static readonly MethodInfo WriteEndObject = Reflection.Method( ( JsonWriter w ) => w.WriteEndObject() );
		public static readonly MethodInfo WritePropertyName = Reflection.Method( ( JsonWriter w ) => w.WritePropertyName( default( string ) ) );
	}
}
