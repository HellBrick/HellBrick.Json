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
				string text = JsonFactory.SerializerFor<Whatever[]>().Serialize( list );
				Console.WriteLine( text );
				list = JsonFactory.DeserializerFor<Whatever[]>().Deserialize( text );
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
		}

		public int X { get; set; }
		public string Y { get; set; }
		public SubClass Sub { get; set; }
	}

	class SubClass
	{
		public DateTime Timestamp { get; set; }
	}
}
