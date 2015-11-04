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
			int[] list = { 42, 64, 128 };

			for ( int i = 0; i < 2; i++ )
			{
				string text = JsonFactory.SerializerFor<int[]>().Serialize( list );
				Console.WriteLine( text );
				list = JsonFactory.DeserializerFor<int[]>().Deserialize( text );
			}
		}
	}
}
