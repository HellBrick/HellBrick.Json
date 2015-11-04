using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HellBrick.Json.Common
{
	internal class EnumerableTypeInfo
	{
		public static EnumerableTypeInfo TryCreate( Type possibleCollectionType )
		{
			TypeInfo collectionTypeInfo = possibleCollectionType.GetTypeInfo();

			MethodInfo getEnumeratorMethod = collectionTypeInfo.DeclaredMethods
				.Where( m => m.IsPublic )
				.Where( m => m.GetParameters().Length == 0 )
				.Where( m => m.Name == "GetEnumerator" )
				.FirstOrDefault();

			if ( getEnumeratorMethod == null )
				return null;

			TypeInfo enumeratorTypeInfo = getEnumeratorMethod.ReturnType.GetTypeInfo();
			PropertyInfo currentProperty = enumeratorTypeInfo.GetDeclaredProperty( "Current" );

			MethodInfo moveNextMethod = enumeratorTypeInfo.DeclaredMethods
				.Where( m => m.IsPublic )
				.Where( m => m.GetParameters().Length == 0 )
				.Where( m => m.Name == "MoveNext" )
				.First();

			return new EnumerableTypeInfo( possibleCollectionType, currentProperty.PropertyType, getEnumeratorMethod, moveNextMethod, currentProperty );
		}

		private EnumerableTypeInfo( Type collectionType, Type itemType, MethodInfo getEnumeratorMethod, MethodInfo moveNextMethod, PropertyInfo currentProperty )
		{
			CollectionType = collectionType;
			ItemType = itemType;
			GetEnumeratorMethod = getEnumeratorMethod;
			MoveNextMethod = moveNextMethod;
			CurrentProperty = currentProperty;
		}

		public Type CollectionType { get; }
		public Type ItemType { get; }
		public MethodInfo GetEnumeratorMethod { get; }
		public MethodInfo MoveNextMethod { get; }
		public PropertyInfo CurrentProperty { get; }
	}
}
