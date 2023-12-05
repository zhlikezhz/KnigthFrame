using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Huge.MVVM
{
    public class Section
    {
        Prefab m_Prefab;
        SectionInfo m_SectionInfo;
        bool m_bIsActived = false;

        internal async UniTask InitAsync(SectionInfo sectionInfo)
        {
            m_SectionInfo = sectionInfo;
        }

        internal async UniTask InitAsync(SectionInfo sectionInfo, GameObject root)
        {
            m_SectionInfo = sectionInfo;
        }

        internal async UniTask DestroyAsync()
        {

        }

        internal void AfterInit(params object[] args)
        {

        }

        internal void BeforeDestroy()
        {

        }

        internal virtual void Start()
        {

        }

        internal virtual void OnEnable()
        {

        }

        internal virtual void OnDisable()
        {

        }

        internal virtual void OnDestroy()
        {

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

        public void SetParent(GameObject parent)
        {
            m_Prefab.SetParent(parent);
        }

        public SectionInfo GetSectionInfo()
        {
            return m_SectionInfo;
        }
    }
}
