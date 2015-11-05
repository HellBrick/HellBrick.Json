using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HellBrick.Json.Benchmarks.Deserialization
{
	internal class DeserializationBenchmarkCore<T>
	{
		private static readonly JsonSerializer<T> _hellBrickSerializer = JsonFactory.SerializerFor<T>();
		private static readonly JsonDeserializer<T> _hellBrickDeserializer = JsonFactory.DeserializerFor<T>();

		private readonly string _json;

		public DeserializationBenchmarkCore( T value )
		{
			_json = _hellBrickSerializer.Serialize( value );
		}

		public void HellBrickJson()
		{
			T value = _hellBrickDeserializer.Deserialize( _json );
		}

		public void NewtonsoftJson()
		{
			T value = JsonConvert.DeserializeObject<T>( _json );
		}
	}
}
