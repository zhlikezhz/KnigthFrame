using System;
using Huge.Pool;
using System.Collections.Specialized;

namespace Huge.MVVM.DataBinding
{
    public class ListPropertyProxy<TViewModel> : IProxy, IReuseObject 
        where TViewModel : ViewModel
    {
        static readonly ObjectPool<ListPropertyProxy<TViewModel>> s_Pool = new ObjectPool<ListPropertyProxy<TViewModel>>(l => l.OnReinit(), l => l.OnRelease(), false);
        public static void Release(ListPropertyProxy<TViewModel> toRelease) => s_Pool.Release(toRelease);
        public static ListPropertyProxy<TViewModel> Get() => s_Pool.Get();

        IListView Target {get; set;}
        ObservableList<TViewModel> Source {get; set;}
        public Func<ObservableList<TViewModel>> GetSourceDelegate {get;set;}

        public void To(IListView view)
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

                    Target.Reset();
                    for (int i = 0; i < source.Count; i++)
                    {
                        TViewModel value = source[i];
                        var evtArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, i);
                        Target.OnCollectionChanged(Target, evtArgs);
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