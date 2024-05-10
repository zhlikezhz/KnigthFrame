using System;

namespace Huge.MVVM.DataBinding
{
    public class PropertyProxy<TSource, TTarget, TValue> : IProxy
    {
        public TSource Source {get;set;}
        public TTarget Target {get;set;}
        public string PropertyName {get;set;}
        public Func<TSource, TValue> GetValue {get;set;}
        public Action<TTarget, TValue> SetValue {get;set;}

        public void To(Action<TTarget, TValue> action)
        {
            SetValue = action;
        }

        public void Update()
        {
            if (GetValue != null && SetValue != null)
            {
                SetValue(Target, GetValue(Source));
            }
        }
    }
}