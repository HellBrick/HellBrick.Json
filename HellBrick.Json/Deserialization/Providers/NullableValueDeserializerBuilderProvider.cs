using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Utils;
using Newtonsoft.Json;

namespace HellBrick.Json.Deserialization.Providers
{
	internal class NullableValueDeserializerBuilderProvider : IDeserializerBuilderProvider
	{
		private readonly Dictionary<Type, MethodInfo> _readValueMethods;

		public NullableValueDeserializerBuilderProvider()
		{
			_readValueMethods =
				new MethodInfo[]
				{
					Reflection.Method( ( JsonReader r ) => r.ReadAsBytes() ),
					Reflection.Method( ( JsonReader r ) => r.ReadAsDateTime() ),
					Reflection.Method( ( JsonReader r ) => r.ReadAsDateTimeOffset() ),
					Reflection.Method( ( JsonReader r ) => r.ReadAsDecimal() ),
					Reflection.Method( ( JsonReader r ) => r.ReadAsInt32() ),
					Reflection.Method( ( JsonReader r ) => r.ReadAsString() )
				}
				.ToDictionary( m => m.ReturnType, m => m );
		}

		public ExpressionDeserializerBuilder<T> TryCreateBuilder<T>()
		{
			MethodInfo readMethod = _readValueMethods.GetOrDefault( typeof( T ) );
			return readMethod != null ? new ValueDeserializerBuilder<T>( readMethod ) : null;
		}

		private class ValueDeserializerBuilder<T> : ExpressionDeserializerBuilder<T>
		{
			private readonly MethodInfo _readMethod;

			public ValueDeserializerBuilder( MethodInfo readMethod )
			{
				_readMethod = readMethod;
			}

			protected override Expression BuildDeserializerBody( DeserializeParameters<T> parameters )
			{
				return Expression.Call( parameters.Reader, _readMethod );
			}
		}
	}
}
