using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HellBrick.Json.Serialization.Providers
{
	internal class FallbackSerializerBuilderProvider : ISerializerBuilderProvider
	{
		public ISerializerBuilder<T> TryCreateBuilder<T>() => new FallbackSerializerBuilder<T>();

		private class FallbackSerializerBuilder<T> : ISerializerBuilder<T>
		{
			public Action<T, JsonWriter> BuildSerializationMethod()
			{
				JsonSerializer newtonsoftSerializer = new JsonSerializer();
				return ( T value, JsonWriter writer ) => newtonsoftSerializer.Serialize( writer, value );
			}
		}
	}
}
