using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Huge;
using Huge.Pool;
using Huge.Asset;
using UnityEngine.Rendering.Universal;

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

    public enum WindowType
    {
        None    = 0,
        Page    = 1,
        Popup   = 2,
    }

    public class UIManager : Singleton<UIManager>
    {
        Camera m_UICamera;
        GameObject m_MaskObject;
        GameObject m_UIRootObject;
        List<Window> m_StackWindow = new List<Window>();
        Dictionary<System.Type, Window> m_WindowPools = new Dictionary<System.Type, Window>();
        Dictionary<LayerType, GameObject> m_LayerObjects = new Dictionary<LayerType, GameObject>();

        public void Init()
        {
            var prefab = Resources.Load<GameObject>("UIRoot");
            m_UIRootObject = GameObject.Instantiate(prefab as GameObject);
            GameObject.DontDestroyOnLoad(m_UIRootObject);
            InternalInit(m_UIRootObject);
        }

        public async UniTask InitAsync()
        {
            var prefab = await Resources.LoadAsync<GameObject>("UIRoot");
            m_UIRootObject = GameObject.Instantiate(prefab as GameObject);
            GameObject.DontDestroyOnLoad(m_UIRootObject);
            InternalInit(m_UIRootObject);
        }

        public void Destroy()
        {
            GameObject.Destroy(m_UIRootObject);
        }

        void InternalInit(GameObject uiRoot)
        {
            Transform uiTrans = uiRoot.transform;
            m_MaskObject = uiTrans.Find("UI/UIMask").gameObject;
            m_UICamera = uiTrans.Find("UICamera").GetComponent<Camera>();
            Camera.main.GetComponent<UniversalAdditionalCameraData>().cameraStack.Add(m_UICamera);
            m_LayerObjects.Add(LayerType.BackLayer, uiTrans.Find("UI/BackLayer").gameObject);
            m_LayerObjects.Add(LayerType.NormalLayer, uiTrans.Find("UI/NormalLayer").gameObject);
            m_LayerObjects.Add(LayerType.PopupLayer, uiTrans.Find("UI/PopupLayer").gameObject);
            m_LayerObjects.Add(LayerType.GuideLayer, uiTrans.Find("UI/GuideLayer").gameObject);
            m_LayerObjects.Add(LayerType.TopLayer, uiTrans.Find("UI/TopLayer").gameObject);
        }

        public Camera GetCamera()
        {
            return m_UICamera;
        }

        public T OpenWindow<T>(WindowType windowType, LayerType layerType = LayerType.NormalLayer) where T : Window
        {
            if (windowType == WindowType.None)
            {
                UnityEngine.Debug.LogError("WindowType is None");
                return null;
            }

            var t = typeof(T);
            Window window = Activator.CreateInstance(t) as Window;
            m_StackWindow.Add(window);
            try
            {
                window.Init(null, windowType, layerType);
                return window as T;
            }
            catch(Exception ex)
            {
                Huge.Debug.LogError($"UI: init {t.Name} error: {ex.Message}.\n{ex.StackTrace}.");
                m_StackWindow.Remove(window);
                return null;
            }
        }

        public async UniTask<T> OpenWindowAsync<T>(WindowType windowType, LayerType layerType = LayerType.NormalLayer) where T : Window
        {
            if (windowType == WindowType.None)
            {
                UnityEngine.Debug.LogError("WindowType is None");
                return null;
            }

            var t = typeof(T);
            Window window = Activator.CreateInstance(t) as Window;
            m_StackWindow.Add(window);
            try
            {
                await window.InitAsync(null, windowType, layerType);
                return window as T;
            }
            catch(Exception ex)
            {
                Huge.Debug.LogError($"UI: init {t.Name} error: {ex.Message}.\n{ex.StackTrace}");
                if (m_StackWindow.Contains(window)) m_StackWindow.Remove(window);
                return null;
            }
        }

        public void CloseWindow(Window window)
        {
            if (window != null && m_StackWindow.Contains(window) && !window.IsDestroied())
            {
                try
                {
                    window.Destroy();
                    m_StackWindow.Remove(window);
                }
                catch (Exception ex)
                {
                    Huge.Debug.LogError($"UI: close {window.GetType().Name} error: {ex.Message}.\n{ex.StackTrace}");
                    if (m_StackWindow.Contains(window)) m_StackWindow.Remove(window);
                    throw ex;
                }
            }
        }

        public async UniTask CloseWindowAsync(Window window)
        {
            if (window != null && m_StackWindow.Contains(window) && !window.IsDestroied())
            {
                try
                {
                    await window.DestroyAsync();
                    m_StackWindow.Remove(window);
                }
                catch (Exception ex)
                {
                    Huge.Debug.LogError($"UI: close {window.GetType().Name} error: {ex.Message}.\n{ex.StackTrace}");
                    if (m_StackWindow.Contains(window)) m_StackWindow.Remove(window);
                    throw ex;
                }
            }
        }

        public void CloseWindow<T>() where T : Window
        {
            Window window = GetWindow<T>();
            if (window != null)
            {
                CloseWindow(window);
            }
        }

        public Window GetWindow<T>() where T : Window
        {
            var t = typeof(T);
            for(int i = 0; i < m_StackWindow.Count; i++)
            {
                Window window = m_StackWindow[i];
                if (window.GetType() == t)
                {
                    return window;
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

        internal List<Window> GetWindowStack()
        {
            return m_StackWindow;
        }
    }
}
