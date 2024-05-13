using System;
using UnityEngine;
using System.Collections.Specialized;

namespace Huge.MVVM.DataBinding
{
    public interface IListView
    {
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e);
    }
}
