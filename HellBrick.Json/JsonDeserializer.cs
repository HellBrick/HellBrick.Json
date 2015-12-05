using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HellBrick.Json
{
	public class JsonDeserializer<T>
	{
		private readonly Func<JsonReader, T> _deserializer;

		internal JsonDeserializer( Func<JsonReader, T> deserializer, string pseudoCode )
		{
			_deserializer = deserializer;
			PseudoCode = pseudoCode;
		}

		public string PseudoCode { get; }

		public T Deserialize( string jsonString )
		{
			using ( JsonTextReader jsonReader = new JsonTextReader( new StringReader( jsonString ) ) )
			{
				return Deserialize( jsonReader );
			}
		}

		public T Deserialize( JsonReader jsonReader ) => _deserializer( jsonReader );
	}
}
