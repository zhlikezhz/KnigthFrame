using System;
using UnityEngine;
using System.ComponentModel;
using Huge.Pool;
using Huge.MVVM;

namespace Huge.MVVM.DataBinding
{
    public class CustomPropertyBinder : IBinder, IReuseObject
    {
        static readonly ObjectPool<CustomPropertyBinder> s_Pool = new ObjectPool<CustomPropertyBinder>(l => l.OnReinit(), l => l.OnRelease(), false);
        public static void Release(CustomPropertyBinder toRelease) => s_Pool.Release(toRelease);
        public static CustomPropertyBinder Get() => s_Pool.Get();

        bool m_bIsDirty = false;
        bool m_bIsBuilded = false;
        IProxy Proxy {get;set;}
        string PropertyName { get; set; }
        INotifyPropertyChanged Value {get; set;}
        public INotifyPropertyChanged Source {get; set;}

        public CustomPropertyBinder()
        {

        }

        void OnReinit()
        {

        }

        public CustomPropertyProxy<TValue> For<TValue>(Func<TValue> func, string propertyName) where TValue : ObservableObject
        {
            PropertyName = propertyName;
            var proxy = CustomPropertyProxy<TValue>.Get();
            proxy.Handler = OnValuePropertyChanged;
            proxy.GetValue = func;
            Proxy = proxy;
            return proxy;
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
        }

        public void Release()
        {
            Release(this);
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
            if (!m_bIsBuilded && Source != null)
            {
                m_bIsBuilded = true;
                Source.PropertyChanged += OnPropertyChanged;
                Proxy?.Update();
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

        void OnValuePropertyChanged(object sender, PropertyChangedEventArgs evt)
        {
            m_bIsDirty = true;
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