using System;

namespace Huge.MVVM
{
    public class DBString : DBType
    {
        string m_Value;

        public string value
        {
            internal get { return m_Value; }
            set
            {
                if (m_Value != value)
                {
                    IsDirty = true;
                    m_Value = value;
                }
            }
        }

        public Action<DBString> valueChanged;
        internal override void InvokeChange()
        {
            IsDirty = false;
            valueChanged?.Invoke(this);
        }
    }
}