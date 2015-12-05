using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Newtonsoft.Json;

namespace HellBrick.Json
{
	public sealed class JsonSerializer<T>
	{
		private readonly Action<T, JsonWriter> _serializer;

		internal JsonSerializer( Action<T, JsonWriter> serializer, string pseudoCode )
		{
			_serializer = serializer;
			PseudoCode = pseudoCode;
		}

		public string PseudoCode { get; }

		public string Serialize( T value )
		{
			using ( StringWriter stringWriter = new StringWriter() )
			{
				using ( JsonTextWriter jsonWriter = new JsonTextWriter( stringWriter ) )
				{
					Serialize( value, jsonWriter );
				}

				return stringWriter.ToString();
			}
		}

		public void Serialize( T value, JsonWriter jsonWriter ) => _serializer( value, jsonWriter );
	}
}