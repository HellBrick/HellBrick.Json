using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellBrick.Json.Benchmarks.Common
{
	public class JilCompatibleFlatClass
	{
		public static JilCompatibleFlatClass Create() => new JilCompatibleFlatClass()
		{
			Number = 42,
			Text = "64",
			TimeStamp = DateTime.UtcNow,
			Duration = TimeSpan.FromMinutes( 42 ),
			Flag = true
		};

		public int Number { get; set; }
		public string Text { get; set; }
		public DateTime TimeStamp { get; set; }
		public TimeSpan? Duration { get; set; }
		public bool Flag { get; set; }
	}
}
