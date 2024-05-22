using System;
using System.Collections.Specialized;

namespace Huge.MVVM.DataBinding
{
    public interface IListView
    {
        void Reset();
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e);
    }
}