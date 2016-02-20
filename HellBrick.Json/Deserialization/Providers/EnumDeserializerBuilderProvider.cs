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

namespace HellBrick.Json.Deserialization.Providers
{
	internal class EnumDeserializerBuilderProvider : IDeserializerBuilderProvider
	{
		public IDeserializerBuilder<T> TryCreateBuilder<T>() => typeof( T ).GetTypeInfo().IsEnum ? new EnumDeserializerBuilder<T>() : null;

		private class EnumDeserializerBuilder<T> : IDeserializerBuilder<T>
		{
			public Expression BuildDeserializationExpression( Expression reader ) =>
				Block
				(
					typeof( T ),
					Call( reader, JsonReaderMembers.Read ),
					Condition
					(
						Equal( Property( reader, JsonReaderMembers.TokenType ), Constant( JsonToken.Integer ) ),
						Convert
						(
							Convert
							(
								Property( reader, Reflection.Property( ( JsonReader r ) => r.Value ) ),
								typeof( long )
							),
							typeof( T )
						),
						Convert
						(
							Call
							(
								null,
								Reflection.Method( () => Enum.Parse( default( Type ), default( string ) ) ),
								Constant( typeof( T ), typeof( Type ) ),
								Convert
								(
									Property( reader, Reflection.Property( ( JsonReader r ) => r.Value ) ),
									typeof( string )
								)
							),
							typeof( T )
						)
					)
				);
		}
	}
}
