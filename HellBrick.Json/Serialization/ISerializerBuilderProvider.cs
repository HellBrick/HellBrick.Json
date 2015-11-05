using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellBrick.Json.Serialization
{
	internal interface ISerializerBuilderProvider
	{
		ISerializerBuilder<T> TryCreateBuilder<T>();
	}
}
