using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HellBrick.Json.Serialization
{
	internal static class ExpressionFactory
	{
		public static Expression Serialize( Expression value, Expression writer )
		{
			MethodInfo selectBuilderDefinition = typeof( SerializerBuilderSelector ).GetTypeInfo().GetDeclaredMethod( "SelectBuilder" );
			MethodInfo selectBuilderMethod = selectBuilderDefinition.GetGenericMethodDefinition().MakeGenericMethod( value.Type );
			object builder = selectBuilderMethod.Invoke( null, new object[ 0 ] );

			MethodInfo buildMethod = builder.GetType().GetTypeInfo().GetDeclaredMethod( nameof( ISerializerBuilder<int>.BuildSerializationExpression ) );
			object boxedExpression = buildMethod.Invoke( builder, new object[] { value, writer } );
			Expression expression = (Expression) boxedExpression;
			return expression;
		}
	}
}
