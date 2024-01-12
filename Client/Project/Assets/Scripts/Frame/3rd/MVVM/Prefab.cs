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

        internal async static UniTask<Prefab> CreateAsync(System.Type type, GameObject root = null)
        {
            Prefab instance = Activator.CreateInstance(type) as Prefab;
            await instance.OnInitAsync(root);
            return instance;
        }

        internal async UniTask DestroyAsync()
        {
            await OnDestroyAsync();
        }

        protected virtual async UniTask OnInitAsync(GameObject root = null)
        {
            await UniTask.DelayFrame(1);
            throw new NotImplementedException("need implement Prefab.InitAsync method");
        }

        protected virtual async UniTask OnDestroyAsync()
        {
            await UniTask.DelayFrame(1);
            throw new NotImplementedException("need implement Prefab.DestroyAsync method");
        }
    }
}
