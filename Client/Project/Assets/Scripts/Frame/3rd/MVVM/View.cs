using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace Huge.MVVM
{
    public class ViewInfo
    {
        public System.Type View;
        public System.Type Prefab;
        public System.Type ViewModel;
    }

    public class SectionInfo
    {
        public System.Type Section;
        public System.Type Prefab;
        public System.Type ViewModel;
    }

    public class LoginPage : View
    {

    }

    public class LoginSection : Section
    {

    }

    public class LoginPrefab : Prefab
    {

    }

    public class LoginViewModel : ViewModel
    {

    }

    public static class ViewType
    {
        public static ViewInfo Login = new ViewInfo { View=typeof(LoginPage), Prefab=typeof(LoginPrefab), ViewModel=typeof(LoginViewModel) };
        public static SectionInfo LoginSection = new SectionInfo { Section=typeof(LoginSection), Prefab=typeof(LoginPrefab), ViewModel=typeof(LoginViewModel) };
    }

    public abstract class View
    {
        Prefab m_Prefab;
        ViewInfo m_ViewInfo;
        ViewModel m_ViewModel;
        List<Section> m_SectionList = new List<Section>();

        bool m_bIsActived = false;

        internal async UniTask InitAsync(ViewInfo viewInfo)
        {
            m_ViewInfo = viewInfo;
            m_Prefab = await Prefab.CreateAsync(viewInfo.Prefab);
            if (viewInfo.ViewModel != null)
            {
                m_ViewModel = await ViewModel.CreateAsync(viewInfo.ViewModel);
            }
            m_Prefab.SetParent(UIManager.Instance.GetLayerObject(GetLayerType()));
        }

        internal virtual async UniTask DestroyAsync()
        {
            foreach(Section section in m_SectionList)
            {
                await section.DestroyAsync();
            }
            OnDestroy();
            if (m_ViewModel != null)
            {
                await m_ViewModel.DestroyAsync();
            }
            await m_Prefab.DestroyAsync();
        }

        internal virtual void AfterInit(params object[] args)
        {
            Start(args);
        }

        internal virtual void BeforeDestroy()
        {
            foreach(Section section in m_SectionList)
            {
                section.BeforeDestroy();
            }
            SetActive(false);
        }

        protected virtual void Start(params object[] args)
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

        internal protected virtual LayerType GetLayerType()
        {
            return LayerType.NormalLayer;
        }

        public ViewInfo GetViewInfo()
        {
            return m_ViewInfo;
        }

        public void SetParent(GameObject parent)
        {
            m_Prefab.SetParent(parent);
        }

        public void SetActive(bool isActived)
        {
            if (m_bIsActived != isActived)
            {
                m_bIsActived = isActived;
                m_Prefab.SetActive(isActived);
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

        public async UniTask<Section> AddSection(SectionInfo sectionInfo, GameObject parent, params object[] args)
        {
            Section section = Activator.CreateInstance(sectionInfo.Section) as Section;
            m_SectionList.Add(section);
            try
            {
                await section.InitAsync(sectionInfo);
                section.AfterInit(args);
                section.SetParent(parent);
                section.SetActive(true);
                return section;
            }
            catch (Exception ex)
            {
                Huge.Debug.LogError($"UI: init {sectionInfo.Section.Name} error: {ex.Message}.");
                m_SectionList.Remove(section);
                return null;
            }
        }

        public async UniTask<Section> AddSection(SectionInfo sectionInfo, GameObject root, GameObject parent, params object[] args)
        {
            Section section = Activator.CreateInstance(sectionInfo.Section) as Section;
            m_SectionList.Add(section);
            try
            {
                await section.InitAsync(sectionInfo, root);
                section.AfterInit(args);
                section.SetParent(parent);
                section.SetActive(true);
                return section;
            }
            catch (Exception ex)
            {
                Huge.Debug.LogError($"UI: init {sectionInfo.Section.Name} error: {ex.Message}.");
                m_SectionList.Remove(section);
                return null;
            }
        }

        public void RemoveSection(Section section)
        {
            if (m_SectionList.Contains(section))
            {
                m_SectionList.Remove(section);
                section.BeforeDestroy();
                section.DestroyAsync().Forget();
            }
        }

        public bool HasSection(SectionInfo sectionInfo) 
        {
            return (GetSection(sectionInfo) != null);
        }

        public Section GetSection(SectionInfo sectionInfo)
        {
            foreach(var section in m_SectionList)
            {
                if (section.GetSectionInfo() == sectionInfo)
                {
                    return section;
                }
            }
            return null;
        }
    }
}
