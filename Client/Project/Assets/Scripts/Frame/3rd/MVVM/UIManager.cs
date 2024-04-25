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

    public class UIManager : Singleton<UIManager>
    {
        Camera m_UICamera;
        GameObject m_MaskObject;
        GameObject m_UIRootObject;
        List<Window> m_StackWindow = new List<Window>();
        Dictionary<System.Type, Window> m_WindowPools = new Dictionary<System.Type, Window>();
        Dictionary<LayerType, GameObject> m_LayerObjects = new Dictionary<LayerType, GameObject>();

        internal async UniTask InitAsync()
        {
            var prefab = await Resources.LoadAsync<GameObject>("UIRoot");
            m_UIRootObject = GameObject.Instantiate(prefab as GameObject);
            GameObject.DontDestroyOnLoad(m_UIRootObject);
            InternalInit(m_UIRootObject);
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

        internal void Destroy()
        {
            GameObject.Destroy(m_UIRootObject);
        }

        public Camera GetCamera()
        {
            return m_UICamera;
        }

        public T OpenWindow<T>(params object[] args) where T : Window
        {
            var t = typeof(T);
            Window Window = Activator.CreateInstance(t) as Window;
            m_StackWindow.Add(Window);
            try
            {
                Window.Init(null, args);
                return Window as T;
            }
            catch(Exception ex)
            {
                Huge.Debug.LogError($"UI: init {t.Name} error: {ex.Message}.\n{ex.StackTrace}.");
                m_StackWindow.Remove(Window);
                return null;
            }
        }

        public async UniTask<T> OpenWindowAsync<T>(params object[] args) where T : Window
        {
            var t = typeof(T);
            Window Window = Activator.CreateInstance(t) as Window;
            m_StackWindow.Add(Window);
            try
            {
                await Window.InitAsync(null, args);
                return Window as T;
            }
            catch(Exception ex)
            {
                Huge.Debug.LogError($"UI: init {t.Name} error: {ex.Message}.\n{ex.StackTrace}");
                if (m_StackWindow.Contains(Window)) m_StackWindow.Remove(Window);
                return null;
            }
        }

        public void CloseWindow(Window Window)
        {
            if (Window != null && m_StackWindow.Contains(Window) && Window.IsDestroied())
            {
                try
                {
                    m_StackWindow.Remove(Window);
                    Window.Destroy();
                }
                catch (Exception ex)
                {
                    Huge.Debug.LogError($"UI: close {Window.GetType().Name} error: {ex.Message}.\n{ex.StackTrace}");
                    if (m_StackWindow.Contains(Window)) m_StackWindow.Remove(Window);
                    throw ex;
                }
            }
        }

        public void CloseWindowAndPlayAnimation(Window Window)
        {
            CloseWindowAsync(Window);
        }

        public void CloseWindowAsync(Window Window)
        {
            if (Window != null && m_StackWindow.Contains(Window) && Window.IsDestroied())
            {
                try
                {
                    m_StackWindow.Remove(Window);
                    Window.DestroyAsync().Forget();
                }
                catch (Exception ex)
                {
                    Huge.Debug.LogError($"UI: close {Window.GetType().Name} error: {ex.Message}.\n{ex.StackTrace}");
                    if (m_StackWindow.Contains(Window)) m_StackWindow.Remove(Window);
                    throw ex;
                }
            }
        }

        public void CloseWindow<T>() where T : Window
        {
            Window Window = GetWindow<T>();
            if (Window != null)
            {
                CloseWindow(Window);
            }
        }

        public Window GetWindow<T>() where T : Window
        {
            var t = typeof(T);
            for(int i = 0; i < m_StackWindow.Count; i++)
            {
                Window Window = m_StackWindow[i];
                if (Window.GetType() == t)
                {
                    return Window;
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
