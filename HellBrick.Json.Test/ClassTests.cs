using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using HellBrick.Json.Test.Types;
using Newtonsoft.Json;
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
		public void TwoFlatClassesAreDeserializedCorrectly()
		{
			string json = "{\"Number\":42,\"Text\":\"Some text\"}" + Environment.NewLine + "{\"Number\":53,\"Text\":\"Another text\"}";
			SimpleFlatClass value1 = new SimpleFlatClass() { Number = 42, Text = "Some text" };
			SimpleFlatClass value2 = new SimpleFlatClass() { Number = 53, Text = "Another text" };

			using ( JsonTextReader reader = new JsonTextReader( new StringReader( json ) ) { SupportMultipleContent = true } )
			{
				SimpleFlatClass actual1 = JsonFactory.DeserializerFor<SimpleFlatClass>().Deserialize( reader );
				SimpleFlatClass actual2 = JsonFactory.DeserializerFor<SimpleFlatClass>().Deserialize( reader );
				actual1.Should().Be( value1 );
				actual2.Should().Be( value2 );
			}
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

		[Fact]
		public void DefaultValueTypeValueIsSkipped()
		{
			SimpleFlatClass value = new SimpleFlatClass() { Text = "Text", Number = default( int ) };
			value.Should().SerializeTo<SimpleFlatClass>( "{\"Text\":\"Text\"}" );
		}

		[Fact]
		public void DefaultReferenceTypeValueIsSkipped()
		{
			NestedClass value = new NestedClass() { Inner = null };
			value.Should().SerializeTo<NestedClass>( "{}" );
		}
	}
}
