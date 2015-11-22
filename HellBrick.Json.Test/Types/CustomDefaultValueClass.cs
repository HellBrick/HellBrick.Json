using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellBrick.Json.Test.Types
{
	public class CustomDefaultValueClass : IEquatable<CustomDefaultValueClass>
	{
		public int Value { get; set; } = 42;

		#region IEquatable<NestedClass>

		public override int GetHashCode()
		{
			unchecked
			{
				const int prime = -1521134295;
				int hash = 12345701;
				hash = hash * prime + EqualityComparer<int>.Default.GetHashCode( Value );
				return hash;
			}
		}

		public bool Equals( CustomDefaultValueClass other ) => !Object.ReferenceEquals( other, null ) && EqualityComparer<int>.Default.Equals( Value, other.Value );
		public override bool Equals( object obj ) => Equals( obj as CustomDefaultValueClass );

		public static bool operator ==( CustomDefaultValueClass x, CustomDefaultValueClass y ) =>
			Object.ReferenceEquals( x, y ) ||
			!Object.ReferenceEquals( x, null ) && x.Equals( y );

		public static bool operator !=( CustomDefaultValueClass x, CustomDefaultValueClass y ) => !( x == y );

		#endregion
	}
}
