using System;
 
namespace Huge.MVVM
{
    public class DBDouble : DBType, IComparable, IFormattable, IComparable<DBDouble>, IEquatable<DBDouble>
    {
        double m_Value;
        public double value
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

        public Action<double> valueChanged;
        internal override void InvokeChange()
        {
            IsDirty = false;
            valueChanged?.Invoke(m_Value);
        }

        public DBDouble(double value)
        {
            m_Value = value;
            valueChanged = null;
        }

        public static explicit operator byte(DBDouble value)
        {
            return (byte)value.value;
        }

        public static explicit operator sbyte(DBDouble value)
        {
            return (sbyte)value.value;
        }

        public static explicit operator short(DBDouble value)
        {
            return (short)value.value;
        }

        public static explicit operator ushort(DBDouble value)
        {
            return (ushort)value.value;
        }

        public static implicit operator int(DBDouble value)
        {
            return (int)value.value;
        }

        public static explicit operator uint(DBDouble value)
        {
            return (uint)value.value;
        }

        public static implicit operator long(DBDouble value)
        {
            return (long)value.value;
        }

        public static explicit operator ulong(DBDouble value)
        {
            return (ulong)value.value;
        }

        public static implicit operator float(DBDouble value)
        {
            return (float)value.value;
        }

        public static implicit operator double(DBDouble value)
        {
            return value.value;
        }

        public static bool operator <(DBDouble lhs, DBDouble rhs)
        {
            return lhs.m_Value < rhs.m_Value;
        }

        public static bool operator <=(DBDouble lhs, DBDouble rhs)
        {
            return lhs.m_Value <= rhs.m_Value;
        }

        public static bool operator >(DBDouble lhs, DBDouble rhs)
        {
            return lhs.m_Value > rhs.m_Value;
        }

        public static bool operator >=(DBDouble lhs, DBDouble rhs)
        {
            return lhs.m_Value >= rhs.m_Value;
        }

        public int CompareTo(object obj)
        {
            return CompareTo(Convert.ToDouble(obj));
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

        public int CompareTo(DBDouble other)
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

        public bool Equals(DBDouble other)
        {
            return m_Value == other.m_Value;
        }
    }
}