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
	internal class ArrayDeserializerBuilderProvider : IDeserializerBuilderProvider
	{
		public ExpressionDeserializerBuilder<T> TryCreateBuilder<T>()
		{
			Type arrayType = typeof( T );
			if ( !arrayType.IsArray || arrayType.GetArrayRank() != 1 )
				return null;

			Type itemType = arrayType.GetElementType();
			return Activator.CreateInstance( typeof( ArrayDeserializerBuilder<,> ).MakeGenericType( arrayType, itemType ) ) as ExpressionDeserializerBuilder<T>;
		}

		private class ArrayDeserializerBuilder<TArray, TItem> : ExpressionDeserializerBuilder<TArray>
		{
			private static readonly MethodInfo _toArray = Reflection.Method( () => Enumerable.ToArray( default( List<TItem> ) ) );

			protected override Expression BuildDeserializerBody( DeserializeParameters<TArray> parameters )
			{
				return Expression.Call
				(
					null,
					_toArray,
					Expression.Call
					(
						Expression.Call( null, JsonFactoryMembers<List<TItem>>.DeserializerFor ),
						JsonDeserializerMembers<List<TItem>>.Deserialize,
						parameters.Reader
					)
				);
			}
		}
	}
}
