using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HellBrick.Json.Common
{
	internal class CollectionTypeInfo
	{
		public static CollectionTypeInfo TryCreate( Type possibleCollectionType )
		{
			EnumerableTypeInfo enumerableTypeInfo = EnumerableTypeInfo.TryCreate( possibleCollectionType );
			if ( enumerableTypeInfo == null )
				return null;

			MethodInfo addMethod =
			(
				from method in possibleCollectionType.GetTypeInfo().GetDeclaredMethods( "Add" )
				let parameters = method.GetParameters()
				where parameters.Length == 1 && parameters[ 0 ].ParameterType == enumerableTypeInfo.ItemType
				select method
			)
			.FirstOrDefault();

			if ( addMethod == null )
				return null;

			return new CollectionTypeInfo( enumerableTypeInfo, addMethod );
		}

		private CollectionTypeInfo( EnumerableTypeInfo enumerableTypeInfo, MethodInfo addMethod )
		{
			EnumerableTypeInfo = enumerableTypeInfo;
			AddMethod = addMethod;
		}

		public EnumerableTypeInfo EnumerableTypeInfo { get; }
		public MethodInfo AddMethod { get; }
	}
}