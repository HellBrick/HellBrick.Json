using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet;
using HellBrick.Json.Benchmarks.Common;

namespace HellBrick.Json.Benchmarks.Deserialization
{
	public class IntArrayDeserializationBenchmark
	{
		private readonly DeserializationBenchmarkCore<int[]> _core = new DeserializationBenchmarkCore<int[]>( Enumerable.Repeat( 42, 100 * 1000 ).ToArray() );

		[Benchmark]
		public void HellBrickJson() => _core.HellBrickJson();

		[Benchmark]
		public void NewtonsoftJson() => _core.NewtonsoftJson();
	}
}
