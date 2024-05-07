using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Huge.MVVM
{
    public class ViewModel
    {
        internal protected View view;
        internal protected Prefab prefab;
        public CancellationToken Token { get; internal set; }

        internal void Destroy()
        {
            OnDestroy();
        }

        public bool IsDestroied()
        {
            return view.IsDestroied();
        }

        internal protected virtual void Start(params object[] args)
        {

        }

        internal protected virtual void OnEnable()
        {

        }

        internal protected virtual void OnDisable()
        {

        }

        internal protected virtual void OnDestroy()
        {

        }
    }
}