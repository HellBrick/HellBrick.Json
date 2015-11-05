using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Common;
using HellBrick.Json.Utils;

namespace HellBrick.Json.Serialization.Providers
{
	internal class ArraySerializerBuilderProvider : ISerializerBuilderProvider
	{
		public ISerializerBuilder<T> TryCreateBuilder<T>()
		{
			Type arrayType = typeof( T );
			if ( !arrayType.IsArray || arrayType.GetArrayRank() != 1 )
				return null;

			Type itemType = arrayType.GetElementType();
			return Activator.CreateInstance( typeof( ArraySerializerBuilder<> ).MakeGenericType( itemType ) ) as ISerializerBuilder<T>;
		}

		private class ArraySerializerBuilder<TItem> : ISerializerBuilder<TItem[]>
		{
			public Expression BuildSerializationExpression( Expression value, Expression writer )
			{
				LocalVariables locals = new LocalVariables();
				return Expression.Block( locals.Variables, EnumerateExpressions( value, writer, locals ) );
			}

			private IEnumerable<Expression> EnumerateExpressions( Expression value, Expression writer, LocalVariables locals )
			{
				yield return Expression.Call( writer, JsonWriterMembers.WriteStartArray );
				yield return Expression.Assign( locals.Index, Expression.Constant( 0 ) );

				LabelTarget loopBreak = Expression.Label( "loopBreak" );

				yield return Expression.Loop
				(
					Expression.IfThenElse
					(
						Expression.LessThan( locals.Index, Expression.ArrayLength( value ) ),
						Expression.Block
						(
							ExpressionFactory.Serialize( Expression.ArrayIndex( value, locals.Index ), writer ),
							Expression.PostIncrementAssign( locals.Index )
						),
						Expression.Break( loopBreak )
					),
					loopBreak
				);

				yield return Expression.Call( writer, JsonWriterMembers.WriteEndArray );
			}

			private class LocalVariables
			{
				public LocalVariables()
				{
					Index = Expression.Parameter( typeof( int ), "i" );
					Variables = new ParameterExpression[] { Index };
				}

				public ParameterExpression Index { get; }
				public IEnumerable<ParameterExpression> Variables { get; }
			}
		}
	}
}
