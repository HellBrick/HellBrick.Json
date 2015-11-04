using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HellBrick.Json.Utils;
using Newtonsoft.Json;

namespace HellBrick.Json.Common
{
	internal static class JsonReaderMembers
	{
		public static readonly PropertyInfo TokenType = Reflection.Property( ( JsonReader r ) => r.TokenType );
	}
}
