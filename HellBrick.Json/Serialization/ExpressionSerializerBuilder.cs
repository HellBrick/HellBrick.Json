using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HellBrick.Json.Serialization
{
	internal abstract class ExpressionSerializerBuilder<T> : ISerializerBuilder<T>
	{
		Action<T, JsonWriter> ISerializerBuilder<T>.BuildSerializationMethod()
		{
			SerializeParameters<T> parameters = new SerializeParameters<T>();
			Expression body = BuildSerializerBody( parameters );
			Expression<Action<T, JsonWriter>> lambda = Expression.Lambda<Action<T, JsonWriter>>( body, parameters.Parameters );
			return lambda.Compile();
		}

		protected abstract Expression BuildSerializerBody( SerializeParameters<T> parameters );
	}
}
