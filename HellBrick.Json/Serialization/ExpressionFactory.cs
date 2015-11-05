using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Utils;

namespace HellBrick.Json.Serialization
{
	internal static class ExpressionFactory
	{
		private static readonly MethodInfo _serializeInternalDefinition = Reflection
			.Method( () => SerializeGeneric<int>( default( Expression ), default( Expression ) ) )
			.GetGenericMethodDefinition();

		public static Expression Serialize( Expression value, Expression writer )
		{
			MethodInfo serializeInternalMethod = _serializeInternalDefinition.MakeGenericMethod( value.Type );
			Expression expression = serializeInternalMethod.Invoke( null, new object[] { value, writer } ) as Expression;
			return expression;
		}

		private static Expression SerializeGeneric<T>( Expression value, Expression writer )
		{
			ISerializerBuilder<T> builder = SerializerBuilderSelector.SelectBuilder<T>();
			Expression expression = builder.BuildSerializationExpression( value, writer );
			return expression;
		}
	}
}
