using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Utils;
using Newtonsoft.Json;

namespace HellBrick.Json.Serialization.Providers
{
	internal class ValueSerializerBuilderProvider : ISerializerBuilderProvider
	{
		private readonly Dictionary<Type, MethodInfo> _writeValueMethods;

		public ValueSerializerBuilderProvider()
		{
			_writeValueMethods =
				(
					from method in typeof( JsonWriter ).GetTypeInfo().GetDeclaredMethods( "WriteValue" )
					let parameters = method.GetParameters()
					where parameters.Length > 0
					let parameterType = parameters[ 0 ].ParameterType
					where parameterType != typeof( object )
					select new { Method = method, ParameterType = parameterType }
				)
				.ToDictionary( p => p.ParameterType, m => m.Method );
		}

		public ISerializerBuilder<T> TryCreateBuilder<T>()
		{
			MethodInfo writeMethod = _writeValueMethods.GetOrDefault( typeof( T ) );
			return writeMethod != null ? new ValueSerializerBuilder<T>( writeMethod ) : null;
		}

		private class ValueSerializerBuilder<T> : ExpressionSerializerBuilder<T>
		{
			private readonly MethodInfo _writeMethod;

			public ValueSerializerBuilder( MethodInfo writeMethod )
			{
				_writeMethod = writeMethod;
			}

			protected override Expression BuildSerializerBody( SerializeParameters<T> parameters )
			{
				return Expression.Call( parameters.Writer, _writeMethod, parameters.Value );
			}
		}
	}
}
