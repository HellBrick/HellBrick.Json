using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Serialization.Providers;

namespace HellBrick.Json.Serialization
{
	internal static class SerializerBuilderSelector
	{
		private static readonly ISerializerBuilderProvider[] _providers = new ISerializerBuilderProvider[]
		{
			new ValueSerializerBuilderProvider(),
			new EnumSerializerBuilderProvider(),
			new ArraySerializerBuilderProvider(),
			new CollectionSerializerBuilderProvider(),
			new ClassSerializerBuilderProvider()
		};

		public static ISerializerBuilder<T> SelectBuilder<T>()
		{
			ISerializerBuilder<T> builder = _providers.Select( p => p.TryCreateBuilder<T>() ).FirstOrDefault( b => b != null );
			if ( builder == null )
				throw new NotSupportedException( $"Failed to select serializer builder for {typeof( T ).Name}" );

			return builder;
		}
	}
}
