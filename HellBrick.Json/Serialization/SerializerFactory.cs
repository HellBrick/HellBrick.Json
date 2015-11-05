using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Serialization.Providers;
using Newtonsoft.Json;

namespace HellBrick.Json.Serialization
{
	internal static class SerializerFactory
	{
		private static readonly ISerializerBuilderProvider[] _providers =
		{
			new ValueSerializerBuilderProvider(),
			new ArraySerializerBuilderProvider(),
			new CollectionSerializerBuilderProvider(),
			new FallbackSerializerBuilderProvider()
		};

		public static JsonSerializer<T> CreateSerializer<T>()
		{
			ISerializerBuilder<T> builder = _providers.Select( p => p.TryCreateBuilder<T>() ).FirstOrDefault( b => b != null );
			if ( builder == null )
				throw new NotSupportedException( $"Failed to create serializer builder for {typeof( T ).Name}" );

			Action<T, JsonWriter> serializationMethod = builder.BuildSerializationMethod();
			return new JsonSerializer<T>( serializationMethod );
		}
	}
}
