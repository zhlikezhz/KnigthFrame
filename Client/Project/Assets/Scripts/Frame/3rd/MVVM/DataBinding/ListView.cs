using System;
using UnityEngine;
using Huge.MVVM;
using System.Collections.Specialized;

namespace Huge.MVVM.DataBinding
{
    public class ListView : View, IListView
    {
        public virtual void Clear()
        {

        }

        public virtual void AddItem(int index, ViewModel item)
        {

        }
        
        public virtual void RemoveItem(int index, ViewModel item)
        {

        }

        public virtual void MoveItem(int oldIndex, int newIndex, ViewModel item)
        {

        }

        public virtual void ReplaceItem(int index, ViewModel oldItem, ViewModel newItem)
        {

        }

        public void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    Clear();
                    break;
                case NotifyCollectionChangedAction.Add:
                    AddItem(e.NewStartingIndex, e.NewItems[0] as ViewModel);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveItem(e.NewStartingIndex, e.OldItems[0] as ViewModel);
                    break;
                case NotifyCollectionChangedAction.Move:
                    MoveItem(e.OldStartingIndex, e.NewStartingIndex, e.NewItems[0] as ViewModel);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ReplaceItem(e.OldStartingIndex, e.OldItems[0] as ViewModel, e.NewItems[0] as ViewModel);
                    break;
            }
        }
    }
}