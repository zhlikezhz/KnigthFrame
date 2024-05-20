using System;
using Huge.Pool;
using UnityEditor;
using UnityEngine;
using Huge.MVVM;
using System.ComponentModel;
using System.Collections.Specialized;

namespace Huge.MVVM.DataBinding
{
    public class ListBinder : IBinder, IReuseObject
    {
        static readonly ObjectPool<ListBinder> s_Pool = new ObjectPool<ListBinder>(null, l => l.OnRelease(), false);
        public static void Release(ListBinder toRelease) => s_Pool.Release(toRelease);
        public static ListBinder Get() => s_Pool.Get();

        bool m_bIsDirty = false;
        bool m_bIsBuilded = false;
        IProxy Proxy {get;set;}
        string PropertyName { get; set; }
        public INotifyPropertyChanged Source {get; set;}

        public ListBinder()
        {

        }

        void OnRelease()
        {
            UnBuild();
            Source = null;
            PropertyName = null;
            if (Proxy is IReuseObject reuseObject)
            {
                reuseObject.Release();
            }
            Proxy = null;
            m_bIsBuilded = false;
            m_bIsBuilded = false;
        }

        public void Release()
        {
            Release(this);
        }

        public ListPropertyProxy<TTarget> For<TTarget>(Func<ObservableList<TTarget>> func, string propertyName) where TTarget : ViewModel
        {
            PropertyName = propertyName;
            var proxy = ListPropertyProxy<TTarget>.Get();
            proxy.GetSourceDelegate = func;
            Proxy = proxy;
            return proxy;
        }

        public void Update()
        {
            if (m_bIsDirty && m_bIsBuilded)
            {
                m_bIsDirty = false;
                Proxy?.Update();
            }
        }

        public void Build()
        {
            if (!m_bIsBuilded)
            {
                m_bIsBuilded = true;
                Source.PropertyChanged += OnPropertyChanged;
                Proxy?.Update();
            }
        }

        public void UnBuild()
        {
            if (m_bIsBuilded)
            {
                m_bIsBuilded = false;
                Source.PropertyChanged -= OnPropertyChanged;
            }
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs evt)
        {
            if (evt.PropertyName == PropertyName)
            {
                m_bIsDirty = true;
            }
        }
    }
}