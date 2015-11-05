using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HellBrick.Json.Serialization
{
	internal static class SerializerFactory
	{
		public static JsonSerializer<T> CreateSerializer<T>()
		{
			ISerializerBuilder<T> builder = SerializerBuilderSelector.SelectBuilder<T>();
			SerializeParameters<T> parameters = new SerializeParameters<T>();
			Expression serializationBody = builder.BuildSerializationExpression( parameters.Value, parameters.Writer );
			Expression<Action<T, JsonWriter>> lambda = Expression.Lambda<Action<T, JsonWriter>>( serializationBody, parameters.Parameters );
			Action<T, JsonWriter> serializationMethod = lambda.Compile();
			return new JsonSerializer<T>( serializationMethod );
		}
	}
}
