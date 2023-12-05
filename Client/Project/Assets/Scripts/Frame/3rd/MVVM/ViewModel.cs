using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class ViewModel
{
    public async static UniTask<ViewModel> CreateAsync(System.Type type)
    {
        ViewModel instance = Activator.CreateInstance(type) as ViewModel;
        await instance.InitAsync();
        return instance;
    }

    public virtual async UniTask<UniTaskVoid> InitAsync()
    {
        await UniTask.DelayFrame(1);
        throw new NotImplementedException("need implement Prefab.InitAsync method");
    }

    public virtual async UniTask<UniTaskVoid> DestroyAsync()
    {
        await UniTask.DelayFrame(1);
        throw new NotImplementedException("need implement Prefab.DestroyAsync method");
    }
}
