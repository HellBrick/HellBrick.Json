using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Utils;
using static System.Linq.Expressions.Expression;

namespace HellBrick.Json.Deserialization.Providers
{
	internal class ArrayDeserializerBuilderProvider : IDeserializerBuilderProvider
	{
		public IDeserializerBuilder<T> TryCreateBuilder<T>()
		{
			Type arrayType = typeof( T );
			if ( !arrayType.IsArray || arrayType.GetArrayRank() != 1 )
				return null;

			Type itemType = arrayType.GetElementType();
			return Activator.CreateInstance( typeof( ArrayDeserializerBuilder<> ).MakeGenericType( itemType ) ) as IDeserializerBuilder<T>;
		}

		private class ArrayDeserializerBuilder<TItem> : RelayDeserializerBuilder<TItem[], List<TItem>>
		{
			private readonly MethodInfo _toArray = Reflection.Method( () => Enumerable.ToArray( default( List<TItem> ) ) );

			protected override Expression ConvertToOuter( Expression inner )
			{
				ParameterExpression innerResult = Parameter( inner.Type, "collection" );
				return Block
				(
					new ParameterExpression[] { innerResult },
					Assign( innerResult, inner ),
					Condition
					(
						Equal( innerResult, Constant( null, inner.Type ) ),
						Constant( null, typeof( TItem[] ) ),
						Call( null, _toArray, innerResult )
					)
				);
			}
		}
	}
}
