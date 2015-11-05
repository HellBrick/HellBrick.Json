using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HellBrick.Json.Deserialization;
using HellBrick.Json.Serialization;

namespace HellBrick.Json
{
	public static class JsonFactory
	{
		private static readonly Cache _deserializerCache = new Cache();
		private static readonly Cache _serializerCache = new Cache();

		public static JsonDeserializer<T> DeserializerFor<T>() => _deserializerCache.GetOrCreate( typeof( T ), () => DeserializerFactory.CreateDeserializer<T>() );
		public static JsonSerializer<T> SerializerFor<T>() => _serializerCache.GetOrCreate( typeof( T ), () => SerializerFactory.CreateSerializer<T>() );

		private class Cache
		{
			private readonly ConcurrentDictionary<Type, object> _itemHolders = new ConcurrentDictionary<Type, object>();

			public TItem GetOrCreate<TItem>( Type type, Func<TItem> itemFactory ) where TItem : class
			{
				Lazy<TItem> holder = _itemHolders.GetOrAdd( type, _ => new Lazy<TItem>( itemFactory, LazyThreadSafetyMode.ExecutionAndPublication ) ) as Lazy<TItem>;
				return holder.Value;
			}
		}
	}
}
