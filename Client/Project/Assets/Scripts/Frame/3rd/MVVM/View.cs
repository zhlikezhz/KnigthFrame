using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Threading;

namespace Huge.MVVM
{
    public abstract class View
    {
        bool m_bIsActived = false;
        bool m_bIsDestroied = false;
        CancellationTokenSource m_Source;

        protected Prefab m_Prefab;
        protected ViewModel m_ViewModel;
        protected List<Section> m_SectionList = new List<Section>();

        public CancellationToken Token { get; private set; }

        /// <summary>
        /// 同步创建
        /// /// </summary>
        /// <param name="args"></param>
        internal void Init(GameObject root, params object[] args)
        {
            Type t = GetType();
            ViewSettingAttribute viewSetting = t.GetAttribute<ViewSettingAttribute>();
            if (viewSetting == null)
            {
                throw new Exception("view need ViewSettingAttribute, please add ViewSettingAttribute to view");
            }
            else
            {
                if (viewSetting.Prefab == null)
                {
                    throw new Exception("view need set prefab, please add ViewSettingAttribute to view and set ViewSettingAttribute.Prefab");
                }
                m_Prefab = Prefab.Create(viewSetting.Prefab, root);
                m_Source = CancellationTokenSource.CreateLinkedTokenSource(m_Prefab.Token);
                Token = m_Source.Token;

                if (viewSetting.ViewModel != null)
                {
                    m_ViewModel = Activator.CreateInstance(viewSetting.ViewModel) as ViewModel;
                    m_ViewModel.Token = m_Source.Token;
                }
                m_Prefab.SetParent(UIManager.Instance.GetLayerObject(GetLayerType()));

                SetActive(true);
                AfterCreate();

                //调用用户接口,此时框架层都处理完成
                m_ViewModel?.Start(args);
                Start(args);
            }
        }

        /// <summary>
        /// 异步创建
        /// </summary>
        /// <param name="viewInfo"></param>
        /// <returns></returns>
        internal async UniTask InitAsync(GameObject root, params object[] args)
        {
            Type t = GetType();
            ViewSettingAttribute viewSetting = t.GetAttribute<ViewSettingAttribute>();
            if (viewSetting == null)
            {
                throw new Exception("view need ViewSettingAttribute, please add ViewSettingAttribute to view");
            }
            else
            {
                if (viewSetting.Prefab == null)
                {
                    throw new Exception("view need set prefab, please add ViewSettingAttribute to view and set ViewSettingAttribute.Prefab");
                }
                m_Prefab = await Prefab.CreateAsync(viewSetting.Prefab, root);
                m_Source = CancellationTokenSource.CreateLinkedTokenSource(m_Prefab.Token);
                Token = m_Source.Token;

                if (viewSetting.ViewModel != null)
                {
                    m_ViewModel = Activator.CreateInstance(viewSetting.ViewModel) as ViewModel;
                    m_ViewModel.Token = m_Source.Token;
                    m_ViewModel.m_Prefab = m_Prefab;
                    m_ViewModel.m_View = this;
                }
                m_Prefab.SetParent(UIManager.Instance.GetLayerObject(GetLayerType()));

                SetActive(true);
                AfterCreate();

                await DoOpenAnimation();

                //调用用户接口,此时框架层都处理完成
                m_ViewModel?.Start(args);
                Start(args);
            }
        }

        internal void Destroy()
        {
            if (!m_bIsDestroied)
            {
                m_bIsDestroied = true;
                foreach(Section section in m_SectionList)
                {
                    section.Destroy();
                }

                BeforeDestroy();
                SetActive(false);
                OnDestroy();
                m_ViewModel?.Destroy();
                m_Prefab?.Destroy();
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
                foreach(Section section in m_SectionList)
                {
                    await section.DestroyAsync();
                }

                BeforeDestroy();
                await DoCloseAnimation();
                SetActive(false);
                OnDestroy();
                m_ViewModel?.Destroy();
                m_Prefab?.Destroy();
                m_Source.Cancel();
            }
        }

        /// <summary>
        /// 在View初始化之后被调用
        /// </summary>
        /// <param name="args"></param> <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        internal virtual void AfterCreate()
        {

        }

        /// <summary>
        /// 在View销毁销毁之前被调用
        /// </summary> <summary>
        /// 
        /// </summary>
        internal virtual void BeforeDestroy()
        {

        }

#region override
        public virtual LayerType GetLayerType()
        {
            return LayerType.NormalLayer;
        }

        protected virtual async UniTask DoOpenAnimation()
        {

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

        protected virtual async UniTask DoCloseAnimation()
        {

        }

        protected virtual void OnDestroy()
        {

        }
#endregion

#region public method
        public void SetParent(GameObject parent, bool worldPositionStays = false)
        {
            m_Prefab.SetParent(parent, worldPositionStays);
        }

        public void SetActive(bool isActived)
        {
            if (m_bIsActived != isActived)
            {
                m_bIsActived = isActived;
                m_Prefab?.SetActive(isActived);
                if (isActived)
                {
                    m_ViewModel?.OnEnable();
                    OnEnable();
                }
                else
                {
                    m_ViewModel?.OnDisable();
                    OnDisable();
                }
            }
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
    }
}
