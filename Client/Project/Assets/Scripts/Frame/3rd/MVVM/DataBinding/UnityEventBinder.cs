using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Huge.MVVM.DataBinding
{
    public class UnityEventBinder : IBinder
    {
        bool m_bIsBuilded = false;
        public UnityEvent OnEvent {get; set;}
        public UnityAction OnAction {get; set;}

        public UnityEventBinder For(UnityEvent evt)
        {
            OnEvent = evt;
            return this;
        }

        public void To(UnityAction action)
        {
            OnAction = action;
        }

        public void Build()
        {
            if (!m_bIsBuilded)
            {
                m_bIsBuilded = true;
                OnEvent.AddListener(OnAction);
            }
        }

        public void UnBuild()
        {
            if (m_bIsBuilded)
            {
                m_bIsBuilded = false;
                OnEvent.RemoveListener(OnAction);
            }
        }

        public void Update()
        {

        }
    }

    public class UnityEventBinder<T0> : IBinder
    {
        bool m_bIsBuilded = false;
        public UnityEvent<T0> OnEvent {get; set;}
        public UnityAction<T0> OnAction {get; set;}

        public UnityEventBinder<T0> For(UnityEvent<T0> evt)
        {
            OnEvent = evt;
            return this;
        }

        public void To(UnityAction<T0> action)
        {
            OnAction = action;
        }

        public void Build()
        {
            if (!m_bIsBuilded)
            {
                m_bIsBuilded = true;
                OnEvent.AddListener(OnAction);
            }
        }

        public void UnBuild()
        {
            if (m_bIsBuilded)
            {
                m_bIsBuilded = false;
                OnEvent.RemoveListener(OnAction);
            }
        }

        public void Update()
        {

        }
    }

    public class UnityEventBinder<T0, T1> : IBinder
    {
        bool m_bIsBuilded = false;
        public UnityEvent<T0, T1> OnEvent {get; set;}
        public UnityAction<T0, T1> OnAction {get; set;}

        public UnityEventBinder<T0, T1> For(UnityEvent<T0, T1> evt)
        {
            OnEvent = evt;
            return this;
        }

        public void To(UnityAction<T0, T1> action)
        {
            OnAction = action;
        }

        public void Build()
        {
            if (!m_bIsBuilded)
            {
                m_bIsBuilded = true;
                OnEvent.AddListener(OnAction);
            }
        }

        public void UnBuild()
        {
            if (m_bIsBuilded)
            {
                m_bIsBuilded = false;
                OnEvent.RemoveListener(OnAction);
            }
        }

        public void Update()
        {

        }
    }

    public class UnityEventBinder<T0, T1, T2> : IBinder
    {
        bool m_bIsBuilded = false;
        public UnityEvent<T0, T1, T2> OnEvent {get; set;}
        public UnityAction<T0, T1, T2> OnAction {get; set;}

        public UnityEventBinder<T0, T1, T2> For(UnityEvent<T0, T1, T2> evt)
        {
            OnEvent = evt;
            return this;
        }

        public void To(UnityAction<T0, T1, T2> action)
        {
            OnAction = action;
        }

        public void Build()
        {
            if (!m_bIsBuilded)
            {
                m_bIsBuilded = true;
                OnEvent.AddListener(OnAction);
            }
        }

        public void UnBuild()
        {
            if (m_bIsBuilded)
            {
                m_bIsBuilded = false;
                OnEvent.RemoveListener(OnAction);
            }
        }

        public void Update()
        {

        }
    }
}