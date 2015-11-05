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
using static HellBrick.Json.Serialization.ExpressionFactory;

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

		private class ClassSerializerBuilder<T> : ISerializerBuilder<T>
		{
			private readonly PropertyInfo[] _properties;

			public ClassSerializerBuilder( PropertyInfo[] properties )
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
					yield return Serialize( Property( value, property ), writer );
				}

				yield return Call( writer, JsonWriterMembers.WriteEndObject );
			}
		}
	}
}
