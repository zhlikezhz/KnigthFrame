using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Huge.MVVM
{
    public abstract class SubView : View
    {
        Window m_winParent;
        internal void SetWindow(Window window) 
        {
            m_winParent = window;
        }

        public void Close()
        {
            m_winParent?.RemoveSubView(this);
        }
    }
}
