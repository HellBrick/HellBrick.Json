﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Utils;
using Newtonsoft.Json;

namespace HellBrick.Json.Serialization
{
	internal static class SerializerFactory
	{
		public static JsonSerializer<T> CreateSerializer<T>()
		{
			ParameterExpression value = Expression.Parameter( typeof( T ), "value" );
			ParameterExpression writer = Expression.Parameter( typeof( JsonWriter ), "writer" );

			Expression serializationBody = ExpressionFactory.Serialize( value, writer );
			Expression<Action<T, JsonWriter>> lambda = Expression.Lambda<Action<T, JsonWriter>>( serializationBody, value, writer );
			Action<T, JsonWriter> serializationMethod = lambda.Compile();
			return new JsonSerializer<T>( serializationMethod, serializationBody.ToDebugView() );
		}
	}
}
