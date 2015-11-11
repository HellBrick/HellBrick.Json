using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace HellBrick.Json.Test.CollectionTests
{
	public class ArrayTests
	{
		[Fact]
		public void Empty()
		{
			int[] array = Array.Empty<int>();
			string json = "[]";
			json.ShouldRoundTripThroughOrdered<int[], int>( array );
		}

		[Fact]
		public void NonEmpty()
		{
			int[] array = { 42, 256, 128 };
			string json = "[42,256,128]";
			json.ShouldRoundTripThroughOrdered<int[], int>( array );
		}

		[Fact]
		public void Null()
		{
			int[] array = null;
			string json = "null";
			json.ShouldRoundTripThroughOrdered<int[], int>( array );
		}
	}

	public class ListTests
	{
		[Fact]
		public void Empty()
		{
			List<int> list = new List<int>();
			string json = "[]";
			json.ShouldRoundTripThroughOrdered<List<int>, int>( list );
		}

		[Fact]
		public void NonEmpty()
		{
			List<int> list = new List<int>() { 42, 256, 128 };
			string json = "[42,256,128]";
			json.ShouldRoundTripThroughOrdered<List<int>, int>( list );
		}

		[Fact]
		public void Null()
		{
			List<int> list = null;
			string json = "null";
			json.ShouldRoundTripThroughOrdered<List<int>, int>( list );
		}
	}

	public class HashSetTests
	{
		[Fact]
		public void Empty()
		{
			HashSet<int> set = new HashSet<int>();
			string json = "[]";
			json.ShouldRoundTripThroughUnordered<HashSet<int>, int>( set );
		}

		[Fact]
		public void NonEmpty()
		{
			HashSet<int> set = new HashSet<int>() { 42, 256, 128 };
			string json = "[42,256,128]";
			json.ShouldRoundTripThroughUnordered<HashSet<int>, int>( set );
		}

		[Fact]
		public void Null()
		{
			HashSet<int> set = null;
			string json = "null";
			json.ShouldRoundTripThroughUnordered<HashSet<int>, int>( set );
		}

		[Fact]
		public void DuplicatesAreDiscardedOnDeserialization()
		{
			string json = "[42,256,128,42,42]";
			HashSet<int> set = new HashSet<int>() { 42, 256, 128 };
			json.ShouldDeserializeToUnordered<HashSet<int>, int>( set );
		}
	}
}
