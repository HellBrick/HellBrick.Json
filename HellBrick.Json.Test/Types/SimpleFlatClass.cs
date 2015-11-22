using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellBrick.Json.Test.Types
{
	internal class SimpleFlatClass : IEquatable<SimpleFlatClass>
	{
		public int Number { get; set; }
		public string Text { get; set; }

		#region IEquatable<SimpleFlatClass>

		public override int GetHashCode()
		{
			unchecked
			{
				const int prime = -1521134295;
				int hash = 12345701;
				hash = hash * prime + EqualityComparer<int>.Default.GetHashCode( Number );
				hash = hash * prime + EqualityComparer<string>.Default.GetHashCode( Text );
				return hash;
			}
		}

		public bool Equals( SimpleFlatClass other ) => !Object.ReferenceEquals( other, null ) && EqualityComparer<int>.Default.Equals( Number, other.Number ) && Text == other.Text;
		public override bool Equals( object obj ) => Equals( obj as SimpleFlatClass );

		public static bool operator ==( SimpleFlatClass x, SimpleFlatClass y ) =>
			Object.ReferenceEquals( x, y ) ||
			!Object.ReferenceEquals( x, null ) && x.Equals( y );

		public static bool operator !=( SimpleFlatClass x, SimpleFlatClass y ) => !( x == y );

		#endregion
	}
}
