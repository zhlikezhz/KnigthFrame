using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Huge.MVVM
{
    public abstract class ViewComponent
    {
        internal void Init(params object[] args)
        {

        }

        internal async UniTask InitAsync(params object[] args)
        {

        }

        internal async UniTask Destroy()
        {

        }
    }
}
