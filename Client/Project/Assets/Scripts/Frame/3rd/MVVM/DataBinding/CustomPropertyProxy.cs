using System;
using Huge.Pool;
using System.ComponentModel;

namespace Huge.MVVM.DataBinding
{
    public class CustomPropertyProxy<TValue> : IProxy, IReuseObject
        where TValue : ObservableObject
    {
        static readonly ObjectPool<CustomPropertyProxy<TValue>> s_Pool = new ObjectPool<CustomPropertyProxy<TValue>>(l => l.OnReinit(), l => l.OnRelease(), false);
        public static void Release(CustomPropertyProxy<TValue> toRelease) => s_Pool.Release(toRelease);
        public static CustomPropertyProxy<TValue> Get() => s_Pool.Get();

        TValue Value {get; set;}
        public Func<TValue> GetValue {get;set;}
        public Action<TValue> SetValue {get;set;}
        public PropertyChangedEventHandler Handler {get; set;}

        public void To(Action<TValue> action)
        {
            SetValue = action;
        }

        public void Update()
        {
            if (GetValue != null && SetValue != null)
            {
                TValue value = GetValue();
                if (value != null) SetValue(value);
                if (value != null)
                {
                    if (value != Value)
                    {
                        if (Value != null)
                        {
                            Value.PropertyChanged -= Handler;
                        }
                        value.PropertyChanged += Handler;
                    }
                }
                else
                {
                    if (Value != null)
                    {
                        Value.PropertyChanged -= Handler;
                    }
                }
                Value = value;
            }
        }

        void OnReinit()
        {

        }

        void OnRelease()
        {
            GetValue = null;
            SetValue = null;
            if (Value != null)
            {
                Value.PropertyChanged -= Handler;
            }
        }

        public void Release()
        {
            Release(this);
        }
    }
}