using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Utils;

namespace HellBrick.Json.Deserialization
{
	internal static class ExpressionFactory
	{
		private static readonly MethodInfo _deserializeGenericDefinition = Reflection
			.Method( () => Deserialize<int>( default( Expression ) ) )
			.GetGenericMethodDefinition();

		public static Expression Deserialize( Type valueType, Expression reader )
		{
			MethodInfo deserializeDenericMethod = _deserializeGenericDefinition.MakeGenericMethod( valueType );
			Expression expression = deserializeDenericMethod.Invoke( null, new object[] { reader } ) as Expression;
			return expression;
		}

		private static Expression Deserialize<T>( Expression reader )
		{
			IDeserializerBuilder<T> builder = DeserializerBuilderSelector.SelectBuilder<T>();
			Expression expression = builder.BuildDeserializationExpression( reader );
			return expression;
		}
	}
}
