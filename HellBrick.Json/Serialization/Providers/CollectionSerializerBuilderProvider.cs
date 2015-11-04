using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Common;
using HellBrick.Json.Utils;
using Newtonsoft.Json;

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

		private class CollectionSerializerBuilder<TCollection, TItem> : ExpressionSerializerBuilder<TCollection>
		{
			private readonly EnumerableTypeInfo _enumerableTypeInfo;

			public CollectionSerializerBuilder( EnumerableTypeInfo enumerableTypeInfo )
			{
				_enumerableTypeInfo = enumerableTypeInfo;
			}

			protected override Expression BuildSerializerBody( SerializeParameters<TCollection> parameters )
			{
				LocalVariables locals = new LocalVariables( _enumerableTypeInfo );
				return Expression.Block( locals.Variables, EnumerateSerializerExpressions( parameters, locals ) );
			}

			private IEnumerable<Expression> EnumerateSerializerExpressions( SerializeParameters<TCollection> parameters, LocalVariables locals )
			{
				yield return Expression.Call( parameters.Writer, JsonWriterMembers.WriteStartArray );
				yield return Expression.Assign( locals.ItemSerializer, Expression.Call( null, JsonFactoryMembers<TItem>.SerializerFor ) );
				yield return Expression.Assign( locals.Enumerator, Expression.Call( parameters.Value, _enumerableTypeInfo.GetEnumeratorMethod ) );

				LabelTarget loopBreak = Expression.Label( "loopBreak" );

				yield return Expression.Loop
				(
					Expression.IfThenElse
					(
						Expression.Call( locals.Enumerator, _enumerableTypeInfo.MoveNextMethod ),
						Expression.Call
						(
							locals.ItemSerializer,
							JsonSerializerMembers<TItem>.Serialize,
							Expression.Property( locals.Enumerator, _enumerableTypeInfo.CurrentProperty ), parameters.Writer
						),
						Expression.Break( loopBreak )
					),
					loopBreak
				);

				yield return Expression.Call( parameters.Writer, JsonWriterMembers.WriteEndArray );
			}

			private class LocalVariables
			{
				public LocalVariables( EnumerableTypeInfo enumerableTypeInfo )
				{
					Enumerator = Expression.Parameter( enumerableTypeInfo.GetEnumeratorMethod.ReturnType, "enumerator" );
					ItemSerializer = Expression.Parameter( typeof( JsonSerializer<TItem> ), "itemSerializer" );

					Variables = new ParameterExpression[] { Enumerator, ItemSerializer };
				}

				public ParameterExpression Enumerator { get; }
				public ParameterExpression ItemSerializer { get; }

				public IEnumerable<ParameterExpression> Variables { get; }
			}
		}
	}
}
