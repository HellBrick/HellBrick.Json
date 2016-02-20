using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HellBrick.Json.TestConsole
{
	class Program
	{
		static void Main( string[] args )
		{
			Whatever[] list = { new Whatever( 42 ), new Whatever( 64 ), new Whatever( 128 ) };

			for ( int i = 0; i < 2; i++ )
			{
				JsonSerializer<Whatever[]> jsonSerializer = JsonFactory.SerializerFor<Whatever[]>();
				string text = jsonSerializer.Serialize( list );
				Console.WriteLine( text );
				JsonDeserializer<Whatever[]> jsonDeserializer = JsonFactory.DeserializerFor<Whatever[]>();
				list = jsonDeserializer.Deserialize( text );
			}
		}
	}

	class Whatever
	{
		public Whatever()
		{
		}

		public Whatever( int x )
		{
			X = x;
			Y = x.ToString();
			Sub = new SubClass() { Timestamp = DateTime.UtcNow };
			Type = SomeEnum.Something;
		}

		public int X { get; set; }
		public string Y { get; set; }
		public SubClass Sub { get; set; }
		public SomeEnum Type { get; set; }
	}

	class SubClass
	{
		public DateTime Timestamp { get; set; }
	}

	enum SomeEnum
	{
		None = 0,
		Something = 5
	}
}
