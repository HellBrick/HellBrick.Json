﻿using System;
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
	internal class ValueSerializeExpressionBuilderProvider : ISerializeExpressionBuilderProvider
	{
		private readonly Dictionary<Type, MethodInfo> _writeValueMethods =
			(
				from method in typeof( JsonWriter ).GetTypeInfo().GetDeclaredMethods( "WriteValue" )
				let parameters = method.GetParameters()
				where parameters.Length > 0
				let parameterType = parameters[ 0 ].ParameterType
				where parameterType != typeof( object )
				select new { Method = method, ParameterType = parameterType }
			)
			.ToDictionary( p => p.ParameterType, m => m.Method );

		public ISerializeExpressionBuilder<T> TryCreateBuilder<T>()
		{
			MethodInfo writeMethod = _writeValueMethods.GetOrDefault( typeof( T ) );
			return writeMethod != null ? new ValueSerializeExpressionBuilder<T>( writeMethod ) : null;
		}

		private class ValueSerializeExpressionBuilder<T> : ISerializeExpressionBuilder<T>
		{
			private readonly MethodInfo _writeMethod;

			public ValueSerializeExpressionBuilder( MethodInfo writeMethod )
			{
				_writeMethod = writeMethod;
			}

			public Expression BuildSerializationExpression( Expression value, Expression writer ) => Expression.Call( writer, _writeMethod, value );
		}
	}
}
