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
	internal class ArraySerializeExpressionBuilderProvider : ISerializeExpressionBuilderProvider
	{
		public ISerializeExpressionBuilder<T> TryCreateBuilder<T>()
		{
			Type arrayType = typeof( T );
			if ( !arrayType.IsArray || arrayType.GetArrayRank() != 1 )
				return null;

			Type itemType = arrayType.GetElementType();
			return Activator.CreateInstance( typeof( ArraySerializeExpressionBuilder<> ).MakeGenericType( itemType ) ) as ISerializeExpressionBuilder<T>;
		}

		private class ArraySerializeExpressionBuilder<TItem> : ISerializeExpressionBuilder<TItem[]>
		{
			public Expression BuildSerializationExpression( Expression value, Expression writer )
			{
				LocalVariables locals = new LocalVariables();
				return Expression.Block( locals.Variables, EnumerateExpressions( value, writer, locals ) );
			}

			private IEnumerable<Expression> EnumerateExpressions( Expression value, Expression writer, LocalVariables locals )
			{
				yield return Expression.Call( writer, JsonWriterMembers.WriteStartArray );
				yield return Expression.Assign( locals.ItemSerializer, Expression.Call( null, JsonFactoryMembers<TItem>.SerializerFor ) );
				yield return Expression.Assign( locals.Index, Expression.Constant( 0 ) );

				LabelTarget loopBreak = Expression.Label( "loopBreak" );

				yield return Expression.Loop
				(
					Expression.IfThenElse
					(
						Expression.LessThan( locals.Index, Expression.ArrayLength( value ) ),
						Expression.Block
						(
							Expression.Call
							(
								locals.ItemSerializer,
								JsonSerializerMembers<TItem>.Serialize,
								Expression.ArrayIndex( value, locals.Index ), writer
							),
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
					ItemSerializer = Expression.Parameter( typeof( JsonSerializer<TItem> ), "itemSerializer" );
					Index = Expression.Parameter( typeof( int ), "i" );
					Variables = new ParameterExpression[] { ItemSerializer, Index };
				}

				public ParameterExpression Index { get; }
				public ParameterExpression ItemSerializer { get; }
				public IEnumerable<ParameterExpression> Variables { get; }
			}
		}
	}
}
