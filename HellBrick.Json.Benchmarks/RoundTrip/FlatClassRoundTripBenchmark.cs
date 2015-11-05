using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet;
using HellBrick.Json.Benchmarks.Common;

namespace HellBrick.Json.Benchmarks.RoundTrip
{
	public class FlatClassRoundTripBenchmark
	{
		private static readonly RoundTripBenchmarkCore<FlatClass> _core = new RoundTripBenchmarkCore<FlatClass>( FlatClass.Create() );

		[Benchmark]
		public void HellBrickJson() => _core.HellBrickJson();

		[Benchmark]
		public void NewtonsoftJson() => _core.NewtonsoftJson();
	}
}
