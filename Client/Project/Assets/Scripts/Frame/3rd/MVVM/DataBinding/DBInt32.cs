using System;
 
namespace Huge.MVVM
{
    public class DBInt32 : DBType, IComparable, IFormattable, IComparable<DBInt32>, IEquatable<DBInt32>
    {
        private int m_Value;
        public int value
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

        public Action<int> valueChanged;
        internal override void InvokeChange()
        {
            IsDirty = false;
            valueChanged?.Invoke(m_Value);
        }

        public DBInt32(int value)
        {
            m_Value = value;
            valueChanged = null;
        }

        public static explicit operator byte(DBInt32 value)
        {
            return (byte)value.value;
        }

        public static explicit operator sbyte(DBInt32 value)
        {
            return (sbyte)value.value;
        }

        public static explicit operator short(DBInt32 value)
        {
            return (short)value.value;
        }

        public static explicit operator ushort(DBInt32 value)
        {
            return (ushort)value.value;
        }

        public static implicit operator int(DBInt32 value)
        {
            return value.value;
        }

        public static explicit operator uint(DBInt32 value)
        {
            return (uint)value.value;
        }

        public static implicit operator long(DBInt32 value)
        {
            return value.value;
        }

        public static explicit operator ulong(DBInt32 value)
        {
            return (ulong)value.value;
        }

        public static implicit operator float(DBInt32 value)
        {
            return value.value;
        }

        public static implicit operator double(DBInt32 value)
        {
            return value.value;
        }

        public static bool operator <(DBInt32 lhs, DBInt32 rhs)
        {
            return lhs.m_Value < rhs.m_Value;
        }

        public static bool operator <=(DBInt32 lhs, DBInt32 rhs)
        {
            return lhs.m_Value <= rhs.m_Value;
        }

        public static bool operator >(DBInt32 lhs, DBInt32 rhs)
        {
            return lhs.m_Value > rhs.m_Value;
        }

        public static bool operator >=(DBInt32 lhs, DBInt32 rhs)
        {
            return lhs.m_Value >= rhs.m_Value;
        }

        public int CompareTo(object obj)
        {
            return CompareTo(Convert.ToInt32(obj));
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

        public int CompareTo(DBInt32 other)
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

        public bool Equals(DBInt32 other)
        {
            return m_Value == other.m_Value;
        }
    }
}