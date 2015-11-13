using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Utils;
using Newtonsoft.Json;

namespace HellBrick.Json.Serialization
{
	internal abstract class ReferenceTypeSerializerBuilder<T> : ISerializerBuilder<T>
	{
		public Expression BuildSerializationExpression( Expression value, Expression writer ) =>
			Expression.IfThenElse
			(
				Expression.Equal( value, Expression.Constant( null, value.Type ) ),
				Expression.Call( writer, Reflection.Method( ( JsonWriter w ) => w.WriteNull() ) ),
				SerializeNonNullValue( value, writer )
			);

		protected abstract Expression SerializeNonNullValue( Expression value, Expression writer );
	}
}
