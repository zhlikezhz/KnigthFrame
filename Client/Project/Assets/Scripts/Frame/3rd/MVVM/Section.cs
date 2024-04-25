using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Huge.MVVM
{
    public abstract class Section : View
    {
        View m_ViewParent;
        internal void SetView(View parent)
        {
            m_ViewParent = parent;
        }

        public void Close()
        {
            Destroy();
        }

        public void CloseAndPlayAnimation()
        {
            DestroyAsync().Forget();
        }
    }
}
