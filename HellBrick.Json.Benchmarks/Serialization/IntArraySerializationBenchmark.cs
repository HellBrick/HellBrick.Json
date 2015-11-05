using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet;
using HellBrick.Json.Benchmarks.Common;

namespace HellBrick.Json.Benchmarks.Serialization
{
	public class IntArraySerializationBenchmark
	{
		private static readonly SerializationBenchmarkCore<int[]> _core = new SerializationBenchmarkCore<int[]>( Enumerable.Repeat( 42, 100 * 1000 ).ToArray() );

		[Benchmark]
		public void HellBrickJson() => _core.HellBrickJson();

		[Benchmark]
		public void NewtonsoftJson() => _core.NewtonsoftJson();
	}
}
