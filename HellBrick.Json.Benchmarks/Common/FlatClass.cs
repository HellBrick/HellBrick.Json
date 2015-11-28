using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellBrick.Json.Benchmarks.Common
{
	public class FlatClass
	{
		public static FlatClass Create() => new FlatClass()
		{
			Number = 42,
			Text = "64",
			TimeStamp = DateTime.UtcNow,
			Url = new Uri( "http://bogus.host/1234" ),
			Duration = TimeSpan.FromMinutes( 42 ),
			Flag = true
		};

		public int Number { get; set; }
		public string Text { get; set; }
		public DateTime TimeStamp { get; set; }
		public Uri Url { get; set; }
		public TimeSpan? Duration { get; set; }
		public bool Flag { get; set; }
	}
}
