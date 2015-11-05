using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HellBrick.Json.DeserializationOld
{
	internal static class RelayDeserializerBuilder
	{
		public static IDeserializerBuilder<TFrom> Create<TFrom, TTo>( Func<TTo, TFrom> valueConverter ) => new RelayDeserializerBuilder<TFrom, TTo>( valueConverter );
	}

	public class RelayDeserializerBuilder<TFrom, TTo> : IDeserializerBuilder<TFrom>
	{
		private readonly Func<TTo, TFrom> _valueConverter;

		public RelayDeserializerBuilder( Func<TTo, TFrom> valueConverter )
		{
			_valueConverter = valueConverter;
		}

		public Func<JsonReader, TFrom> BuildDeserializationMethod()
		{
			JsonDeserializer<TTo> underlyingDeserializer = JsonFactory.DeserializerFor<TTo>();
			return reader => _valueConverter( underlyingDeserializer.Deserialize( reader ) );
		}
	}
}
