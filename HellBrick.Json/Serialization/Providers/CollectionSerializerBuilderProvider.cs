using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Common;

namespace HellBrick.Json.Serialization.Providers
{
	internal class CollectionSerializerBuilderProvider : ISerializerBuilderProvider
	{
		public ISerializerBuilder<T> TryCreateBuilder<T>()
		{
			EnumerableTypeInfo enumerableTypeInfo = EnumerableTypeInfo.TryCreate( typeof( T ) );
			if ( enumerableTypeInfo == null )
				return null;

			Type builderType = typeof( CollectionSerializerBuilder<,> ).MakeGenericType( enumerableTypeInfo.CollectionType, enumerableTypeInfo.ItemType );
			ISerializerBuilder<T> builder = Activator.CreateInstance( builderType, new object[] { enumerableTypeInfo } ) as ISerializerBuilder<T>;
			return builder;
		}

		private class CollectionSerializerBuilder<TCollection, TItem> : ReferenceTypeSerializerBuilder<TCollection>
		{
			private readonly EnumerableTypeInfo _enumerableTypeInfo;

			public CollectionSerializerBuilder( EnumerableTypeInfo enumerableTypeInfo )
			{
				_enumerableTypeInfo = enumerableTypeInfo;
			}

			protected override Expression SerializeNonNullValue( Expression value, Expression writer )
			{
				LocalVariables locals = new LocalVariables( _enumerableTypeInfo );
				return Expression.Block( locals.Variables, EnumerateSerializerExpressions( value, writer, locals ) );
			}

			private IEnumerable<Expression> EnumerateSerializerExpressions( Expression value, Expression writer, LocalVariables locals )
			{
				yield return Expression.Call( writer, JsonWriterMembers.WriteStartArray );
				yield return Expression.Assign( locals.Enumerator, Expression.Call( value, _enumerableTypeInfo.GetEnumeratorMethod ) );

				LabelTarget loopBreak = Expression.Label( "loopBreak" );

				yield return Expression.Loop
				(
					Expression.IfThenElse
					(
						Expression.Call( locals.Enumerator, _enumerableTypeInfo.MoveNextMethod ),
						ExpressionFactory.Serialize( Expression.Property( locals.Enumerator, _enumerableTypeInfo.CurrentProperty ), writer ),
						Expression.Break( loopBreak )
					),
					loopBreak
				);

				yield return Expression.Call( writer, JsonWriterMembers.WriteEndArray );
			}

			private class LocalVariables
			{
				public LocalVariables( EnumerableTypeInfo enumerableTypeInfo )
				{
					Enumerator = Expression.Parameter( enumerableTypeInfo.GetEnumeratorMethod.ReturnType, "enumerator" );
					Variables = new ParameterExpression[] { Enumerator };
				}

				public ParameterExpression Enumerator { get; }
				public IEnumerable<ParameterExpression> Variables { get; }
			}
		}
	}
}
