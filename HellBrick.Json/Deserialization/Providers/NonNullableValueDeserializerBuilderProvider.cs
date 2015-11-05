using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Utils;

namespace HellBrick.Json.Deserialization.Providers
{
	internal class NonNullableValueDeserializerBuilderProvider : IDeserializerBuilderProvider
	{
		public IDeserializerBuilder<T> TryCreateBuilder<T>()
		{
			TypeInfo typeInfo = typeof( T ).GetTypeInfo();
			if ( !typeInfo.IsNonNullableValue() )
				return null;

			return Activator.CreateInstance( typeof( NonNullableValueDeserializerBuilder<> ).MakeGenericType( typeof( T ) ) ) as IDeserializerBuilder<T>;
		}

		private class NonNullableValueDeserializerBuilder<T> : RelayDeserializerBuilder<T, T?> where T : struct
		{
			private readonly PropertyInfo _value = Reflection.Property( ( T? nullable ) => nullable.Value );

			protected override Expression ConvertToOuter( Expression inner ) => Expression.Property( inner, _value );
		}
	}
}
