using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HellBrick.Json.Deserialization
{
	internal static class DeserializerFactory
	{
		public static JsonDeserializer<T> CreateDeserializer<T>()
		{
			ParameterExpression writer = Expression.Parameter( typeof( JsonReader ), "reader" );

			Expression deserializationBody = ExpressionFactory.Deserialize( typeof( T ), writer );
			Expression<Func<JsonReader, T>> lambda = Expression.Lambda<Func<JsonReader, T>>( deserializationBody, writer );
			Func<JsonReader, T> deserializationMethod = lambda.Compile();
			return new JsonDeserializer<T>( deserializationMethod );
		}
	}
}
