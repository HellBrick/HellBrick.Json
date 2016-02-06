using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet;
using HellBrick.Json.Benchmarks.Common;

namespace HellBrick.Json.Benchmarks.RoundTrip
{
	public class JilCompatibleFlatClassRoundTripBenchmark
	{
		private static readonly RoundTripBenchmarkCore<JilCompatibleFlatClass> _core = new RoundTripBenchmarkCore<JilCompatibleFlatClass>( JilCompatibleFlatClass.Create() );

		[Benchmark]
		public void HellBrickJson() => _core.HellBrickJson();

		[Benchmark]
		public void NewtonsoftJson() => _core.NewtonsoftJson();

		[Benchmark]
		public void Jil() => _core.JilJson();
	}
}
