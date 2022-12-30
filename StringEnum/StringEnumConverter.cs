using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringEnum
{
	public class StringEnumConverter<T> : TypeConverter
		where T : StringEnum<T>
	{
		public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
		{
			var name = value as string;
			if (name != null)
				return (T?)name;
			else
				return StringEnum<T>.Parse((string?)value);
		}
	}
}
