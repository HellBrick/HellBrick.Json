using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellBrick.Json.Test.Types
{
	internal class NestedClass : IEquatable<NestedClass>
	{
		public SimpleFlatClass Inner { get; set; }

		#region IEquatable<NestedClass>

		public override int GetHashCode()
		{
			unchecked
			{
				const int prime = -1521134295;
				int hash = 12345701;
				hash = hash * prime + EqualityComparer<SimpleFlatClass>.Default.GetHashCode( Inner );
				return hash;
			}
		}

		public bool Equals( NestedClass other ) => !Object.ReferenceEquals( other, null ) && EqualityComparer<SimpleFlatClass>.Default.Equals( Inner, other.Inner );
		public override bool Equals( object obj ) => Equals( obj as NestedClass );

		public static bool operator ==( NestedClass x, NestedClass y ) =>
			Object.ReferenceEquals( x, y ) ||
			!Object.ReferenceEquals( x, null ) && x.Equals( y );

		public static bool operator !=( NestedClass x, NestedClass y ) => !( x == y );

		#endregion
	}
}
