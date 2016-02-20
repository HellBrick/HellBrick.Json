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

namespace HellBrick.Json.Serialization.Providers
{
	internal class EnumSerializerBuilderProvider : ISerializerBuilderProvider
	{
		public ISerializerBuilder<T> TryCreateBuilder<T>() => typeof( T ).GetTypeInfo().IsEnum ? CreateBuilder<T>() : null;

		private ISerializerBuilder<T> CreateBuilder<T>()
		{
			T[] values = Enum.GetValues( typeof( T ) ).Cast<T>().ToArray();
			return new EnumSerializerBuilder<T>( values );
		}

		private class EnumSerializerBuilder<T> : ISerializerBuilder<T>
		{
			private readonly T[] _values;

			public EnumSerializerBuilder( T[] values )
			{
				_values = values;
			}

			public Expression BuildSerializationExpression( Expression value, Expression writer ) =>
				Switch
				(
					value,
					Call
					(
						writer,
						Reflection.Method( ( JsonWriter w ) => w.WriteValue( default( int ) ) ),
						Convert( value, typeof( int ) )
					),
					CreateCases( value, writer )
				);

			private SwitchCase[] CreateCases( Expression value, Expression writer ) => new SwitchCase[] { CreateCase( value, writer ) };
			private SwitchCase CreateCase( Expression value, Expression writer ) =>
				SwitchCase
				(
					Call
					(
						writer,
						Reflection.Method( ( JsonWriter w ) => w.WriteValue( default( string ) ) ),
						Call
						(
							value,
							Reflection.Method( ( T enumValue ) => enumValue.ToString() )
						)
					),
					_values.Select( v => Constant( v, typeof( T ) ) )
				);
		}
	}
}
