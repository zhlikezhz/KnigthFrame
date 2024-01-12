using System;
 
namespace Huge.MVVM
{
    public class DBBool : DBType, IComparable, IComparable<DBBool>, IEquatable<DBBool>
    {
        bool m_Value;
        public bool value
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

        public Action<bool> valueChanged;
        internal override void InvokeChange()
        {
            IsDirty = false;
            valueChanged?.Invoke(m_Value);
        }

        public DBBool(bool value)
        {
            m_Value = value;
            valueChanged = null;
        }

        static int GetInt(DBBool value)
        {
            if (value.value == false)
            {
                return 0;
            }
            return 1;
        }

        public static explicit operator byte(DBBool value)
        {
            return (byte)GetInt(value);
        }

        public static explicit operator sbyte(DBBool value)
        {
            return (sbyte)GetInt(value);
        }

        public static explicit operator short(DBBool value)
        {
            return (short)GetInt(value);
        }

        public static explicit operator ushort(DBBool value)
        {
            return (ushort)GetInt(value);
        }

        public static implicit operator int(DBBool value)
        {
            return GetInt(value);
        }

        public static explicit operator uint(DBBool value)
        {
            return (uint)GetInt(value);
        }

        public static implicit operator long(DBBool value)
        {
            return GetInt(value);
        }

        public static explicit operator ulong(DBBool value)
        {
            return (ulong)GetInt(value);
        }

        public static implicit operator float(DBBool value)
        {
            return GetInt(value);
        }

        public static implicit operator double(DBBool value)
        {
            return GetInt(value);
        }

        public int CompareTo(object obj)
        {
            return CompareTo(Convert.ToBoolean(obj));
        }

        public int CompareTo(DBBool other)
        {
            int value = GetInt(this);
            int othervalue = GetInt(other);
            if (value == othervalue)
            {
                return 0;
            }
            else if (value > othervalue)
            {
                return 1;
            }
            return -1;
        }

        public bool Equals(DBBool other)
        {
            return m_Value == other.m_Value;
        }
    }
}