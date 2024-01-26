using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Huge.MVVM
{
    public abstract class View
    {
        bool m_bIsActived = false;
        bool m_bIsDestoried = false;
        protected Prefab m_Prefab;
        protected ViewModel m_ViewModel;

        /// <summary>
        /// 同步创建
        /// </summary>
        /// <param name="args"></param>
        internal void Init(params object[] args)
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
                m_Prefab = Prefab.Create(viewSetting.Prefab);
                if (viewSetting.ViewModel != null)
                {
                    m_ViewModel = Activator.CreateInstance(viewSetting.ViewModel) as ViewModel;
                }
                m_Prefab.SetParent(UIManager.Instance.GetLayerObject(GetLayerType()));
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
        internal async UniTask InitAsync(params object[] args)
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
                m_Prefab = await Prefab.CreateAsync(viewSetting.Prefab);
                if (viewSetting.ViewModel != null)
                {
                    m_ViewModel = Activator.CreateInstance(viewSetting.ViewModel) as ViewModel;
                }
                m_Prefab.SetParent(UIManager.Instance.GetLayerObject(GetLayerType()));

                AfterCreate();

                await DoOpenAnimation();

                //调用用户接口,此时框架层都处理完成
                m_ViewModel?.Start(args);
                Start(args);
            }
        }

        protected async UniTask DoOpenAnimation()
        {
            await UniTask.WaitForSeconds(0.1f);
        }

        /// <summary>
        /// 异步销毁
        /// </summary>
        /// <returns></returns> <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal async UniTask Destroy()
        {
            if (!m_bIsDestoried)
            {
                BeforeDestroy();

                await DoCloseAnimation();

                m_bIsDestoried = true;
                m_ViewModel?.Destroy();
                m_Prefab?.Destroy();

                //调用用户接口,此时框架层都处理完成
                OnDestroy();
            }
        }

        protected async UniTask DoCloseAnimation()
        {
            await UniTask.WaitForSeconds(0.1f);
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
            SetActive(true);
        }

        /// <summary>
        /// 在View销毁销毁之前被调用
        /// </summary> <summary>
        /// 
        /// </summary>
        internal virtual void BeforeDestroy()
        {
            SetActive(false);
        }

#region override
        internal protected virtual LayerType GetLayerType()
        {
            return LayerType.NormalLayer;
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
#endregion

#region public method
        public void SetParent(GameObject parent)
        {
            m_Prefab.SetParent(parent);
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

        public bool IsDestoried()
        {
            return m_bIsDestoried;
        }

        public bool IsActived()
        {
            return m_bIsActived;
        }

        public void Close()
        {
            UIManager.Instance.CloseView(this);
        }
#endregion

    }
}
