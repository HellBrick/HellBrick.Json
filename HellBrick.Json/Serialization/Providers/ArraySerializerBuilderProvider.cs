using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Common;

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
			return Activator.CreateInstance( typeof( ArraySerializerBuilder<,> ).MakeGenericType( arrayType, itemType ) ) as ISerializerBuilder<T>;
		}

		private class ArraySerializerBuilder<TArray, TItem> : ExpressionSerializerBuilder<TArray>
		{
			private readonly PropertyInfo _lengthProperty;

			public ArraySerializerBuilder()
			{
				_lengthProperty = typeof( TArray ).GetTypeInfo().BaseType.GetTypeInfo().GetDeclaredProperty( "Length" );
			}

			protected override Expression BuildSerializerBody( SerializeParameters<TArray> parameters )
			{
				LocalVariables locals = new LocalVariables();
				return Expression.Block( locals.Variables, EnumerateExpressions( parameters, locals ) );
			}

			private IEnumerable<Expression> EnumerateExpressions( SerializeParameters<TArray> parameters, LocalVariables locals )
			{
				yield return Expression.Call( parameters.Writer, JsonWriterMembers.WriteStartArray );
				yield return Expression.Assign( locals.ItemSerializer, Expression.Call( null, JsonFactoryMembers<TItem>.SerializerFor ) );
				yield return Expression.Assign( locals.Index, Expression.Constant( 0 ) );

				LabelTarget loopBreak = Expression.Label( "loopBreak" );

				yield return Expression.Loop
				(
					Expression.IfThenElse
					(
						Expression.LessThan( locals.Index, Expression.Property( parameters.Value, _lengthProperty ) ),
						Expression.Block
						(
							Expression.Call
							(
								locals.ItemSerializer,
								JsonSerializerMembers<TItem>.Serialize,
								Expression.ArrayIndex( parameters.Value, locals.Index ), parameters.Writer
							),
							Expression.PostIncrementAssign( locals.Index )
						),
						Expression.Break( loopBreak )
					),
					loopBreak
				);

				yield return Expression.Call( parameters.Writer, JsonWriterMembers.WriteEndArray );
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
