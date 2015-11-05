using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Deserialization;
using HellBrick.Json.DeserializationOld.Providers;
using HellBrick.Json.Utils;
using Newtonsoft.Json;
using static System.Linq.Expressions.Expression;

namespace HellBrick.Json.DeserializationOld
{
	internal static class DeserializerFactory
	{
		private static readonly IDeserializerBuilderProvider[] _providers =
		{
			new NonNullableValueDeserializerBuilderProvider()
		};

		public static JsonDeserializer<T> CreateDeserializer<T>()
		{
			IDeserializerBuilder<T> builder = SelectBuilder<T>();
			if ( builder == null )
				throw new NotSupportedException( $"Failed to create deserializer builder for {typeof( T ).Name}" );

			Func<JsonReader, T> serializationMethod = builder.BuildDeserializationMethod();
			return new JsonDeserializer<T>( serializationMethod );
		}

		private static IDeserializerBuilder<T> SelectBuilder<T>()
		{
			return _providers.Select( p => p.TryCreateBuilder<T>() ).FirstOrDefault( b => b != null );
		}

		public class Adapter : Deserialization.IDeserializerBuilderProvider
		{
			public Deserialization.IDeserializerBuilder<T> TryCreateBuilder<T>()
			{
				return SelectBuilder<T>() == null ? null : new Builder<T>();
			}

			private class Builder<T> : Deserialization.IDeserializerBuilder<T>
			{
				private static readonly MethodInfo _createOldDeserializer = Reflection.Method( () => CreateDeserializer<T>() );
				private static readonly MethodInfo _deserialize = Reflection.Method( ( JsonDeserializer<T> d ) => d.Deserialize( default( JsonReader ) ) );

				public Expression BuildDeserializationExpression( Expression reader )
				{
					return Call
					(
						Call( null, _createOldDeserializer ),
						_deserialize,
						reader
					);
				}
			}
		}
	}
}
