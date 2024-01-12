using System;
 
namespace Huge.MVVM
{
    public class DBFloat : DBType, IComparable, IFormattable, IComparable<DBFloat>, IEquatable<DBFloat>
    {
        private float m_Value;
        public float value
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

        public Action<float> valueChanged;
        internal override void InvokeChange()
        {
            IsDirty = false;
            valueChanged?.Invoke(m_Value);
        }

        public DBFloat(float value)
        {
            m_Value = value;
            valueChanged = null;
        }

        public static explicit operator byte(DBFloat value)
        {
            return (byte)value.value;
        }

        public static explicit operator sbyte(DBFloat value)
        {
            return (sbyte)value.value;
        }

        public static explicit operator short(DBFloat value)
        {
            return (short)value.value;
        }

        public static explicit operator ushort(DBFloat value)
        {
            return (ushort)value.value;
        }

        public static implicit operator int(DBFloat value)
        {
            return (int)value.value;
        }

        public static explicit operator uint(DBFloat value)
        {
            return (uint)value.value;
        }

        public static implicit operator long(DBFloat value)
        {
            return (long)value.value;
        }

        public static explicit operator ulong(DBFloat value)
        {
            return (ulong)value.value;
        }

        public static implicit operator float(DBFloat value)
        {
            return value.value;
        }

        public static implicit operator double(DBFloat value)
        {
            return value.value;
        }

        public static bool operator <(DBFloat lhs, DBFloat rhs)
        {
            return lhs.m_Value < rhs.m_Value;
        }

        public static bool operator <=(DBFloat lhs, DBFloat rhs)
        {
            return lhs.m_Value <= rhs.m_Value;
        }

        public static bool operator >(DBFloat lhs, DBFloat rhs)
        {
            return lhs.m_Value > rhs.m_Value;
        }

        public static bool operator >=(DBFloat lhs, DBFloat rhs)
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

        public int CompareTo(DBFloat other)
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

        public bool Equals(DBFloat other)
        {
            return m_Value == other.m_Value;
        }
    }
}