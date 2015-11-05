using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HellBrick.Json.Benchmarks.RoundTrip
{
	internal class RoundTripBenchmarkCore<T>
	{
		private static readonly JsonSerializer<T> _hellBrickSerializer = JsonFactory.SerializerFor<T>();
		private static readonly JsonDeserializer<T> _hellBrickDeserializer = JsonFactory.DeserializerFor<T>();

		private readonly T _value;

		public RoundTripBenchmarkCore( T value )
		{
			_value = value;
		}

		public void HellBrickJson()
		{
			string json = _hellBrickSerializer.Serialize( _value );
			T newValue = _hellBrickDeserializer.Deserialize( json );
		}

		public void NewtonsoftJson()
		{
			string json = JsonConvert.SerializeObject( _value );
			T newValue = JsonConvert.DeserializeObject<T>( json );
		}
	}
}
