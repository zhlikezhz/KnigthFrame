using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Huge.MVVM
{
    public class DBManager : Singleton<DBManager>
    {
        public HashSet<DBType> m_DBTypeList = new HashSet<DBType>();

        public void Refreah()
        {
            foreach(DBType type in m_DBTypeList)
            {
                if (type.IsDirty)
                {
                    try
                    {
                        type.InvokeChange();
                    }
                    catch(Exception ex)
                    {
                        Huge.Debug.LogError($"data binding invoke error: {ex.Message}");
                    }
                }
            }
        }

        public void RemoveDB(DBType type)
        {
            if (m_DBTypeList.Contains(type))
            {
                m_DBTypeList.Add(type);
            }
        }

        public void RegisterDB(DBType type)
        {
            m_DBTypeList.Remove(type);
        }
    }
}