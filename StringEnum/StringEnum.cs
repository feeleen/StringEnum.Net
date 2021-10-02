using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace StringEnum
{
	public abstract class StringEnum<T> : IEquatable<T>//, IConvertible
		where T : StringEnum<T>
	{
		public enum EnumCase
		{
			Lower,
			Upper,
			AsIs
		}

		public string? Value { get; protected set; }

		public override string? ToString() => Value;

		protected static T New([CallerMemberName] string? value = null)
		{
			return New(EnumCase.AsIs, value);
		}

		protected static T New(EnumCase enumCase, [CallerMemberName] string? value = null)
		{
			var objValue = Activator.CreateInstance<T>();

			if (value == null)
			{
				
				objValue.Value = null;
				return objValue;
			}

			switch (enumCase)
			{
				case EnumCase.AsIs:
					objValue.Value = value;
					break;

				case EnumCase.Lower:
					objValue.Value = value.ToLower();
					break;

				case EnumCase.Upper:
					objValue.Value = value.ToUpper();
					break;
			}

			return objValue;
		}

		public static List<T?> AsList()
		{
			return typeof(T)
				.GetProperties(BindingFlags.Public | BindingFlags.Static)
				.Where(p => p.PropertyType == typeof(T))
				.Select(p => (T?)p.GetValue(null))
				.ToList();
		}

		public static T? Parse(string? value)
		{
			List<T?> all = AsList();

			if (!all.Any(a => string.Equals(a?.Value, value, StringComparison.OrdinalIgnoreCase)))
				throw new InvalidOperationException($"Value \"{value}\" is not a valid value for the type {typeof(T).Name}");

			return all.Single(a => string.Equals(a?.Value, value, StringComparison.OrdinalIgnoreCase));
		}


		#region IEquatable
		public bool Equals(T? other)
		{
			if (other is null) 
				return false;

			return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
		}

		public override bool Equals(object? obj)
		{
			if (obj == null) return false;
			if (obj is T other) return this.Equals(other);
			return false;
		}

		public override int GetHashCode() => Value == null ? 0 : Value.ToLower().GetHashCode();

		public TypeCode GetTypeCode()
		{
			return GetTypeCode();
		}
		#endregion

		//#region IConvertible
		//public bool ToBoolean(IFormatProvider provider)
		//{
		//	return ((IConvertible)Value).ToBoolean(provider);
		//}

		//public byte ToByte(IFormatProvider provider)
		//{
		//	return ((IConvertible)Value).ToByte(provider);
		//}

		//public char ToChar(IFormatProvider provider)
		//{
		//	return ((IConvertible)Value).ToChar(provider);
		//}

		//public DateTime ToDateTime(IFormatProvider provider)
		//{
		//	return ((IConvertible)Value).ToDateTime(provider);
		//}

		//public decimal ToDecimal(IFormatProvider provider)
		//{
		//	return ((IConvertible)Value).ToDecimal(provider);
		//}

		//public double ToDouble(IFormatProvider provider)
		//{
		//	return ((IConvertible)Value).ToDouble(provider);
		//}

		//public short ToInt16(IFormatProvider provider)
		//{
		//	return ((IConvertible)Value).ToInt16(provider);
		//}

		//public int ToInt32(IFormatProvider provider)
		//{
		//	return ((IConvertible)Value).ToInt32(provider);
		//}

		//public long ToInt64(IFormatProvider provider)
		//{
		//	return ((IConvertible)Value).ToInt64(provider);
		//}

		//public sbyte ToSByte(IFormatProvider provider)
		//{
		//	return ((IConvertible)Value).ToSByte(provider);
		//}

		//public float ToSingle(IFormatProvider provider)
		//{
		//	return ((IConvertible)Value).ToSingle(provider);
		//}

		//public string ToString(IFormatProvider provider)
		//{
		//	return Value?.ToString(provider);
		//}

		//public object ToType(Type conversionType, IFormatProvider provider)
		//{
		//	return ((IConvertible)Value).ToType(conversionType, provider);
		//}

		//public ushort ToUInt16(IFormatProvider provider)
		//{
		//	return ((IConvertible)Value).ToUInt16(provider);
		//}

		//public uint ToUInt32(IFormatProvider provider)
		//{
		//	return ((IConvertible)Value).ToUInt32(provider);
		//}

		//public ulong ToUInt64(IFormatProvider provider)
		//{
		//	return ((IConvertible)Value).ToUInt64(provider);
		//}
		//#endregion

		public static bool operator ==(StringEnum<T> a, StringEnum<T> b) => a?.Equals(b) ?? false;

		public static bool operator !=(StringEnum<T> a, StringEnum<T> b) => !(a?.Equals(b) ?? false);

		/// <summary>
		/// For: string cat = MyPet.Cat;
		/// </summary>
		/// <param name="input"></param>
		public static implicit operator string?(StringEnum<T> input)
		{
			return input?.Value;
		}

		/// <summary>
		/// For: MyPet cat = (MyPet)"Cat";
		/// </summary>
		/// <param name="input"></param>
		public static implicit operator StringEnum<T>?(string? input)
        {
            return (T?) Parse(input);
        }

		public static implicit operator StringEnum<T>?(T? input)
		{
			return (T?) Parse(input?.Value);
		}
	}


	public class StringEnumConverter<T> : TypeConverter
		where T: StringEnum<T>
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		public override object? ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			var name = value as string;
			if (name != null)
				return (T?) name;
			else
				return StringEnum<T>.Parse((string?)value);
		}
	}
}
