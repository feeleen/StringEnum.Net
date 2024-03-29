﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace StringEnum
{
	public abstract class StringEnum<T> : IEquatable<T>, IConvertible
		where T : StringEnum<T>
	{
		public enum EnumCase
		{
			Lower,
			Upper,
			AsIs
		}

		public string? Value { get; private set; }

		public override string? ToString() => Value;

		protected StringEnum()
		{
			if (GetType() == typeof(T))
			{
				throw new Exception($"Target type intialization not allowed for StringEnum class Fields or Properties ({this.GetType().Name})");
			}
		}

		public T HasPropertyValue<TPropType>(Expression<Func<T, TPropType>> lambda, TPropType value)
		{
			var memberExpression = (MemberExpression)lambda.Body;
			var propertyInfo = (PropertyInfo)memberExpression.Member;

			propertyInfo.SetValue(this, value, null);
			return (T)this;
		}

		protected static T New([CallerMemberName] string? value = null)
		{
			return New(EnumCase.AsIs, value);
		}

		protected static T New(EnumCase enumCase, [CallerMemberName] string? value = null)
		{
			// we don't want default base constructor to get called here, so we don't use Activator.CreateInstance()
			var objValue = (T)FormatterServices.GetUninitializedObject(typeof(T));

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
			var fields = typeof(T)
				.GetFields(BindingFlags.Public | BindingFlags.Static)
				.Where(p => p.FieldType == typeof(T))
				.Select(p => (T?)p.GetValue(null))
				.ToList();

			var props = typeof(T)
				.GetProperties(BindingFlags.Public | BindingFlags.Static)
				.Where(p => p.PropertyType == typeof(T))
				.Select(p => (T?)p.GetValue(null))
				.ToList();

			var result = new List<T?>();
			result.AddRange(props);
			result.AddRange(fields);

			return result;
		}

		public static T? Parse(string? value)
		{
			List<T?> all = AsList();

			if (!all.Any(a => string.Equals(a?.Value, value, StringComparison.OrdinalIgnoreCase)))
				throw new InvalidOperationException($"Value \"{value}\" is not a valid value for the type {typeof(T).Name}");

			return all.First(a => string.Equals(a?.Value, value, StringComparison.OrdinalIgnoreCase));
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

		#region IConvertible
		public bool ToBoolean(IFormatProvider? provider)
		{
			return Convert.ToBoolean(Value, provider);
		}

		public byte ToByte(IFormatProvider? provider)
		{
			return Convert.ToByte(Value, provider);
		}

		public char ToChar(IFormatProvider? provider)
		{
			if (Value == null)
				throw new ArgumentNullException(nameof(Value));

			return Convert.ToChar(Value, provider);
		}

		public DateTime ToDateTime(IFormatProvider? provider)
		{
			return Convert.ToDateTime(Value, provider);
		}

		public decimal ToDecimal(IFormatProvider? provider)
		{
			return Convert.ToDecimal(Value, provider);
		}

		public double ToDouble(IFormatProvider? provider)
		{
			return Convert.ToDouble(Value, provider);
		}

		public short ToInt16(IFormatProvider? provider)
		{
			return Convert.ToInt16(Value, provider);
		}

		public int ToInt32(IFormatProvider? provider)
		{
			return Convert.ToInt32(Value, provider);
		}

		public long ToInt64(IFormatProvider? provider)
		{
			return Convert.ToInt64(Value, provider);
		}

		public sbyte ToSByte(IFormatProvider? provider)
		{
			if (Value == null)
				throw new ArgumentNullException(nameof(Value));

			return Convert.ToSByte(Value, provider);
		}

		public float ToSingle(IFormatProvider? provider)
		{
			return Convert.ToSingle(Value, provider);
		}

		public string ToString(IFormatProvider? provider)
		{
			if (Value == null)
				return string.Empty;

			return Value.ToString(provider);
		}

		public object ToType(Type conversionType, IFormatProvider? provider)
		{
			if (conversionType == typeof(T))
				return (T)Parse(Value)!;

			return Convert.ChangeType(Value, conversionType, provider)!;
		}

		public ushort ToUInt16(IFormatProvider? provider)
		{
			return Convert.ToUInt16(Value, provider);
		}

		public uint ToUInt32(IFormatProvider? provider)
		{
			return Convert.ToUInt32(Value, provider);
		}

		public ulong ToUInt64(IFormatProvider? provider)
		{
			return Convert.ToUInt64(Value, provider);
		}
		#endregion

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
}
