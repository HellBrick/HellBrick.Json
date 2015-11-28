using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet;
using HellBrick.Json.Benchmarks.Common;

namespace HellBrick.Json.Benchmarks.RoundTrip
{
	public class DefaultValuesRoundTripBenchmark
	{
		private static readonly FlatClass _instance = new FlatClass()
		{
			Number = 42,

			Duration = null,
			Flag = false,
			Text = null,
			TimeStamp = default( DateTime ),
			Url = null
		};

		private static readonly RoundTripBenchmarkCore<FlatClass> _core = new RoundTripBenchmarkCore<FlatClass>( _instance );

		[Benchmark]
		public void HellBrickJson() => _core.HellBrickJson();

		[Benchmark]
		public void NewtonsoftJson() => _core.NewtonsoftJson();
	}
}
