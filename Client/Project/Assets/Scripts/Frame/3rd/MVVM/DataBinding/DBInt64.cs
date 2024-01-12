using System;
 
namespace Huge.MVVM
{
    public class DBInt64 : DBType, IComparable, IFormattable, IComparable<DBInt64>, IEquatable<DBInt64>
    {
        private long m_Value;
        public long value
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

        public Action<long> valueChanged;
        internal override void InvokeChange()
        {
            IsDirty = false;
            valueChanged?.Invoke(m_Value);
        }

        public DBInt64(long value)
        {
            m_Value = value;
            valueChanged = null;
        }

        public static explicit operator byte(DBInt64 value)
        {
            return (byte)value.value;
        }

        public static explicit operator sbyte(DBInt64 value)
        {
            return (sbyte)value.value;
        }

        public static explicit operator short(DBInt64 value)
        {
            return (short)value.value;
        }

        public static explicit operator ushort(DBInt64 value)
        {
            return (ushort)value.value;
        }

        public static implicit operator int(DBInt64 value)
        {
            return (int)value.value;
        }

        public static explicit operator uint(DBInt64 value)
        {
            return (uint)value.value;
        }

        public static implicit operator long(DBInt64 value)
        {
            return value.value;
        }

        public static explicit operator ulong(DBInt64 value)
        {
            return (ulong)value.value;
        }

        public static implicit operator float(DBInt64 value)
        {
            return value.value;
        }

        public static implicit operator double(DBInt64 value)
        {
            return value.value;
        }

        public static bool operator <(DBInt64 lhs, DBInt64 rhs)
        {
            return lhs.m_Value < rhs.m_Value;
        }

        public static bool operator <=(DBInt64 lhs, DBInt64 rhs)
        {
            return lhs.m_Value <= rhs.m_Value;
        }

        public static bool operator >(DBInt64 lhs, DBInt64 rhs)
        {
            return lhs.m_Value > rhs.m_Value;
        }

        public static bool operator >=(DBInt64 lhs, DBInt64 rhs)
        {
            return lhs.m_Value >= rhs.m_Value;
        }

        public int CompareTo(object obj)
        {
            return CompareTo(Convert.ToInt64(obj));
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

        public int CompareTo(DBInt64 other)
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

        public bool Equals(DBInt64 other)
        {
            return m_Value == other.m_Value;
        }
    }
}