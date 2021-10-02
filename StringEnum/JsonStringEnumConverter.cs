using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StringEnum
{
	public class JsonStringEnumConverter<T> : Newtonsoft.Json.JsonConverter
			where T : StringEnum<T>
	{
		public override bool CanRead => true;

		public override bool CanWrite => true;

		public override bool CanConvert(Type objectType) => ImplementsGeneric(objectType, typeof(StringEnum<>));

		private static bool ImplementsGeneric(Type? type, Type generic)
		{
			while (type != null)
			{
				if (type.IsGenericType && type.GetGenericTypeDefinition() == generic)
					return true;

				type = type.BaseType;
			}

			return false;
		}

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
		{
			JToken item = JToken.Load(reader);
			string? value = item.Value<string>();
			return StringEnum<T>.Parse(value);
		}

		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
		{
			if (value is StringEnum<T> v)
			{
				if (v.Value is null)
					JValue.CreateNull().WriteTo(writer);
				else
					JToken.FromObject(v.Value).WriteTo(writer);
			}
		}
	}
}
