using System;
using UnityEngine;
using Huge.MVVM;
using System.Collections.Specialized;

namespace Huge.MVVM.DataBinding
{
    public class ListView<T> : View where T : ViewModel
    {
        public virtual void Clear()
        {

        }

        public virtual void AddItem(int index, T item)
        {

        }
        
        public virtual void RemoveItem(int index, T item)
        {

        }

        public virtual void MoveItem(int oldIndex, int newIndex, T item)
        {

        }

        public virtual void ReplaceItem(int index, T oldItem, T newItem)
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
                    AddItem(e.NewStartingIndex, e.NewItems[0] as T);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveItem(e.NewStartingIndex, e.OldItems[0] as T);
                    break;
                case NotifyCollectionChangedAction.Move:
                    MoveItem(e.OldStartingIndex, e.NewStartingIndex, e.NewItems[0] as T);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ReplaceItem(e.OldStartingIndex, e.OldItems[0] as T, e.NewItems[0] as T);
                    break;
            }
        }
    }
}