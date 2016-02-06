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

		static RoundTripBenchmarkCore()
		{
			WarmUpJil();
		}

		private static void WarmUpJil()
		{
			try
			{
				JilJson( default( T ) );
			}
			catch
			{
				// Jil throws on some types that it doesn't support properly.
			}
		}

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

		public void JilJson() => JilJson( _value );

		private static void JilJson( T value )
		{
			string json = Jil.JSON.Serialize( value );
			value = Jil.JSON.Deserialize<T>( json );
		}
	}
}
