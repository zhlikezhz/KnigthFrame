using System;
using System.ComponentModel;
using System.Collections.Generic;
using Huge.Pool;
using Huge.MVVM;
using UnityEngine;
using UnityEngine.UI;

namespace Huge.MVVM.DataBinding
{
    public sealed class BindingSet<TView, TViewModel> : IBinder
        where TViewModel : INotifyPropertyChanged
    {
        TView m_View;
        TViewModel m_ViewModel;
        List<IBinder> m_BinderList = new List<IBinder>();

        public BindingSet(TView view, TViewModel viewModel)
        {
            m_ViewModel = viewModel;
            m_View = view;
        }

        public void SetViewModel(TViewModel viewModel)
        {
            m_ViewModel = viewModel;
        }

        public void SetView(TView view)
        {
            m_View = view;
        }

        public ValueBinder<TViewModel, GameObject> Bind(GameObject target) 
        {
            var binder = new ValueBinder<TViewModel, GameObject>(m_ViewModel, target);
            m_BinderList.Add(binder);
            return binder;
        }

        public ValueBinder<TViewModel, TTarget> Bind<TTarget>(TTarget target) where TTarget : UnityEngine.Component
        {
            var binder = new ValueBinder<TViewModel, TTarget>(m_ViewModel, target);
            m_BinderList.Add(binder);
            return binder;
        }

        public ValueBinder<TViewModel, TTarget> BindList<TTarget>(TTarget target) where TTarget : IListView
        {
            var binder = new ValueBinder<TViewModel, TTarget>(m_ViewModel, target);
            m_BinderList.Add(binder);
            return binder;
        }

        public UnityEventBinder<string> BindInputField(InputField field)
        {
            var binder = new UnityEventBinder<string>();
            m_BinderList.Add(binder);
            return binder;
        }

        public UnityEventBinder BindButton(Button button)
        {
            var binder = new UnityEventBinder();
            m_BinderList.Add(binder);
            return binder;
        }

        public void Build()
        {
            foreach (var binder in m_BinderList)
            {
                binder.Build();
            }
        }

        public void Update()
        {
            foreach (var binder in m_BinderList)
            {
                binder.Update();
            }
        }

        public void UnBuild()
        {
            foreach(var binder in m_BinderList)
            {
                binder.UnBuild();
            }
        }
    }
}
