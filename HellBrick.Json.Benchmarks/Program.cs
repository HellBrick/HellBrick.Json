using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet;
using HellBrick.Json.Benchmarks.RoundTrip;
using HellBrick.Json.Benchmarks.Serialization;

namespace HellBrick.Json.Benchmarks
{
	class Program
	{
		static void Main( string[] args )
		{
			Type[] benchmarkTypes = typeof( Program ).Assembly.GetTypes()
				.Where( t => t.GetTypeInfo().GetMethods().Any( m => m.GetCustomAttribute<BenchmarkAttribute>() != null ) )
				.OrderBy( t => t.Namespace )
				.ToArray();

			BenchmarkCompetitionSwitch benchmarkSwitch = new BenchmarkCompetitionSwitch( benchmarkTypes );
			benchmarkSwitch.Run( args );
		}
	}
}
