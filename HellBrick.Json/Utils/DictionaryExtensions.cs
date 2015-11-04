using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HellBrick.Json.Utils
{
	public static partial class DictionaryExtensions
	{
		public static TValue GetOrDefault<TKey, TValue>( this IDictionary<TKey, TValue> dictionary, TKey key )
		{
			TValue value;
			dictionary.TryGetValue( key, out value );
			return value;
		}
	}
}
