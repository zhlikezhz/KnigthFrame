using System;
using System.ComponentModel;
using System.Collections.Generic;
using Huge.Pool;
using Huge.MVVM;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace Huge.MVVM.DataBinding
{
    public sealed class BindingSet : IBinder, IReuseObject
    {
        static readonly ObjectPool<BindingSet> s_Pool = new ObjectPool<BindingSet>(l => l.OnReinit(), l => l.OnRelease(), false);
        public static void Release(BindingSet toRelease) => s_Pool.Release(toRelease);
        public static BindingSet Get() => s_Pool.Get();
        List<IBinder> m_BinderList = new List<IBinder>();

        public BindingSet()
        {

        }

        void OnReinit()
        {

        }

        void OnRelease()
        {
            UnBuild();
            foreach(var binder in m_BinderList)
            {
                binder.UnBuild();
                if (binder is IReuseObject reuseObject)
                {
                    reuseObject.Release();
                }
            }
            m_BinderList.Clear();
        }

        public void Release()
        {
            Release(this);
        }

        public ValueBinder Bind(INotifyPropertyChanged source) 
        {
            var binder = ValueBinder.Get();
            binder.Source = source;
            m_BinderList.Add(binder);
            return binder;
        }

        public ListBinder BindList(INotifyPropertyChanged source)
        {
            var binder = ListBinder.Get();
            binder.Source = source;
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
