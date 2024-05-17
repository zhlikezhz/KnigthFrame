using System;
using Huge.Pool;

namespace Huge.MVVM.DataBinding
{
    public class PropertyProxy<TValue> : IProxy, IReuseObject
    {
        static readonly ObjectPool<PropertyProxy<TValue>> s_Pool = new ObjectPool<PropertyProxy<TValue>>(l => l.OnReinit(), l => l.OnRelease(), false);
        public static void Release(PropertyProxy<TValue> toRelease) => s_Pool.Release(toRelease);
        public static PropertyProxy<TValue> Get() => s_Pool.Get();

        public Func<TValue> GetValue {get;set;}
        public Action<TValue> SetValue {get;set;}

        public void To(Action<TValue> action)
        {
            SetValue = action;
        }

        public void Update()
        {
            if (GetValue != null && SetValue != null)
            {
                SetValue(GetValue());
            }
        }

        void OnReinit()
        {

        }

        void OnRelease()
        {
            GetValue = null;
            SetValue = null;
        }

        public void Release()
        {
            Release(this);
        }
    }
}