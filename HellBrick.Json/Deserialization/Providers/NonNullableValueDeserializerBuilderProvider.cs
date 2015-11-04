using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HellBrick.Json.Deserialization.Providers
{
	internal class NonNullableValueDeserializerBuilderProvider : IDeserializerBuilderProvider
	{
		public IDeserializerBuilder<T> TryCreateBuilder<T>()
		{
			TypeInfo typeInfo = typeof( T ).GetTypeInfo();
			if ( !typeInfo.IsValueType || IsNullable( typeInfo ) )
				return null;

			return Activator.CreateInstance( typeof( NonNullableValueDeserializerBuilder<> ).MakeGenericType( typeof( T ) ) ) as IDeserializerBuilder<T>;
		}

		private bool IsNullable( TypeInfo typeInfo ) => typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof( Nullable<> );

		private class NonNullableValueDeserializerBuilder<T> : RelayDeserializerBuilder<T, T?> where T : struct
		{
			public NonNullableValueDeserializerBuilder() : base( nullable => nullable.GetValueOrDefault() )
			{
			}
		}
	}
}
