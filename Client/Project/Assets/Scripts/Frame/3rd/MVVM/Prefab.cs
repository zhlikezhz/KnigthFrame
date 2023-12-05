using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Huge.MVVM
{
    public abstract class Prefab    
    {
        public Transform transform;
        public GameObject gameObject;

        public void SetActive(bool actived)
        {
            gameObject.SetActive(actived);
        }

        public void SetParent(GameObject parent, bool worldPositionStays = true)
        {
            transform.SetParent(parent.transform, worldPositionStays);
        }

        public async static UniTask<Prefab> CreateAsync(System.Type type, GameObject root = null)
        {
            Prefab instance = Activator.CreateInstance(type) as Prefab;
            await instance.InitAsync(root);
            return instance;
        }

        public virtual async UniTask InitAsync(GameObject root = null)
        {
            await UniTask.DelayFrame(1);
            throw new NotImplementedException("need implement Prefab.InitAsync method");
        }

        public virtual async UniTask DestroyAsync()
        {
            await UniTask.DelayFrame(1);
            throw new NotImplementedException("need implement Prefab.DestroyAsync method");
        }
    }
}
