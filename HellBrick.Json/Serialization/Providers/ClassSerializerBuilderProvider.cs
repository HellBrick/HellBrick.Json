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
	internal class ClassSerializerBuilderProvider : ISerializerBuilderProvider
	{
		public ISerializerBuilder<T> TryCreateBuilder<T>()
		{
			PropertyInfo[] gettableProperties = EnumerableGettableProperties( typeof( T ) ).ToArray();
			if ( gettableProperties.Length == 0 )
				return null;

			return new ClassSerializerBuilder<T>( gettableProperties );
		}

		private class ClassSerializerBuilder<T> : ExpressionSerializerBuilder<T>
		{
			private readonly PropertyInfo[] _properties;

			public ClassSerializerBuilder( PropertyInfo[] properties )
			{
				_properties = properties;
			}

			protected override Expression BuildSerializerBody( SerializeParameters<T> parameters )
			{
				return Block( EnumerateExpressions( parameters ) );
			}

			private IEnumerable<Expression> EnumerateExpressions( SerializeParameters<T> parameters )
			{
				yield return Call( parameters.Writer, JsonWriterMembers.WriteStartObject );

				foreach ( PropertyInfo property in _properties )
				{
					yield return Call( parameters.Writer, JsonWriterMembers.WritePropertyName, Constant( property.Name ) );
					yield return WritePropertyValueExpression( property, parameters );
				}

				yield return Call( parameters.Writer, JsonWriterMembers.WriteEndObject );
			}

			private Expression WritePropertyValueExpression( PropertyInfo property, SerializeParameters<T> parameters )
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
					Property( parameters.Value, property ), parameters.Writer
				);
			}
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
	}
}
