using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using HellBrick.Json.Test.Types;
using Xunit;

namespace HellBrick.Json.Test
{
	public class ClassTests
	{
		[Fact]
		public void NullIsRoundTripped()
		{
			string json = "null";
			SimpleFlatClass value = null;
			json.Should().RoundTripThrough( value );
		}

		[Fact]
		public void FlatClassIsRoundTripped()
		{
			string json = "{\"Number\":42,\"Text\":\"Some text\"}";
			SimpleFlatClass value = new SimpleFlatClass() { Number = 42, Text = "Some text" };
			json.Should().RoundTripThrough( value );
		}

		[Fact]
		public void NestedClassIsRoundTripped()
		{
			string json = "{\"Inner\":{\"Number\":64,\"Text\":\"Another text\"}}";
			NestedClass value = new NestedClass { Inner = new SimpleFlatClass() { Number = 64, Text = "Another text" } };
			json.Should().RoundTripThrough( value );
		}

		[Fact]
		public void ValueProvidedByDefaultConstructorIsDeserializedFromEmptyJson()
		{
			string json = "{}";
			CustomDefaultValueClass value = new CustomDefaultValueClass();
			json.Should().DeserializeTo( value );
		}
	}
}
