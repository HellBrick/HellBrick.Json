using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Deserialization.Providers;
using Newtonsoft.Json;

namespace HellBrick.Json.Deserialization
{
	internal static class DeserializerFactory
	{
		private static readonly IDeserializerBuilderProvider[] _providers =
		{
			new CollectionDeserializerBuilderProvider(),
			new ArrayDeserializerBuilderProvider(),
			new NullableValueDeserializerBuilderProvider(),
			new NonNullableValueDeserializerBuilderProvider()
		};

		public static JsonDeserializer<T> CreateDeserializer<T>()
		{
			IDeserializerBuilder<T> builder = _providers.Select( p => p.TryCreateBuilder<T>() ).FirstOrDefault( b => b != null );
			if ( builder == null )
				throw new NotSupportedException( $"Failed to create deserializer builder for {typeof( T ).Name}" );

			Func<JsonReader, T> serializationMethod = builder.BuildDeserializationMethod();
			return new JsonDeserializer<T>( serializationMethod );
		}
	}
}
