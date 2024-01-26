using System;
using Huge.Pool;
using UnityEngine;
using UnityEngine.Rendering;

namespace Huge.MVVM
{
    public class DBType : IDisposable
    {
        public DBType()
        {
            DBManager.Instance.RegisterDB(this);
        }

        bool m_bIsDispose = false;
        public void Dispose()
        {
            if (m_bIsDispose == false)
            {
                m_bIsDispose = true;
                DBManager.Instance.RemoveDB(this);
            }
        }

        public bool IsDirty {get; protected set;}
        internal virtual void InvokeChange()
        {

        }
    }
}