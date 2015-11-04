using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Common;
using HellBrick.Json.Utils;
using Newtonsoft.Json;

namespace HellBrick.Json.Deserialization.Providers
{
	internal class ArrayDeserializerBuilderProvider : IDeserializerBuilderProvider
	{
		public IDeserializerBuilder<T> TryCreateBuilder<T>()
		{
			Type arrayType = typeof( T );
			if ( !arrayType.IsArray || arrayType.GetArrayRank() != 1 )
				return null;

			Type itemType = arrayType.GetElementType();
			return Activator.CreateInstance( typeof( ArrayDeserializerBuilder<> ).MakeGenericType( itemType ) ) as IDeserializerBuilder<T>;
		}

		private class ArrayDeserializerBuilder<TItem> : IDeserializerBuilder<TItem[]>
		{
			public Func<JsonReader, TItem[]> BuildDeserializationMethod()
			{
				JsonDeserializer<List<TItem>> innerDeserializer = JsonFactory.DeserializerFor<List<TItem>>();
				return reader => innerDeserializer.Deserialize( reader ).ToArray();
			}
		}
	}
}
