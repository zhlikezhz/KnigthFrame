using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using Huge.Asset;

namespace Huge.MVVM
{
    public abstract class Prefab
    {
        public Transform transform;
        public GameObject gameObject;
        private bool m_bIsDestoried = false;

        protected Prefab()
        {

        }

        public void SetActive(bool actived)
        {
            gameObject.SetActive(actived);
        }

        public void SetParent(GameObject parent, bool worldPositionStays = true)
        {
            transform.SetParent(parent.transform, worldPositionStays);
        }

        internal static async UniTask<Prefab> CreateAsync(System.Type type, GameObject root = null)
        {
            var attr = type.GetAttribute<PrefabSettingAttribute>();
            if (attr == null) 
            {
                throw new Exception($"error: {type.Name}需指定预制体路径");
            }
            else
            {
                Prefab instance = Activator.CreateInstance(type) as Prefab;
                if (instance == null)
                {
                    throw new Exception($"error: {type.Name}必须继承于Prefab");
                }

                if (root == null)
                {
                    var prefab = await AssetManager.Instance.LoadAssetAsync<GameObject>(attr.PrefabPath);
                    if (prefab == null)
                    {
                        throw new Exception($"error: {type.Name}预制体不存在{attr.PrefabPath}");
                    }
                    root = GameObject.Instantiate(prefab);
                }
                instance.gameObject = root;
                instance.transform = root.transform;
                instance.OnInit();
                return instance;
            }
        }

        internal static async UniTask<T> CreateAsync<T>(GameObject root = null) where T : Prefab
        {
            var instance = await CreateAsync(typeof(T), root);
            return instance as T;
        }

        internal static Prefab Create(System.Type type, GameObject root = null)
        {
            var attr = type.GetAttribute<PrefabSettingAttribute>();
            if (attr == null) 
            {
                throw new Exception($"error: {type.Name}需指定预制体路径");
            }
            else
            {
                Prefab instance = Activator.CreateInstance(type) as Prefab;
                if (instance == null)
                {
                    throw new Exception($"error: {type.Name}必须继承于Prefab");
                }

                if (root == null)
                {
                    var prefab = AssetManager.Instance.LoadAsset<GameObject>(attr.PrefabPath);
                    if (prefab == null)
                    {
                        throw new Exception($"error: {type.Name}预制体不存在{attr.PrefabPath}");
                    }
                    root = GameObject.Instantiate(prefab);
                }
                instance.gameObject = root;
                instance.transform = root.transform;
                instance.OnInit();
                return instance;
            }
        }

        internal static T Create<T>(GameObject root = null) where T : Prefab
        {
            var instance = Create(typeof(T), root);
            return instance as T; 
        }

        internal void Destroy()
        {
            if (!m_bIsDestoried)
            {
                m_bIsDestoried = true;
                OnDestroy();
                GameObject.Destroy(gameObject);
            }
        }

        protected virtual void OnInit()
        {

        }

        protected virtual void OnDestroy()
        {

        }
    }
}
