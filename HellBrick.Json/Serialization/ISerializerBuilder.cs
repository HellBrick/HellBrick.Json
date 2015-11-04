using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HellBrick.Json.Serialization
{
	internal interface ISerializerBuilder<T>
	{
		Action<T, JsonWriter> BuildSerializationMethod();
	}
}
