using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Huge.MVVM
{
    public abstract class Window : View
    {
        public void Close()
        {
            UIManager.Instance.CloseWindow(this);
        }

        public async UniTask CloseAndPlayAnimation()
        {
            await UIManager.Instance.CloseWindowAsync(this);
        }

        internal protected virtual bool IsUseMask()
        {
            return true;
        }

        internal protected virtual float GetMaskAlpha()
        {
            return 1.0f;
        }

        internal protected virtual bool IsClickMask()
        {
            return true;
        }

        internal protected virtual void OnClickMask()
        {
            Close();
        }
    }
}