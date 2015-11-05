using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet;

namespace HellBrick.Json.Benchmarks.RoundTrip
{
	public class IntArrayRoundTripBenchmark
	{
		private static readonly RoundTripBenchmarkCore<int[]> _core = new RoundTripBenchmarkCore<int[]>( Enumerable.Repeat( 42, 100 * 1000 ).ToArray() );

		[Benchmark]
		public void HellBrickJson() => _core.HellBrickJson();

		[Benchmark]
		public void NewtonsoftJson() => _core.NewtonsoftJson();
	}
}
