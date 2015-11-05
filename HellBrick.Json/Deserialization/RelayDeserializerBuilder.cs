using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Linq.Expressions.Expression;
using static HellBrick.Json.Deserialization.ExpressionFactory;

namespace HellBrick.Json.Deserialization
{
	internal abstract class RelayDeserializerBuilder<TOuter, TInner> : IDeserializerBuilder<TOuter>
	{
		public Expression BuildDeserializationExpression( Expression reader ) => ConvertToOuter( Deserialize( typeof( TInner ), reader ) );
		protected abstract Expression ConvertToOuter( Expression inner );
	}
}
