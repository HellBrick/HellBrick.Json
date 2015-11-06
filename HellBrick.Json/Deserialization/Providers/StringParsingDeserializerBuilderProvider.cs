using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Utils;
using static System.Linq.Expressions.Expression;

namespace HellBrick.Json.Deserialization.Providers
{
	internal class StringParsingDeserializerBuilderProvider : IDeserializerBuilderProvider
	{
		private static readonly Dictionary<Type, object> _builderLookup =
			(
				from nestedType in typeof( StringParsingDeserializerBuilderProvider ).GetTypeInfo().DeclaredNestedTypes
				where !nestedType.IsAbstract
				from @interface in nestedType.ImplementedInterfaces
				let interfaceInfo = @interface.GetTypeInfo()
				where interfaceInfo.IsGenericType
				where interfaceInfo.GetGenericTypeDefinition() == typeof( IDeserializerBuilder<> )
				let builderTypeArgument = interfaceInfo.GenericTypeArguments[ 0 ]
				select new
				{
					Builder = Activator.CreateInstance( nestedType.AsType() ),
					Type = builderTypeArgument
				}
			)
			.ToDictionary( i => i.Type, i => i.Builder );


		public IDeserializerBuilder<T> TryCreateBuilder<T>() => _builderLookup.GetOrDefault( typeof( T ) ) as IDeserializerBuilder<T>;
	}
}
