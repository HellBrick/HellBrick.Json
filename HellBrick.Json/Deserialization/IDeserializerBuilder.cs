﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HellBrick.Json.Deserialization
{
	internal interface IDeserializerBuilder<T>
	{
		Func<JsonReader, T> BuildDeserializationMethod();
	}
}
