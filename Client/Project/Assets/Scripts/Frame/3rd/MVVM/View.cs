using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Threading;
using Huge.Asset;

namespace Huge.MVVM
{
    public abstract class View
    {
        bool m_bIsActived = false;
        bool m_bIsDestroied = false;
        CancellationTokenSource m_Source;
        internal List<SubView> m_SubViewList = new List<SubView>();

        protected Transform transform;
        public GameObject gameObject;
        public CancellationToken Token { get; private set; }

        /// <summary>
        /// 同步创建
        /// /// </summary>
        /// <param name="args"></param>
        internal void Init(GameObject root)
        {
            Type t = GetType();
            ViewSettingAttribute viewSetting = t.GetAttribute<ViewSettingAttribute>();
            if (viewSetting == null)
            {
                throw new Exception("view need ViewSettingAttribute, please add ViewSettingAttribute to view");
            }
            if (string.IsNullOrEmpty(viewSetting.PrefabPath))
            {
                throw new Exception("view need set PrefabPath, please add ViewSettingAttribute to view and set ViewSettingAttribute.PrefabPath");
            }

            if (root == null)
            {
                var prefab = AssetManager.Instance.LoadAsset<GameObject>(viewSetting.PrefabPath);
                if (prefab == null)
                {
                    throw new Exception($"{t.Name} prefab does not exists {viewSetting.PrefabPath}");
                }
                root = GameObject.Instantiate(prefab);
            }
            gameObject = root;
            transform = root.transform;
            m_Source = CancellationTokenSource.CreateLinkedTokenSource(root.GetCancellationTokenOnDestroy());
            Token = m_Source.Token;
            SetParent(UIManager.Instance.GetLayerObject(GetLayerType()));

            AfterCreate();
            SetActive(true);
            if (this is IViewGenerator viewGenerator)
            {
                viewGenerator.BindGameObject(transform);
            }
            //调用用户接口,此时框架层都处理完成
            OnStart();
        }

        /// <summary>
        /// 异步创建
        /// </summary>
        /// <param name="viewInfo"></param>
        /// <returns></returns>
        internal async UniTask InitAsync(GameObject root)
        {
            Type t = GetType();
            ViewSettingAttribute viewSetting = t.GetAttribute<ViewSettingAttribute>();
            if (viewSetting == null)
            {
                throw new Exception("view need ViewSettingAttribute, please add ViewSettingAttribute to view");
            }
            if (string.IsNullOrEmpty(viewSetting.PrefabPath))
            {
                throw new Exception("view need set PrefabPath, please add ViewSettingAttribute to view and set ViewSettingAttribute.PrefabPath");
            }

            if (root == null)
            {
                var prefab = await AssetManager.Instance.LoadAssetAsync<GameObject>(viewSetting.PrefabPath);
                if (prefab == null)
                {
                    throw new Exception($"{t.Name} prefab does not exists {viewSetting.PrefabPath}");
                }
                root = GameObject.Instantiate(prefab);
            }
            gameObject = root;
            transform = root.transform;
            m_Source = CancellationTokenSource.CreateLinkedTokenSource(root.GetCancellationTokenOnDestroy());
            Token = m_Source.Token;
            SetParent(UIManager.Instance.GetLayerObject(GetLayerType()));

            AfterCreate();
            SetActive(true);
            if (this is IViewGenerator viewGenerator)
            {
                viewGenerator.BindGameObject(transform);
            }
            //调用用户接口,此时框架层都处理完成
            OnStart();

            if (this is IViewEnterAnimation view)
            {
                await view.OnPlayEnterAnimation();
            }
        }

        internal void Destroy()
        {
            if (!m_bIsDestroied)
            {
                m_bIsDestroied = true;
                foreach(SubView subView in m_SubViewList)
                {
                    subView.Destroy();
                }

                BeforeDestroy();
                SetActive(false);
                OnDestroy();
                GameObject.Destroy(gameObject);
                m_Source.Cancel();
            }
        }

        /// <summary>
        /// 异步销毁
        /// </summary>
        /// <returns></returns> <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal async UniTask DestroyAsync()
        {
            if (!m_bIsDestroied)
            {
                m_bIsDestroied = true;
                foreach(SubView subView in m_SubViewList)
                {
                    await subView.DestroyAsync();
                }

                BeforeDestroy();
                if (this is IViewExitAnimation view)
                {
                    await view.OnPlayExitAnimation();
                }

                SetActive(false);
                OnDestroy();
                GameObject.Destroy(gameObject);
                m_Source.Cancel();
            }
        }

#region internal override
        /// <summary>
        /// 在View初始化之后被调用
        /// </summary>
        /// <param name="args"></param> <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        internal virtual void AfterCreate()
        {

        }

        /// <summary>
        /// 在View销毁销毁之前被调用
        /// </summary> <summary>
        /// 
        /// </summary>
        internal virtual void BeforeDestroy()
        {

        }
#endregion

#region override
        public virtual LayerType GetLayerType()
        {
            return LayerType.NormalLayer;
        }

        protected virtual void OnStart()
        {

        }

        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {

        }

        protected virtual void OnDestroy()
        {

        }
#endregion

#region public method
        public void SetParent(View view, bool worldPositionStays = false)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }
            transform.SetParent(view.transform, worldPositionStays);
        }

        public void SetParent(GameObject parent, bool worldPositionStays = false)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            transform.SetParent(parent.transform, worldPositionStays);
        }

        public void SetParent(Transform parent, bool worldPositionStays = false)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            transform.SetParent(parent, worldPositionStays);
        }

        public void SetActive(bool isActived)
        {
            if (m_bIsActived != isActived)
            {
                m_bIsActived = isActived;
                gameObject.SetActive(isActived);
                if (isActived)
                {
                    OnEnable();
                }
                else
                {
                    OnDisable();
                }
            }
        }

        public bool IsDestroied()
        {
            return m_bIsDestroied;
        }

        public bool IsActived()
        {
            return m_bIsActived;
        }
#endregion
    }
}
