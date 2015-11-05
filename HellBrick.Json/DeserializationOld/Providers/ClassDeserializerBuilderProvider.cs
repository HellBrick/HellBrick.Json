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

namespace HellBrick.Json.DeserializationOld.Providers
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

		private class ClassDeserializerBuilder<T> : ExpressionDeserializerBuilder<T>
		{
			private static readonly PropertyInfo _readerValue = Reflection.Property( ( JsonReader r ) => r.Value );
			private static readonly MethodInfo _readerSkip = Reflection.Method( ( JsonReader r ) => r.Skip() );

			private readonly ClassInfo _classInfo;

			public ClassDeserializerBuilder( ClassInfo classInfo )
			{
				_classInfo = classInfo;
			}

			protected override Expression BuildDeserializerBody( DeserializeParameters<T> parameters )
			{
				LocalVariables locals = new LocalVariables();
				return Block( typeof( T ), locals.Variables, EnumerateExpressions( parameters, locals ) );
			}

			private IEnumerable<Expression> EnumerateExpressions( DeserializeParameters<T> parameters, LocalVariables locals )
			{
				LabelTarget returnTarget = Label( typeof( T ), "returnTarget" );

				yield return IfThen
				(
					Or
					(
						Not( Call( parameters.Reader, JsonReaderMembers.Read ) ),
						NotEqual( Property( parameters.Reader, JsonReaderMembers.TokenType ), Constant( JsonToken.StartObject ) )
					),
					Return( returnTarget, Default( typeof( T ) ) )
				);

				yield return Assign( locals.Result, New( _classInfo.Constructor ) );

				SwitchCase[] propertyCases = _classInfo.SettableProperties.Select( p => CreateSwitchCase( p, parameters, locals ) ).ToArray();
				LabelTarget loopBreak = Label( "loopBreak" );
				yield return Loop
				(
					IfThenElse
					(
						And
						(
							Call( parameters.Reader, JsonReaderMembers.Read ),
							Equal( Property( parameters.Reader, JsonReaderMembers.TokenType ), Constant( JsonToken.PropertyName ) )
						),
						Switch
						(
							typeof( void ),
							Convert( Property( parameters.Reader, _readerValue ), typeof( string ) ),
							Call( parameters.Reader, _readerSkip ),
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

			private SwitchCase CreateSwitchCase( PropertyInfo property, DeserializeParameters<T> parameters, LocalVariables locals )
			{
				Type jsonDeserializerType = typeof( JsonDeserializer<> ).MakeGenericType( property.PropertyType );
				MethodInfo deserializeMethod =
					(
						from method in jsonDeserializerType.GetTypeInfo().GetDeclaredMethods( nameof( JsonDeserializer<object>.Deserialize ) )
						let args = method.GetParameters()
						where args.Length == 1 && args[ 0 ].ParameterType == typeof( JsonReader )
						select method
					)
					.FirstOrDefault();

				Type jsonFactoryMembersType = typeof( JsonFactoryMembers<> ).MakeGenericType( property.PropertyType );
				FieldInfo deserializerForField = jsonFactoryMembersType.GetTypeInfo().GetDeclaredField( nameof( JsonFactoryMembers<object>.DeserializerFor ) );
				MethodInfo deserializerFactoryMethod = deserializerForField.GetValue( null ) as MethodInfo;

				return SwitchCase
				(
					Assign
					(
						Property( locals.Result, property ),
						Call
						(
							Call( null, deserializerFactoryMethod ),
							deserializeMethod,
							parameters.Reader
						)
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
	}
}
