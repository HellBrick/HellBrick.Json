using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet;

namespace HellBrick.Json.Benchmarks.RoundTrip
{
	public class FlatClass
	{
		public int Number { get; set; }
		public string Text { get; set; }
		public DateTime TimeStamp { get; set; }
	}

	public class FlatClassRoundTripBenchmark
	{
		private static readonly RoundTripBenchmarkCore<FlatClass> _core = new RoundTripBenchmarkCore<FlatClass>( new FlatClass() { Number = 42, Text = "64", TimeStamp = DateTime.UtcNow } );

		[Benchmark]
		public void HellBrickJson() => _core.HellBrickJson();

		[Benchmark]
		public void NewtonsoftJson() => _core.NewtonsoftJson();
	}
}
