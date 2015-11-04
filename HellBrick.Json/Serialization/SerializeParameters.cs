using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HellBrick.Json.Serialization
{
	internal class SerializeParameters<T>
	{
		public SerializeParameters()
		{
			Value = Expression.Parameter( typeof( T ), "value" );
			Writer = Expression.Parameter( typeof( JsonWriter ), "writer" );
			Parameters = new ParameterExpression[] { Value, Writer };
		}

		public ParameterExpression Value { get; }
		public ParameterExpression Writer { get; }
		public IEnumerable<ParameterExpression> Parameters { get; }
	}
}
