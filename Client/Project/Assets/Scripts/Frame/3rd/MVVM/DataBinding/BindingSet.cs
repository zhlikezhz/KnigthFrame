using System;
using System.Collections.Generic;
using Huge.Pool;
using Huge.MVVM;
using UnityEngine;

namespace Huge.MVVM.DataBinding
{
    public sealed class BindingSet<TView, TViewModel> : IBinder
        where TViewModel : ViewModel
        where TView : View
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

        public ValueBinder<TViewModel, TView> Bind()
        {
            var binder = new ValueBinder<TViewModel, TView>(m_ViewModel, m_View);
            m_BinderList.Add(binder);
            return binder;
        }

        public ValueBinder<TViewModel, TTarget> Bind<TTarget>(TTarget target)
        {
            var binder = new ValueBinder<TViewModel, TTarget>(m_ViewModel, target);
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

        public void UnBuild()
        {
            foreach(var binder in m_BinderList)
            {
                binder.UnBuild();
            }
        }

        public void Update()
        {
            foreach (var binder in m_BinderList)
            {
                binder.Update();
            }
        }
    }
}
