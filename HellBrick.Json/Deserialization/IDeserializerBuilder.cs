using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HellBrick.Json.Deserialization
{
	internal interface IDeserializerBuilder<T>
	{
		Expression BuildDeserializationExpression( Expression reader );
	}
}
