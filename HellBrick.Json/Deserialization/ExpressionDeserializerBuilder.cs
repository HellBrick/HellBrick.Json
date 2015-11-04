using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HellBrick.Json.Deserialization
{
	internal abstract class ExpressionDeserializerBuilder<T> : IDeserializerBuilder<T>
	{
		Func<JsonReader, T> IDeserializerBuilder<T>.BuildDeserializationMethod()
		{
			DeserializeParameters<T> parameters = new DeserializeParameters<T>();
			Expression body = BuildDeserializerBody( parameters );
			Expression<Func<JsonReader, T>> lambda = Expression.Lambda<Func<JsonReader, T>>( body, parameters.Parameters );
			return lambda.Compile();
		}

		protected abstract Expression BuildDeserializerBody( DeserializeParameters<T> parameters );
	}
}
