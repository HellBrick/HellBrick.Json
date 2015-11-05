using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HellBrick.Json.Serialization
{
	internal interface ISerializerBuilder<T>
	{
		Expression BuildSerializationExpression( Expression value, Expression writer );
	}
}
