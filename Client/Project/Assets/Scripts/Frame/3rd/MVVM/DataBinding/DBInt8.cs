using System;
 
namespace Huge.MVVM
{
    public class DBInt8 : DBType, IComparable, IFormattable, IComparable<DBInt8>, IEquatable<DBInt8>
    {
        private byte m_Value;
        public byte value
        {
            get { return m_Value; }
            set
            {
                if (m_Value != value)
                {
                    IsDirty = true;
                    m_Value = value;
                }
            }
        }

        public Action<byte> valueChanged;
        internal override void InvokeChange()
        {
            IsDirty = false;
            valueChanged?.Invoke(m_Value);
        }

        public DBInt8(byte value)
        {
            m_Value = value;
            valueChanged = null;
        }

        public static explicit operator byte(DBInt8 value)
        {
            return value.value;
        }

        public static explicit operator sbyte(DBInt8 value)
        {
            return (sbyte)value.value;
        }

        public static explicit operator short(DBInt8 value)
        {
            return value.value;
        }

        public static explicit operator ushort(DBInt8 value)
        {
            return (ushort)value.value;
        }

        public static implicit operator int(DBInt8 value)
        {
            return value.value;
        }

        public static explicit operator uint(DBInt8 value)
        {
            return (uint)value.value;
        }

        public static implicit operator long(DBInt8 value)
        {
            return value.value;
        }

        public static explicit operator ulong(DBInt8 value)
        {
            return (ulong)value.value;
        }

        public static implicit operator float(DBInt8 value)
        {
            return value.value;
        }

        public static implicit operator double(DBInt8 value)
        {
            return value.value;
        }

        public static bool operator <(DBInt8 lhs, DBInt8 rhs)
        {
            return lhs.m_Value < rhs.m_Value;
        }

        public static bool operator <=(DBInt8 lhs, DBInt8 rhs)
        {
            return lhs.m_Value <= rhs.m_Value;
        }

        public static bool operator >(DBInt8 lhs, DBInt8 rhs)
        {
            return lhs.m_Value > rhs.m_Value;
        }

        public static bool operator >=(DBInt8 lhs, DBInt8 rhs)
        {
            return lhs.m_Value >= rhs.m_Value;
        }

        public int CompareTo(object obj)
        {
            return CompareTo(Convert.ToByte(obj));
        }

        public override string ToString()
        {
            return m_Value.ToString();
        }

        public string ToString(string format)
        {
            return m_Value.ToString(format);
        }

        public string ToString(IFormatProvider provider)
        {
            return m_Value.ToString(provider);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            return m_Value.ToString(format, provider);
        }

        public int CompareTo(DBInt8 other)
        {
            if (m_Value == other.m_Value)
            {
                return 0;
            }
            else if (m_Value > other.m_Value)
            {
                return 1;
            }
            return -1;
        }

        public bool Equals(DBInt8 other)
        {
            return m_Value == other.m_Value;
        }
    }
}