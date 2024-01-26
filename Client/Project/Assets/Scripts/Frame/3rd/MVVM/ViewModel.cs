using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class ViewModel
{
    internal void Destroy()
    {
        OnDestroy();
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
