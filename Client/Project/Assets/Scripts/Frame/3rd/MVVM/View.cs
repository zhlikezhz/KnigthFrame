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
    public abstract partial class View
    {
        LayerType m_LayerType;
        WindowType m_WindowType;
        bool m_bIsActived = false;
        bool m_bIsDestroied = false;
        CancellationTokenSource m_Source;
        internal List<SubView> m_SubViewList = new List<SubView>();

        protected GameObject gameObject;
        protected RectTransform transform;
        public CancellationToken Token { get; private set; }

        /// <summary>
        /// 同步创建
        /// /// </summary>
        /// <param name="args"></param>
        internal void Init(GameObject root, WindowType windowType = WindowType.None, LayerType layerType = LayerType.None)
        {
            m_LayerType = layerType;
            m_WindowType = windowType;

            Type t = GetType();
            if (root == null)
            {
                ViewSettingAttribute viewSetting = t.GetAttribute<ViewSettingAttribute>();
                if (viewSetting == null)
                {
                    throw new Exception("view need ViewSettingAttribute, please add ViewSettingAttribute to view");
                }
                if (string.IsNullOrEmpty(viewSetting.PrefabPath))
                {
                    throw new Exception("view need set PrefabPath, please add ViewSettingAttribute to view and set ViewSettingAttribute.PrefabPath");
                }
                var prefab = AssetManager.Instance.LoadAsset<GameObject>(viewSetting.PrefabPath);
                if (prefab == null)
                {
                    throw new Exception($"{t.Name} prefab does not exists {viewSetting.PrefabPath}");
                }
                root = GameObject.Instantiate(prefab);
            }
            gameObject = root;
            transform = root.transform as RectTransform;
            m_Source = CancellationTokenSource.CreateLinkedTokenSource(root.GetCancellationTokenOnDestroy());
            Token = m_Source.Token;
            if (windowType != WindowType.None && layerType != LayerType.None)
            {
                SetParent(UIManager.Instance.GetLayerObject(GetLayerType()));
            }

            AfterCreate();
            SetActive(true);
            //调用用户接口,此时框架层都处理完成
            OnGenerate();
            OnStart();
        }

        /// <summary>
        /// 异步创建
        /// </summary>
        /// <param name="viewInfo"></param>
        /// <returns></returns>
        internal async UniTask InitAsync(GameObject root, WindowType windowType = WindowType.None, LayerType layerType = LayerType.None)
        {
            m_LayerType = layerType;
            m_WindowType = windowType;

            Type t = GetType();
            if (root == null)
            {
                ViewSettingAttribute viewSetting = t.GetAttribute<ViewSettingAttribute>();
                if (viewSetting == null)
                {
                    throw new Exception("view need ViewSettingAttribute, please add ViewSettingAttribute to view");
                }
                if (string.IsNullOrEmpty(viewSetting.PrefabPath))
                {
                    throw new Exception("view need set PrefabPath, please add ViewSettingAttribute to view and set ViewSettingAttribute.PrefabPath");
                }
                var prefab = await AssetManager.Instance.LoadAssetAsync<GameObject>(viewSetting.PrefabPath);
                if (prefab == null)
                {
                    throw new Exception($"{t.Name} prefab does not exists {viewSetting.PrefabPath}");
                }
                root = GameObject.Instantiate(prefab);
            }
            gameObject = root;
            transform = root.transform as RectTransform;
            m_Source = CancellationTokenSource.CreateLinkedTokenSource(root.GetCancellationTokenOnDestroy());
            Token = m_Source.Token;
            if (windowType != WindowType.None && layerType != LayerType.None)
            {
                SetParent(UIManager.Instance.GetLayerObject(GetLayerType()));
            }

            AfterCreate();
            SetActive(true);
            //调用用户接口,此时框架层都处理完成
            OnGenerate();
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

        /// <summary>
        /// 在View初始化之后被调用
        /// </summary>
        protected virtual void AfterCreate()
        {
            if (this is Window window)
            {
                if(m_WindowType == WindowType.Page)
                {
                    Page.AfterCreate(window);
                }
                else if (m_WindowType == WindowType.Popup)
                {
                    Popup.AfterCreate(window);
                }
            }
        }

        /// <summary>
        /// 在View销毁销毁之前被调用
        /// </summary> 
        protected virtual void BeforeDestroy()
        {
            if (this is Window window)
            {
                if(m_WindowType == WindowType.Page)
                {
                    Page.BeforeDestroy(window);
                }
                else if (m_WindowType == WindowType.Popup)
                {
                    Popup.BeforeDestroy(window);
                }
            }
        }

#region override
        protected virtual void OnGenerate()
        {

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

        public LayerType GetLayerType()
        {
            return m_LayerType;
        }

        public WindowType GetWindowLayer()
        {
            return m_WindowType;
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

#region SubView
        public SubView AddSubView(System.Type type, GameObject root = null)
        {
            SubView subView = Activator.CreateInstance(type) as SubView;
            m_SubViewList.Add(subView);
            try
            {
                subView.Init(root);
                subView.SetView(this);
                subView.SetActive(true);
                return subView;
            }
            catch (Exception ex)
            {
                Huge.Debug.LogError($"UI: init {type.Name} error: {ex.Message}.\n{ex.StackTrace}");
                m_SubViewList.Remove(subView);
                return null;
            }
        }

        public T AddSubView<T>(GameObject root = null) where T : SubView
        {
            return AddSubView(typeof(T), root) as T;
        }

        public void AddSubView(SubView subView)
        {
            m_SubViewList.Add(subView);
        }

        public void RemoveSubView(SubView subView, bool isDestroy = true)
        {
            if (subView != null && m_SubViewList.Contains(subView))
            {
                m_SubViewList.Remove(subView);
                if (isDestroy) subView.Destroy();
            }
        }
#endregion SubView
    }
}
