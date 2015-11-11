using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace HellBrick.Json.Test
{
	public static class CollectionAssertions
	{
		public static string ShouldRoundTripThroughUnordered<TCollection, TItem>( this string json, TCollection collection ) where TCollection : IEnumerable<TItem>
		{
			json
				.ShouldDeserializeToUnordered<TCollection, TItem>( collection )
				.ShouldSerializeTo<TCollection, TItem>( json );

			return json;
      }

		public static string ShouldRoundTripThroughOrdered<TCollection, TItem>( this string json, TCollection collection ) where TCollection : IEnumerable<TItem>
		{
			json
				.ShouldDeserializeToOrdered<TCollection, TItem>( collection )
				.ShouldSerializeTo<TCollection, TItem>( json );

			return json;
		}

		public static string ShouldSerializeTo<TCollection, TItem>( this TCollection collection, string json ) where TCollection : IEnumerable<TItem>
		{
			JsonFactory.SerializerFor<TCollection>().Serialize( collection ).Should().Be( json );
			return json;
		}

		public static TCollection ShouldDeserializeToUnordered<TCollection, TItem>( this string json, TCollection collection ) where TCollection : IEnumerable<TItem>
		{
			TCollection deserialized = JsonFactory.DeserializerFor<TCollection>().Deserialize( json );
			if ( Object.ReferenceEquals( collection, null ) )
				deserialized.Should().BeNull();
			else
				deserialized.Should().BeEquivalentTo( collection );

			return collection;
		}

		public static TCollection ShouldDeserializeToOrdered<TCollection, TItem>( this string json, TCollection collection ) where TCollection : IEnumerable<TItem>
		{
			TCollection deserialized = JsonFactory.DeserializerFor<TCollection>().Deserialize( json );

			if ( Object.ReferenceEquals( collection, null ) )
			{
				deserialized.Should().BeNull();
			}
			else
			{
				deserialized.Should()
					.HaveCount( collection.Count() )
					.And.ContainInOrder( collection );
			}

			return collection;
		}
	}
}
