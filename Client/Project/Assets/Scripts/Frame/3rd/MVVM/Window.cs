using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Huge.MVVM
{
    public abstract class Window : View
    {
        internal Window()
        {

        }

        public T AddSection<T>(GameObject parent) where T : SubView
        {
            return AddSection<T>(null, parent);
        }

        public T AddSection<T>(GameObject root, GameObject parent) where T : SubView
        {
            var t = typeof(T);
            SubView subView = Activator.CreateInstance(t) as SubView;
            m_SubViewList.Add(subView);
            try
            {
                subView.Init(root);
                subView.SetParent(parent, false);
                subView.SetView(this);
                subView.SetActive(true);
                return subView as T;
            }
            catch (Exception ex)
            {
                Huge.Debug.LogError($"UI: init {t.Name} error: {ex.Message}.\n{ex.StackTrace}");
                m_SubViewList.Remove(subView);
                return null;
            }
        }

        public void RemoveSection(SubView subView)
        {
            if (subView != null && m_SubViewList.Contains(subView))
            {
                m_SubViewList.Remove(subView);
                try
                {
                    subView.Destroy();
                }
                catch (Exception ex)
                {
                    Huge.Debug.LogError($"UI: remove {subView.GetType().Name} error: {ex.Message}.\n{ex.StackTrace}");
                }
            }
        }

        public void Close()
        {
            UIManager.Instance.CloseWindow(this);
        }

        public async UniTask CloseAndPlayAnimation()
        {
            await UIManager.Instance.CloseWindowAsync(this);
        }
    }
}