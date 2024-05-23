using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Huge.MVVM
{
    public abstract class SubView : View
    {
        View m_viewParent;
        internal void SetView(View window) 
        {
            m_viewParent = window;
        }

        public void Close()
        {
            m_viewParent?.RemoveSubView(this);
        }
    }
}
