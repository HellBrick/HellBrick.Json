using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Serialization.Providers;

namespace HellBrick.Json.Serialization
{
	internal static class SerializeExpressionBuilderSelector
	{
		private static readonly ISerializeExpressionBuilderProvider[] _providers = new ISerializeExpressionBuilderProvider[]
		{
			new ValueSerializeExpressionBuilderProvider(),
			new CollectionSerializeExpressionBuilderProvider(),
			new OldSerializerFactory.Adapter(),
			new ClassSerializeExpressionBuilderProvider()
		};

		public static ISerializeExpressionBuilder<T> SelectBuilder<T>()
		{
			ISerializeExpressionBuilder<T> builder = _providers.Select( p => p.TryCreateBuilder<T>() ).FirstOrDefault( b => b != null );
			if ( builder == null )
				throw new NotSupportedException( $"Failed to select serializer builder for {typeof( T ).Name}" );

			return builder;
		}
	}
}
