using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Huge;
using Huge.Pool;
using Huge.Asset;
using UnityEditor.PackageManager.Requests;
using UnityEngine.PlayerLoop;

namespace Huge.MVVM
{
    public enum LayerType
    {
        None = 0,
        BackLayer = 1,
        NormalLayer = 2,
        PopupLayer = 3,
        GuideLayer = 4,
        TopLayer = 5,
    }

    public class UIManager : Singleton<UIManager>
    {
        Camera m_UICamera;
        GameObject m_MaskObject;
        GameObject m_UIRootObject;
        List<View> m_StackView = new List<View>();
        Dictionary<System.Type, View> m_ViewPools = new Dictionary<System.Type, View>();
        Dictionary<LayerType, GameObject> m_LayerObjects = new Dictionary<LayerType, GameObject>();

        internal void Init()
        {
            GameObject prefab = Resources.Load<GameObject>("UIRoot");
            m_UIRootObject = GameObject.Instantiate(prefab);
            GameObject.DontDestroyOnLoad(m_UIRootObject);
            m_UIRootObject.name = "uiRoot";
            InternalInit(GameObject.Instantiate(prefab));
        }

        internal async UniTask InitAsync()
        {
            var prefab = await Resources.LoadAsync<GameObject>("UIRoot");
            m_UIRootObject = GameObject.Instantiate(prefab as GameObject);
            GameObject.DontDestroyOnLoad(m_UIRootObject);
            m_UIRootObject.name = "uiRoot";
            InternalInit(m_UIRootObject);
        }

        void InternalInit(GameObject uiRoot)
        {
            Transform uiTrans = uiRoot.transform;
            m_MaskObject = uiTrans.Find("UI/UIMask").gameObject;
            m_UICamera = uiTrans.Find("UICamera").GetComponent<Camera>();
            m_LayerObjects.Add(LayerType.BackLayer, uiTrans.Find("UI/BackLayer").gameObject);
            m_LayerObjects.Add(LayerType.NormalLayer, uiTrans.Find("UI/NormalLayer").gameObject);
            m_LayerObjects.Add(LayerType.PopupLayer, uiTrans.Find("UI/PopupLayer").gameObject);
            m_LayerObjects.Add(LayerType.GuideLayer, uiTrans.Find("UI/GuideLayer").gameObject);
            m_LayerObjects.Add(LayerType.TopLayer, uiTrans.Find("UI/TopLayer").gameObject);
        }

        internal void Destroy()
        {
            GameObject.Destroy(m_UIRootObject);
        }

        public Camera GetCamera()
        {
            return m_UICamera;
        }

        public T OpenView<T>(params object[] args) where T : View
        {
            var t = typeof(T);
            View view = Activator.CreateInstance(t) as View;
            m_StackView.Add(view);
            try
            {
                view.Init(args);
                return view as T;
            }
            catch(Exception ex)
            {
                Huge.Debug.LogError($"UI: init {t.Name} error: {ex.Message}.");
                m_StackView.Remove(view);
                return null;
            }
        }

        public async UniTask<T> OpenViewAsync<T>(params object[] args) where T : View
        {
            var t = typeof(T);
            View view = Activator.CreateInstance(t) as View;
            m_StackView.Add(view);
            try
            {
                await view.InitAsync(args);
                return view as T;
            }
            catch(Exception ex)
            {
                Huge.Debug.LogError($"UI: init {t.Name} error: {ex.Message}.");
                if (m_StackView.Contains(view)) m_StackView.Remove(view);
                return null;
            }
        }

        public void CloseView(View view)
        {
            if (view != null && m_StackView.Contains(view) && view.IsDestoried())
            {
                try
                {
                    m_StackView.Remove(view);
                    view.Destroy().Forget();
                }
                catch (Exception ex)
                {
                    Huge.Debug.LogError($"UI: close {view.GetType().Name} error: {ex.Message}.");
                    if (m_StackView.Contains(view)) m_StackView.Remove(view);
                    throw ex;
                }
            }
        }

        public void CloseView<T>() where T : View
        {
            View view = GetView<T>();
            if (view != null)
            {
                CloseView(view);
            }
        }

        public View GetView<T>() where T : View
        {
            var t = typeof(T);
            for(int i = 0; i < m_StackView.Count; i++)
            {
                View view = m_StackView[i];
                if (view.GetType() == t)
                {
                    return view;
                }
            }
            return null;
        }

        internal GameObject GetMaskObject()
        {
            return m_MaskObject;
        }

        internal GameObject GetLayerObject(LayerType layerType)
        {
            if (m_LayerObjects.TryGetValue(layerType, out var layerObject))
            {
                return layerObject;
            }
            return null;
        }

        internal List<View> GetViewStack()
        {
            return m_StackView;
        }
    }
}
