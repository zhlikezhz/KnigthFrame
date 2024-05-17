using System;
using UnityEngine;
using System.Collections.Specialized;

namespace Huge.MVVM.DataBinding
{
    public interface IListView
    {
        void Clear();
        void AddItem(int index, ViewModel item);
        void RemoveItem(int index, ViewModel item);
        void MoveItem(int oldIndex, int newIndex, ViewModel item);
        void ReplaceItem(int index, ViewModel oldItem, ViewModel newItem);
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e);
    }
}
