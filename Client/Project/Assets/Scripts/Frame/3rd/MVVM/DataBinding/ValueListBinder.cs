using System;
using Huge.Pool;
using UnityEditor;
using UnityEngine;
using Huge.MVVM;
using System.ComponentModel;
using System.Collections.Specialized;

namespace Huge.MVVM.DataBinding
{
    public class ValueListBinder : IBinder, IReuseObject
    {
        static readonly ObjectPool<ValueListBinder> s_Pool = new ObjectPool<ValueListBinder>(null, l => l.OnRelease(), false);
        public static void Release(ValueListBinder toRelease) => s_Pool.Release(toRelease);
        public static ValueListBinder Get() => s_Pool.Get();

        bool m_bIsDirty = true;
        bool m_bIsBuilded = false;
        IProxy Proxy {get;set;}
        string PropertyName { get; set; }
        public INotifyPropertyChanged Source {get; set;}

        INotifyCollectionChanged List {get; set;}
        Func<INotifyCollectionChanged> Action {get; set;}
        NotifyCollectionChangedEventHandler Handler {get; set;}

        public ValueListBinder()
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

        public ValueListBinder For(Func<INotifyCollectionChanged> func, string propertyName)
        {
            PropertyName = propertyName;
            Action = func;
            return this;
        }

        public void To(NotifyCollectionChangedEventHandler handler)
        {
            Handler = handler;
        }

        public void Update()
        {
            if (m_bIsDirty && m_bIsBuilded)
            {
                m_bIsDirty = false;
            }
        }

        public void Build()
        {
            if (!m_bIsBuilded)
            {
                m_bIsBuilded = true;
                Source.PropertyChanged += OnPropertyChanged;
                var list = Action();
                if (list != null)
                {
                    list.CollectionChanged += Handler;
                }
                List = list;
            }
        }

        public void UnBuild()
        {
            if (m_bIsBuilded)
            {
                m_bIsBuilded = false;
                Source.PropertyChanged -= OnPropertyChanged;
                if (List != null)
                {
                    List.CollectionChanged -= Handler;
                }
                List = null;
            }
        }

        void OnPropertyChanged(object sender, PropertyChangedEventArgs evt)
        {
            if (evt.PropertyName == PropertyName)
            {
                m_bIsDirty = true;
                var list = Action();
                if (List != null)
                {
                    List.CollectionChanged -= Handler;
                }
                if (list != null)
                {
                    list.CollectionChanged += Handler;
                }
                List = list;
            }
        }
    }
}