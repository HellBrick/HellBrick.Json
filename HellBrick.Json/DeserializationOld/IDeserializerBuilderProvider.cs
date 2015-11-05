using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellBrick.Json.DeserializationOld
{
	internal interface IDeserializerBuilderProvider
	{
		IDeserializerBuilder<T> TryCreateBuilder<T>();
	}
}
