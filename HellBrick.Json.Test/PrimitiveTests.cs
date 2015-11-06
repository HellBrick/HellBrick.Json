using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace HellBrick.Json.Test
{
	public class PrimitiveTests
	{
		[Theory]
		[InlineData( 42, "42" )]
		public void Int32( int value, string json )
		{
			json.Should().RoundTripThrough( value );
		}

		[Theory]
		[InlineData( System.Int64.MaxValue, "9223372036854775807" )]
		public void Int64( long value, string json )
		{
			json.Should().RoundTripThrough( value );
		}

		[Theory]
		[InlineData( 42, "42.0" )]
		[InlineData( 0, "0.0" )]
		[InlineData( System.Double.NaN, "\"NaN\"" )]
		public void Double( double value, string json )
		{
			json.Should().RoundTripThrough( value );
		}

		[Theory]
		[InlineData( true, "true" )]
		[InlineData( false, "false" )]
		public void Boolean( bool value, string json )
		{
			json.Should().RoundTripThrough( value );
		}

		[Theory]
		[InlineData( "", "\"\"" )]
		[InlineData( null, "null" )]
		[InlineData( "text", "\"text\"" )]
		public void String( string value, string json )
		{
			json.Should().RoundTripThrough( value );
		}
	}
}
