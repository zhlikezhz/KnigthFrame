using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Huge.MVVM
{
    public abstract class Window : View
    {
        public T AddSection<T>(GameObject parent, params object[] args) where T : Section
        {
            return AddSection<T>(null, parent, args);
        }

        public T AddSection<T>(GameObject root, GameObject parent, params object[] args) where T : Section
        {
            var t = typeof(T);
            Section section = Activator.CreateInstance(t) as Section;
            m_SectionList.Add(section);
            try
            {
                section.Init(root, args);
                section.SetParent(parent, false);
                section.SetView(this);
                section.SetActive(true);
                return section as T;
            }
            catch (Exception ex)
            {
                Huge.Debug.LogError($"UI: init {t.Name} error: {ex.Message}.\n{ex.StackTrace}");
                m_SectionList.Remove(section);
                return null;
            }
        }

        public void RemoveSection(Section section)
        {
            if (section != null && m_SectionList.Contains(section))
            {
                m_SectionList.Remove(section);
                try
                {
                    section.Destroy();
                }
                catch (Exception ex)
                {
                    Huge.Debug.LogError($"UI: remove {section.GetType().Name} error: {ex.Message}.\n{ex.StackTrace}");
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