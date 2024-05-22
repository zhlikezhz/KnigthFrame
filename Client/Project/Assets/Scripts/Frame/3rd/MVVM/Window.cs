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

        public T AddSubView<T>(GameObject parent) where T : SubView
        {
            return AddSubView<T>(null, parent);
        }

        public T AddSubView<T>(GameObject root, GameObject parent) where T : SubView
        {
            var t = typeof(T);
            SubView subView = Activator.CreateInstance(t) as SubView;
            m_SubViewList.Add(subView);
            try
            {
                subView.Init(root);
                subView.SetWindow(this);
                subView.SetParent(parent, false);
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

        public void AddSubView(SubView subView, GameObject parent)
        {
            m_SubViewList.Add(subView);
            subView.SetParent(parent, false);
        }

        public void RemoveSubView(SubView subView, bool isDestroy = true)
        {
            if (subView != null && m_SubViewList.Contains(subView))
            {
                m_SubViewList.Remove(subView);
                if (isDestroy) subView.Destroy();
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

        internal protected virtual bool IsUseMask()
        {
            return true;
        }

        internal protected virtual float GetMaskAlpha()
        {
            return 1.0f;
        }

        internal protected virtual bool IsClickMask()
        {
            return true;
        }

        internal protected virtual void OnClickMask()
        {
            Close();
        }
    }
}