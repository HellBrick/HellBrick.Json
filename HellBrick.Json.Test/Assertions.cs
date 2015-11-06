using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Numeric;
using FluentAssertions.Primitives;

namespace HellBrick.Json.Test
{
	internal static class Assertions
	{
		public static StringAssertions RoundTripThrough<T>( this StringAssertions target, T value )
		{
			target.DeserializeTo( value );
			value.Should().SerializeTo<T>( target.Subject );
			return target;
		}

		public static StringAssertions DeserializeTo<T>( this StringAssertions target, T value )
		{
			JsonFactory.DeserializerFor<T>().Deserialize( target.Subject ).Should().Be( value );
			return target;
		}

		public static ObjectAssertions SerializeTo<T>( this ObjectAssertions target, string json )
		{
			JsonFactory.SerializerFor<T>().Serialize( (T) target.Subject ).Should().Be( json );
			return target;
		}
	}
}
