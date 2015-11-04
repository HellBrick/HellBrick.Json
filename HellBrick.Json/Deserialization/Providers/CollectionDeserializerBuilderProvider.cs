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

namespace HellBrick.Json.Deserialization.Providers
{
	internal class CollectionDeserializerBuilderProvider : IDeserializerBuilderProvider
	{
		ExpressionDeserializerBuilder<T> IDeserializerBuilderProvider.TryCreateBuilder<T>()
		{
			CollectionTypeInfo collectionTypeInfo = CollectionTypeInfo.TryCreate( typeof( T ) );
			if ( collectionTypeInfo == null )
				return null;

			Type builderType = typeof( CollectionDeserializerBuilder<,> ).MakeGenericType( collectionTypeInfo.EnumerableTypeInfo.CollectionType, collectionTypeInfo.EnumerableTypeInfo.ItemType );
			ExpressionDeserializerBuilder<T> builder = Activator.CreateInstance( builderType, new object[] { collectionTypeInfo.AddMethod } ) as ExpressionDeserializerBuilder<T>;
			return builder;
		}

		private class CollectionDeserializerBuilder<TCollection, TItem> : ExpressionDeserializerBuilder<TCollection>
		{
			private static readonly MethodInfo _readMethod = Reflection.Method( ( JsonReader r ) => r.Read() );

			private readonly MethodInfo _addMethod;

			public CollectionDeserializerBuilder( MethodInfo addMethod )
			{
				_addMethod = addMethod;
			}

			protected override Expression BuildDeserializerBody( DeserializeParameters<TCollection> parameters )
			{
				DeserializeLocalVariables locals = new DeserializeLocalVariables();
				return Expression.Block( typeof( TCollection ), locals.Variables, EnumerateDeserializerExpressions( parameters, locals ) );
			}

			private IEnumerable<Expression> EnumerateDeserializerExpressions( DeserializeParameters<TCollection> parameters, DeserializeLocalVariables locals )
			{
				LabelTarget returnLabel = Expression.Label( "return" );
				LabelTarget loopBreak = Expression.Label( "loopBreak" );

				yield return Expression.Assign( locals.Collection, Expression.New( typeof( TCollection ) ) );
				yield return Expression.Assign( locals.ItemDeserializer, Expression.Call( null, JsonFactoryMembers<TItem>.DeserializerFor ) );

				yield return Expression.IfThen
				(
					Expression.Call( parameters.Reader, _readMethod ),
					Expression.Loop
					(
						Expression.Block
						(
							Expression.Assign( locals.Item, Expression.Call( locals.ItemDeserializer, JsonDeserializerMembers<TItem>.Deserialize, parameters.Reader ) ),
							Expression.IfThenElse
							(
								Expression.NotEqual( Expression.Property( parameters.Reader, JsonReaderMembers.TokenType ), Expression.Constant( JsonToken.EndArray ) ),
								Expression.Call( locals.Collection, _addMethod, locals.Item ),
								Expression.Break( loopBreak )
							)
						),
						loopBreak
					)
				);

				yield return locals.Collection;
			}

			private class DeserializeLocalVariables
			{
				public DeserializeLocalVariables()
				{
					ItemDeserializer = Expression.Parameter( typeof( JsonDeserializer<TItem> ), "itemDeserializer" );
					Collection = Expression.Parameter( typeof( TCollection ), "collection" );
					Item = Expression.Parameter( typeof( TItem ), "item" );

					Variables = new ParameterExpression[] { ItemDeserializer, Collection, Item };
				}

				public ParameterExpression ItemDeserializer { get; }
				public ParameterExpression Collection { get; }
				public ParameterExpression Item { get; }

				public IEnumerable<ParameterExpression> Variables { get; }
			}
		}
	}
}
