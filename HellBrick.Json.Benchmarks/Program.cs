using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet;
using HellBrick.Json.Benchmarks.RoundTrip;

namespace HellBrick.Json.Benchmarks
{
	class Program
	{
		static void Main( string[] args )
		{
			new BenchmarkRunner().RunCompetition( new IntArrayRoundTripBenchmark() );
		}
	}
}
