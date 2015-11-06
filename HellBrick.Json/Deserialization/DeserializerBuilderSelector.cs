using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Deserialization.Providers;

namespace HellBrick.Json.Deserialization
{
	internal static class DeserializerBuilderSelector
	{
		private static readonly IDeserializerBuilderProvider[] _providers = new IDeserializerBuilderProvider[]
		{
			new NullableValueDeserializerBuilderProvider(),
			new NonNullableValueDeserializerBuilderProvider(),
			new Int64DeserializerBuilderProvider(),
			new StringParsingDeserializerBuilderProvider(),
			new ArrayDeserializerBuilderProvider(),
			new CollectionDeserializerBuilderProvider(),
			new ClassDeserializerBuilderProvider()
		};

		public static IDeserializerBuilder<T> SelectBuilder<T>()
		{
			IDeserializerBuilder<T> builder = _providers.Select( p => p.TryCreateBuilder<T>() ).FirstOrDefault( b => b != null );
			if ( builder == null )
				throw new NotSupportedException( $"Failed to select deserializer builder for {typeof( T ).FullName}" );

			return builder;
		}
	}
}
