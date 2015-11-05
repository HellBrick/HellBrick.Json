using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Serialization.Providers;
using HellBrick.Json.Utils;
using Newtonsoft.Json;
using static System.Linq.Expressions.Expression;

namespace HellBrick.Json.Serialization
{
	internal static class OldSerializerFactory
	{
		private static readonly ISerializerBuilderProvider[] _providers =
		{
		};

		public static JsonSerializer<T> CreateSerializer<T>()
		{
			ISerializerBuilder<T> builder = CreateOldBuilder<T>();
			if ( builder == null )
				throw new NotSupportedException( $"Failed to create serializer builder for {typeof( T ).Name}" );

			Action<T, JsonWriter> serializationMethod = builder.BuildSerializationMethod();
			return new JsonSerializer<T>( serializationMethod );
		}

		private static ISerializerBuilder<T> CreateOldBuilder<T>()
		{
			return _providers.Select( p => p.TryCreateBuilder<T>() ).FirstOrDefault( b => b != null );
		}

		public class Adapter : ISerializeExpressionBuilderProvider
		{
			public ISerializeExpressionBuilder<T> TryCreateBuilder<T>()
			{
				return CreateOldBuilder<T>() != null ? new OldFactoryExpressionBuilder<T>() : null;
			}

			private class OldFactoryExpressionBuilder<T> : ISerializeExpressionBuilder<T>
			{
				private static readonly MethodInfo _createOldSerializer = Reflection.Method( () => CreateSerializer<T>() );
				private static readonly MethodInfo _serialize = Reflection.Method( ( JsonSerializer<T> s ) => s.Serialize( default( T ), default( JsonWriter ) ) );

				public Expression BuildSerializationExpression( Expression value, Expression writer )
				{
					return Call
					(
						Call( null, _createOldSerializer ),
						_serialize,
						value, writer
					);
				}
			}
		}
	}
}
