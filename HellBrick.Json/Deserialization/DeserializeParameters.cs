using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HellBrick.Json.Deserialization
{
	internal class DeserializeParameters<T>
	{
		public DeserializeParameters()
		{
			Reader = Expression.Parameter( typeof( JsonReader ), "reader" );
			Parameters = new ParameterExpression[] { Reader };
		}

		public ParameterExpression Reader { get; }
		public IEnumerable<ParameterExpression> Parameters { get; }
	}
}
