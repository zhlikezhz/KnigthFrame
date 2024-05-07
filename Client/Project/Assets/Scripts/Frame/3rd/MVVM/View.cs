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
        internal List<Section> m_SectionList = new List<Section>();

        protected Prefab Prefab;
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
                Prefab = Prefab.Create(viewSetting.Prefab, root);
                m_Source = CancellationTokenSource.CreateLinkedTokenSource(Prefab.Token);
                Token = m_Source.Token;
                Prefab.SetParent(UIManager.Instance.GetLayerObject(GetLayerType()));

                SetActive(true);
                AfterCreate();
                //调用用户接口,此时框架层都处理完成
                OnStart(args);
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
                Prefab = await Prefab.CreateAsync(viewSetting.Prefab, root);
                Prefab.SetParent(UIManager.Instance.GetLayerObject(GetLayerType()));
                m_Source = CancellationTokenSource.CreateLinkedTokenSource(Prefab.Token);
                Token = m_Source.Token;
                AfterCreate();

                SetActive(true);
                //调用用户接口,此时框架层都处理完成
                OnStart(args);

                if (this is IViewEnterAnimation view)
                {
                    await view.OnPlayEnterAnimation();
                }
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
                Prefab?.Destroy();
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
                if (this is IViewExitAnimation view)
                {
                    await view.OnPlayExitAnimation();
                }

                SetActive(false);
                OnDestroy();
                Prefab?.Destroy();
                m_Source.Cancel();
            }
        }

#region internal override
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
#endregion

#region override
        public virtual LayerType GetLayerType()
        {
            return LayerType.NormalLayer;
        }

        protected virtual void OnStart(params object[] args)
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
#endregion

#region public method
        public void SetParent(GameObject parent, bool worldPositionStays = false)
        {
            Prefab.SetParent(parent, worldPositionStays);
        }

        public void SetActive(bool isActived)
        {
            if (m_bIsActived != isActived)
            {
                m_bIsActived = isActived;
                Prefab?.SetActive(isActived);
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
