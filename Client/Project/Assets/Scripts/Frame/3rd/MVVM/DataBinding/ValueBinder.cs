using System;
using Huge.Pool;
using UnityEditor;
using UnityEngine;
using Huge.MVVM;
using System.ComponentModel;

namespace Huge.MVVM.DataBinding
{
    public class ValueBinder : IBinder, IReuseObject
    {
        static readonly ObjectPool<ValueBinder> s_Pool = new ObjectPool<ValueBinder>(l => l.OnReinit(), l => l.OnRelease(), false);
        public static void Release(ValueBinder toRelease) => s_Pool.Release(toRelease);
        public static ValueBinder Get() => s_Pool.Get();

        bool m_bIsDirty = true;
        bool m_bIsBuilded = false;
        IProxy Proxy {get;set;}
        string PropertyName { get; set; }
        public INotifyPropertyChanged Source {get; set;}

        public ValueBinder()
        {

        }

        void OnReinit()
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
            Release(this);
        }

        public void Release()
        {
            Release(this);
        }

        public PropertyProxy<TValue> For<TValue>(Func<TValue> func, string propertyName)
        {
            PropertyName = propertyName;
            var proxy = PropertyProxy<TValue>.Get();
            proxy.GetValue = func;
            Proxy = proxy;
            return proxy;
        }

        public void Update()
        {
            if (m_bIsDirty && m_bIsBuilded)
            {
                m_bIsDirty = false;
                Proxy.Update();
            }
        }

        public void Build()
        {
            if (!m_bIsBuilded && Source != null)
            {
                m_bIsBuilded = true;
                Source.PropertyChanged += OnPropertyChanged;
            }
        }

        public void UnBuild()
        {
            if (m_bIsBuilded && Source != null)
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