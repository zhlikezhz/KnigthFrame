using System;
 
namespace Huge.MVVM
{
    public class DBInt16 : DBType, IComparable, IFormattable, IComparable<DBInt16>, IEquatable<DBInt16>
    {
        private short m_Value;
        public short value
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

        public Action<DBInt16> valueChanged;
        internal override void InvokeChange()
        {
            IsDirty = false;
            valueChanged?.Invoke(this);
        }

        public DBInt16()
        {

        }

        public DBInt16(short value)
        {
            m_Value = value;
        }

        public static explicit operator byte(DBInt16 value)
        {
            return (byte)value.value;
        }

        public static explicit operator sbyte(DBInt16 value)
        {
            return (sbyte)value.value;
        }

        public static explicit operator short(DBInt16 value)
        {
            return value.value;
        }

        public static explicit operator ushort(DBInt16 value)
        {
            return (ushort)value.value;
        }

        public static implicit operator int(DBInt16 value)
        {
            return (int)value.value;
        }

        public static explicit operator uint(DBInt16 value)
        {
            return (uint)value.value;
        }

        public static implicit operator long(DBInt16 value)
        {
            return value.value;
        }

        public static explicit operator ulong(DBInt16 value)
        {
            return (ulong)value.value;
        }

        public static implicit operator float(DBInt16 value)
        {
            return value.value;
        }

        public static implicit operator double(DBInt16 value)
        {
            return value.value;
        }

        public static bool operator <(DBInt16 lhs, DBInt16 rhs)
        {
            return lhs.m_Value < rhs.m_Value;
        }

        public static bool operator <=(DBInt16 lhs, DBInt16 rhs)
        {
            return lhs.m_Value <= rhs.m_Value;
        }

        public static bool operator >(DBInt16 lhs, DBInt16 rhs)
        {
            return lhs.m_Value > rhs.m_Value;
        }

        public static bool operator >=(DBInt16 lhs, DBInt16 rhs)
        {
            return lhs.m_Value >= rhs.m_Value;
        }

        public int CompareTo(object obj)
        {
            return CompareTo(Convert.ToInt16(obj));
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

        public int CompareTo(DBInt16 other)
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

        public bool Equals(DBInt16 other)
        {
            return m_Value == other.m_Value;
        }
    }
}