using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Utils;
using static System.Linq.Expressions.Expression;

namespace HellBrick.Json.Deserialization.Providers
{
	internal class Int64DeserializerBuilderProvider : IDeserializerBuilderProvider
	{
		public IDeserializerBuilder<T> TryCreateBuilder<T>()
		{
			return typeof( T ) == typeof( long? ) ? new Int64DeserializerBuilder() as IDeserializerBuilder<T> : null;
		}

		private class Int64DeserializerBuilder : RelayDeserializerBuilder<long?, decimal?>
		{
			protected override Expression ConvertToOuter( Expression inner )
			{
				ParameterExpression decimalValue = Parameter( typeof( decimal? ) );
				return Block
				(
					new ParameterExpression[] { decimalValue },
					Assign( decimalValue, inner ),
					Condition
					(
						Equal( decimalValue, Constant( null, typeof( decimal? ) ) ),
						Default( typeof( long? ) ),
						Convert
						(
							Convert
							(
								Property( decimalValue, Reflection.Property( ( decimal? d ) => d.Value ) ),
								typeof( long )
							),
							typeof( long? )
						)
					)
				);
			}
		}
	}
}
