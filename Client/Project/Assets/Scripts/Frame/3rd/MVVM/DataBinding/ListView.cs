using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using Huge.MVVM;

namespace Huge.MVVM.DataBinding
{
    public class ListView<TView, TViewModel> : SubView, IListView
        where TViewModel : ViewModel
        where TView : ItemView<TViewModel>
    {
        public Window Parent {get; set;}
        public ScrollRect Scroll {get; set;}
        public GameObject Content {get; set;}
        public GameObject Template {get; set;}
        List<TView> m_ItemList = new List<TView>();
        Stack<TView> m_ItemCacheList = new Stack<TView>();

        protected override void OnDestroy()
        {
            Reset();
            foreach(var view in m_ItemCacheList)
            {
                view.Destroy();
            }
            m_ItemCacheList.Clear();
        }

        public virtual void Reset()
        {
            for (int i = 0; i < m_ItemList.Count; i++)
            {
                var itemView = m_ItemList[i] as ItemView<TViewModel>;
                Parent.RemoveSubView(itemView, false);
                itemView.SetParent(Scroll.transform, false);
                itemView.SetActive(false);
                itemView.RemoveViewModel();
                m_ItemCacheList.Push(itemView as TView);
            }
            m_ItemList.Clear();
        }

        public virtual void AddItem(TViewModel item)
        {
            if (m_ItemCacheList.TryPop(out var itemView))
            {
                Parent.AddSubView(itemView, Content);
                itemView.SetActive(true);
            }
            else
            {
                GameObject inst = GameObject.Instantiate(Template);
                itemView = Parent.AddSubView<TView>(inst, Content);
                m_ItemList.Add(itemView);
            }
            itemView.SetAsLastSibling();
            var view = itemView as ItemView<TViewModel>;
            view.ReplaceViewModel(item);
        }
        
        public virtual void RemoveItem(int index, TViewModel item)
        {
            if (0 <= index && index < m_ItemList.Count)
            {
                var itemView = m_ItemList[index];
                m_ItemList.RemoveAt(index);
                Parent.RemoveSubView(itemView, false);
                itemView.SetParent(Scroll.transform, false);
                itemView.SetActive(false);
                itemView.RemoveViewModel();
                m_ItemCacheList.Push(itemView);
            }
        }

        public virtual void InsertItem(int index, TViewModel item)
        {
            if (index == m_ItemList.Count)
            {
                AddItem(item);
            }
            else if (0 <= index && index < m_ItemList.Count)
            {
                if (m_ItemCacheList.TryPop(out var itemView))
                {
                    Parent.AddSubView(itemView, Content);
                    itemView.SetActive(true);
                }
                else
                {
                    GameObject inst = GameObject.Instantiate(Template);
                    itemView = Parent.AddSubView<TView>(inst, Content);
                    m_ItemList.Add(itemView);
                }
                itemView.SetSiblingIndex(index);
                var view = itemView as ItemView<TViewModel>;
                view.ReplaceViewModel(item);
            }
        }

        public virtual void ReplaceItem(int index, TViewModel newItem)
        {
            if (0 <= index && index < m_ItemList.Count)
            {
                var itemView = m_ItemList[index] as ItemView<TViewModel>;
                itemView.RemoveViewModel();
                itemView.ReplaceViewModel(newItem);
            }
        }

        public virtual void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    Reset();
                    break;
                case NotifyCollectionChangedAction.Add:
                    AddItem(e.NewItems[0] as TViewModel);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveItem(e.NewStartingIndex, e.OldItems[0] as TViewModel);
                    break;
                case NotifyCollectionChangedAction.Move:
                    InsertItem(e.NewStartingIndex, e.NewItems[0] as TViewModel);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ReplaceItem(e.OldStartingIndex, e.NewItems[0] as TViewModel);
                    break;
            }
        }
    }
}