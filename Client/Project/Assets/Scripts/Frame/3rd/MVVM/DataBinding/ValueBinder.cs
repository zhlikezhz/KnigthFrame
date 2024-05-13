using System;
using Huge.Pool;
using UnityEditor;
using UnityEngine;
using Huge.MVVM;
using System.ComponentModel;

namespace Huge.MVVM.DataBinding
{
    public class ValueBinder<TSource, TTarget> : IBinder
        where TSource : INotifyPropertyChanged
    {
        bool m_bIsDirty = true;
        bool m_bIsBuilded = false;
        public string PropertyName { get; set; }
        public TTarget Target {get;set;}
        public TSource Source {get;set;}
        public IProxy Proxy {get;set;}

        public ValueBinder(TSource source, TTarget target)
        {
            Source = source;
            Target = target;
        }

        public PropertyProxy<TSource, TTarget, TValue> For<TValue>(Func<TSource, TValue> func, string propertyName)
        {
            PropertyName = propertyName;
            var proxy = new PropertyProxy<TSource, TTarget, TValue>();
            proxy.PropertyName = propertyName;
            proxy.GetValue = func;
            proxy.Source = Source;
            proxy.Target = Target;
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