using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Huge;
using Huge.Pool;
using Huge.Asset;

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
        GameObject m_MaskObject;
        GameObject m_UIRootObject;
        List<View> m_StackView = new List<View>();
        Dictionary<System.Type, View> m_ViewPools = new Dictionary<System.Type, View>();
        Dictionary<LayerType, GameObject> m_LayerObjects = new Dictionary<LayerType, GameObject>();

        const string CanvasPrefabPath = "Art/Prefab/Canvas.prefab";
        internal async UniTask InitAsync()
        {
            GameObject prefab = await AssetManager.Instance.LoadAssetAsync<GameObject>(CanvasPrefabPath);
            m_UIRootObject = GameObject.Instantiate(prefab);
            GameObject.DontDestroyOnLoad(m_UIRootObject);
        }

        public async UniTask<View> OpenView(ViewInfo viewInfo, params object[] args)
        {
            View view = Activator.CreateInstance(viewInfo.View) as View;
            m_StackView.Add(view);
            try
            {
                await view.InitAsync(viewInfo);
                view.AfterInit(args);
                view.SetActive(true);
                return view;
            }
            catch(Exception ex)
            {
                Huge.Debug.LogError($"UI: init {viewInfo.View.Name} error: {ex.Message}.");
                m_StackView.Remove(view);
                return null;
            }
        }

        public void CloseView(View view)
        {
            if (m_StackView.Contains(view))
            {
                ViewInfo viewInfo = view.GetViewInfo();
                try
                {
                    view.BeforeDestroy();
                    m_StackView.Remove(view);
                    view.DestroyAsync().Forget();
                }
                catch (Exception ex)
                {
                    Huge.Debug.LogError($"UI: close {viewInfo.View.Name} error: {ex.Message}.");
                    m_StackView.Remove(view);
                    throw ex;
                }
            }
        }

        public void CloseView(ViewInfo viewInfo)
        {
            View view = GetView(viewInfo);
            if (view != null)
            {
                CloseView(view);
            }
        }

        public View GetView(ViewInfo viewInfo)
        {
            for(int i = 0; i < m_StackView.Count; i++)
            {
                View view = m_StackView[i];
                if (view.GetViewInfo() == viewInfo)
                {
                    return view;
                }
            }
            return null;
        }

        internal List<View> GetViewStack()
        {
            return m_StackView;
        }

        internal GameObject GetLayerObject(LayerType layerType)
        {
            if (m_LayerObjects.TryGetValue(layerType, out var layerObject))
            {
                return layerObject;
            }
            return null;
        }
    }
}
