using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Huge.MVVM
{
    public class DBType : IDisposable
    {
        public DBType()
        {
            DataBindingManager.Instance.RegisterDB(this);
        }

        bool m_bIsDispose = false;
        public void Dispose()
        {
            if (m_bIsDispose == false)
            {
                m_bIsDispose = true;
                DataBindingManager.Instance.RemoveDB(this);
            }
        }

        public bool IsDirty {get; protected set;}
        internal virtual void InvokeChange()
        {

        }
    }
}