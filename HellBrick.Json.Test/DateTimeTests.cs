using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace HellBrick.Json.Test
{
	public class DateTimeTests
	{
		[Fact]
		public void UtcDate()
		{
			DateTime date = new DateTime( 2015, 11, 06, 14, 25, 00, DateTimeKind.Utc );
			const string json = "\"2015-11-06T14:25:00Z\"";
			json.Should().RoundTripThrough( date );
		}

		[Fact]
		public void LocalDate()
		{
			DateTime date = new DateTime( 2015, 11, 06, 14, 25, 00, DateTimeKind.Local );
			string json = $"\"2015-11-06T14:25:00{date.ToString( "zzz" )}\"";
			json.Should().RoundTripThrough( date );
		}

		[Fact]
		public void UnspecifiedDate()
		{
			DateTime date = new DateTime( 2015, 11, 06, 14, 25, 00, DateTimeKind.Unspecified );
			const string json = "\"2015-11-06T14:25:00\"";
			json.Should().RoundTripThrough( date );
		}
	}
}
