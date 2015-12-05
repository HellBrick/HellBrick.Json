using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HellBrick.Json.Utils
{
	public static class TypeInfoExtensions
	{
		public static bool IsNonNullableValue( this TypeInfo typeInfo ) => typeInfo.IsValueType && !typeInfo.IsNullable();
		public static bool IsNullable( this TypeInfo typeInfo ) => typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof( Nullable<> );

		public static string ToDeclarationString( this TypeInfo typeInfo )
		{
			if ( !typeInfo.IsGenericType )
				return typeInfo.ToString();

			StringBuilder builder = new StringBuilder();
			builder.Append( typeInfo.Namespace );
			builder.Append( "." );

			int tickOffset = typeInfo.Name.IndexOf( '`' );
			builder.Append( typeInfo.Name.Substring( 0, tickOffset ) );
			builder.Append( "<" );

			bool isFirst = true;
			foreach ( Type typeArgument in typeInfo.GenericTypeArguments )
			{
				if ( !isFirst )
					builder.Append( ", " );

				builder.Append( typeArgument.GetTypeInfo().ToDeclarationString() );
				isFirst = false;
			}

			builder.Append( ">" );
			return builder.ToString();
		}
	}
}
