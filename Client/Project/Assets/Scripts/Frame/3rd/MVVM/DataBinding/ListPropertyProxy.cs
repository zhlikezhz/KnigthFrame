using System;
using Huge.Pool;

namespace Huge.MVVM.DataBinding
{
    public class ListPropertyProxy<TValue> : IProxy, IReuseObject where TValue : ViewModel
    {
        static readonly ObjectPool<ListPropertyProxy<TValue>> s_Pool = new ObjectPool<ListPropertyProxy<TValue>>(l => l.OnReinit(), l => l.OnRelease(), false);
        public static void Release(ListPropertyProxy<TValue> toRelease) => s_Pool.Release(toRelease);
        public static ListPropertyProxy<TValue> Get() => s_Pool.Get();

        ListView<TValue> Target {get; set;}
        ObservableList<TValue> Source {get; set;}
        public Func<ObservableList<TValue>> GetSourceDelegate {get;set;}

        public void To(ListView<TValue> view)
        {
            Target = view;
        }

        public void Update()
        {
            var source = GetSourceDelegate();
            if (Source != source)
            {
                if (Source != null)
                {
                    Source.CollectionChanged -= Target.OnCollectionChanged;
                }
                if (source != null)
                {
                    source.CollectionChanged += Target.OnCollectionChanged;

                    Target.Clear();
                    for (int i = 0; i < Source.Count; i++)
                    {
                        TValue value = Source[i];
                        Target.AddItem(i, value);
                    }
                }
                Source = source;
            }
        }

        void OnReinit()
        {

        }

        void OnRelease()
        {
            Source = null;
            Target = null;
            GetSourceDelegate = null;
        }

        public void Release()
        {
            Release(this);
        }
    }
}