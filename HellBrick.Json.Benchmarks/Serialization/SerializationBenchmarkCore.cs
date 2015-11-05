using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HellBrick.Json.Benchmarks.Serialization
{
	internal class SerializationBenchmarkCore<T>
	{
		private static readonly JsonSerializer<T> _hellBrickSerializer = JsonFactory.SerializerFor<T>();
		private static readonly JsonDeserializer<T> _hellBrickDeserializer = JsonFactory.DeserializerFor<T>();

		private readonly T _value;

		public SerializationBenchmarkCore( T value )
		{
			_value = value;
		}

		public void HellBrickJson()
		{
			string json = _hellBrickSerializer.Serialize( _value );
		}

		public void NewtonsoftJson()
		{
			string json = JsonConvert.SerializeObject( _value );
		}
	}
}
