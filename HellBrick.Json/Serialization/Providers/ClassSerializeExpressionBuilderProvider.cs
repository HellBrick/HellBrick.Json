using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Common;
using Newtonsoft.Json;
using static System.Linq.Expressions.Expression;

namespace HellBrick.Json.Serialization.Providers
{
	internal class ClassSerializeExpressionBuilderProvider : ISerializeExpressionBuilderProvider
	{
		public ISerializeExpressionBuilder<T> TryCreateBuilder<T>()
		{
			PropertyInfo[] gettableProperties = EnumerableGettableProperties( typeof( T ) ).ToArray();
			if ( gettableProperties.Length == 0 )
				return null;

			return new ClassSerializeExpressionBuilder<T>( gettableProperties );
		}

		private IEnumerable<PropertyInfo> EnumerableGettableProperties( Type type )
		{
			while ( type != null )
			{
				TypeInfo typeInfo = type.GetTypeInfo();
				foreach ( PropertyInfo property in typeInfo.DeclaredProperties.Where( p => p.CanRead && p.GetMethod.IsPublic && p.GetIndexParameters().Length == 0 ) )
					yield return property;

				type = typeInfo.BaseType;
			}
		}

		private class ClassSerializeExpressionBuilder<T> : ISerializeExpressionBuilder<T>
		{
			private readonly PropertyInfo[] _properties;

			public ClassSerializeExpressionBuilder( PropertyInfo[] properties )
			{
				_properties = properties;
			}

			public Expression BuildSerializationExpression( Expression value, Expression writer )
			{
				return Block( EnumerateExpressions( value, writer ) );
			}

			private IEnumerable<Expression> EnumerateExpressions( Expression value, Expression writer )
			{
				yield return Call( writer, JsonWriterMembers.WriteStartObject );

				foreach ( PropertyInfo property in _properties )
				{
					yield return Call( writer, JsonWriterMembers.WritePropertyName, Constant( property.Name ) );
					yield return WritePropertyValueExpression( property, value, writer );
				}

				yield return Call( writer, JsonWriterMembers.WriteEndObject );
			}

			private Expression WritePropertyValueExpression( PropertyInfo property, Expression value, Expression writer )
			{
				Type jsonSerializerType = typeof( JsonSerializer<> ).MakeGenericType( property.PropertyType );
				MethodInfo serializeMethod =
					(
						from method in jsonSerializerType.GetTypeInfo().GetDeclaredMethods( nameof( JsonSerializer<object>.Serialize ) )
						let args = method.GetParameters()
						where args.Length == 2 && args[ 0 ].ParameterType == property.PropertyType && args[ 1 ].ParameterType == typeof( JsonWriter )
						select method
					)
					.FirstOrDefault();

				Type jsonFactoryMembersType = typeof( JsonFactoryMembers<> ).MakeGenericType( property.PropertyType );
				FieldInfo serializerForField = jsonFactoryMembersType.GetTypeInfo().GetDeclaredField( nameof( JsonFactoryMembers<object>.SerializerFor ) );
				MethodInfo serializerFactoryMethod = serializerForField.GetValue( null ) as MethodInfo;

				return Call
				(
					Call( null, serializerFactoryMethod ),
					serializeMethod,
					Property( value, property ), writer
				);
			}
		}
	}
}
