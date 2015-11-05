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
using static System.Linq.Expressions.Expression;
using static HellBrick.Json.Deserialization.ExpressionFactory;

namespace HellBrick.Json.Deserialization.Providers
{
	internal class ClassDeserializerBuilderProvider : IDeserializerBuilderProvider
	{
		public IDeserializerBuilder<T> TryCreateBuilder<T>()
		{
			TypeInfo typeInfo = typeof( T ).GetTypeInfo();
			if ( typeInfo.IsValueType )
				return null;

			ClassInfo classInfo = new ClassInfo( typeof( T ) );
			if ( classInfo.Constructor == null )
				throw new NotSupportedException( $"Deserializing {typeInfo.Name} is not supported because the type doesn't have a public parameterless constructor." );

			return new ClassDeserializerBuilder<T>( classInfo );
		}

		private class ClassInfo
		{
			public ClassInfo( Type type )
			{
				TypeInfo typeInfo = type.GetTypeInfo();

				Constructor = typeInfo.DeclaredConstructors.FirstOrDefault( c => c.IsPublic && c.GetParameters().Length == 0 );
				SettableProperties = EnumerateSettableProperties( typeInfo ).ToArray();
			}

			private IEnumerable<PropertyInfo> EnumerateSettableProperties( TypeInfo typeInfo )
			{
				while ( typeInfo != null )
				{
					foreach ( PropertyInfo property in typeInfo.DeclaredProperties.Where( p => p.CanWrite && p.SetMethod.IsPublic ) )
						yield return property;

					typeInfo = typeInfo.BaseType?.GetTypeInfo();
				}
			}

			public ConstructorInfo Constructor { get; }
			public PropertyInfo[] SettableProperties { get; }
		}

		private class ClassDeserializerBuilder<T> : IDeserializerBuilder<T>
		{
			private static readonly PropertyInfo _readerValue = Reflection.Property( ( JsonReader r ) => r.Value );
			private static readonly MethodInfo _readerSkip = Reflection.Method( ( JsonReader r ) => r.Skip() );

			private readonly ClassInfo _classInfo;

			public ClassDeserializerBuilder( ClassInfo classInfo )
			{
				_classInfo = classInfo;
			}

			public Expression BuildDeserializationExpression( Expression reader )
			{
				LocalVariables locals = new LocalVariables();
				return Block( typeof( T ), locals.Variables, EnumerateExpressions( reader, locals ) );
			}

			private IEnumerable<Expression> EnumerateExpressions( Expression reader, LocalVariables locals )
			{
				LabelTarget returnTarget = Label( typeof( T ), "returnTarget" );

				yield return IfThen
				(
					Or
					(
						Not( Call( reader, JsonReaderMembers.Read ) ),
						NotEqual( Property( reader, JsonReaderMembers.TokenType ), Constant( JsonToken.StartObject ) )
					),
					Return( returnTarget, Default( typeof( T ) ) )
				);

				yield return Assign( locals.Result, New( _classInfo.Constructor ) );

				SwitchCase[] propertyCases = _classInfo.SettableProperties.Select( p => CreateSwitchCase( p, reader, locals ) ).ToArray();
				LabelTarget loopBreak = Label( "loopBreak" );
				yield return Loop
				(
					IfThenElse
					(
						And
						(
							Call( reader, JsonReaderMembers.Read ),
							Equal( Property( reader, JsonReaderMembers.TokenType ), Constant( JsonToken.PropertyName ) )
						),
						Switch
						(
							typeof( void ),
							Convert( Property( reader, _readerValue ), typeof( string ) ),
							Call( reader, _readerSkip ),
							null,
							propertyCases
						),
						Break( loopBreak )
					),
					loopBreak
				);

				yield return Return( returnTarget, locals.Result );
				yield return Label( returnTarget, Default( typeof( T ) ) );
			}

			private SwitchCase CreateSwitchCase( PropertyInfo property, Expression reader, LocalVariables locals )
			{
				return SwitchCase
				(
					Assign
					(
						Property( locals.Result, property ),
						Deserialize( property.PropertyType, reader )
					),
					Constant( property.Name )
				);
			}

			private class LocalVariables
			{
				public LocalVariables()
				{
					Result = Parameter( typeof( T ), "result" );
					Variables = new ParameterExpression[] { Result };
				}

				public ParameterExpression Result { get; }
				public IEnumerable<ParameterExpression> Variables { get; }
			}
		}
	}
}
