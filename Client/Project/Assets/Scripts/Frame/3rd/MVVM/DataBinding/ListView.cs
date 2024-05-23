using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using Huge.MVVM;

namespace Huge.MVVM.DataBinding
{
    public class ListView<TView> : SubView, IListView
        where TView : ItemView
    {
        public Window Parent {get; set;}
        public ScrollRect Scroll {get; set;}
        public GameObject Content {get; set;}
        public GameObject Template {get; set;}
        List<ItemView> m_ItemList = new List<ItemView>();
        Stack<ItemView> m_ItemCacheList = new Stack<ItemView>();

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
                var itemView = m_ItemList[i];
                Parent.RemoveSubView(itemView, false);
                itemView.SetParent(Scroll.transform, false);
                itemView.SetActive(false);
                itemView.RemoveViewModel();
                m_ItemCacheList.Push(itemView);
            }
            m_ItemList.Clear();
        }

        public virtual void AddItem(ViewModel item)
        {
            if (m_ItemCacheList.TryPop(out var itemView))
            {
                Parent.AddSubView(itemView);
            }
            else
            {
                GameObject inst = GameObject.Instantiate(Template);
                itemView = Parent.AddSubView<TView>(inst);
                m_ItemList.Add(itemView);
            }
            itemView.SetParent(Content.transform, false);
            itemView.SetActive(true);
            itemView.SetAsLastSibling();
            itemView.ReplaceViewModel(item);
        }
        
        public virtual void RemoveItem(int index, ViewModel item)
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

        public virtual void InsertItem(int index, ViewModel item)
        {
            if (index == m_ItemList.Count)
            {
                AddItem(item);
            }
            else if (0 <= index && index < m_ItemList.Count)
            {
                if (m_ItemCacheList.TryPop(out var itemView))
                {
                    Parent.AddSubView(itemView);
                    itemView.SetActive(true);
                }
                else
                {
                    GameObject inst = GameObject.Instantiate(Template);
                    itemView = Parent.AddSubView<TView>(inst);
                    m_ItemList.Add(itemView);
                }
                itemView.SetParent(Content.transform, false);
                itemView.SetActive(true);
                itemView.SetSiblingIndex(index);
                itemView.ReplaceViewModel(item);
            }
        }

        public virtual void ReplaceItem(int index, ViewModel newItem)
        {
            if (0 <= index && index < m_ItemList.Count)
            {
                var itemView = m_ItemList[index];
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
                    AddItem(e.NewItems[0] as ViewModel);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveItem(e.NewStartingIndex, e.OldItems[0] as ViewModel);
                    break;
                case NotifyCollectionChangedAction.Move:
                    InsertItem(e.NewStartingIndex, e.NewItems[0] as ViewModel);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ReplaceItem(e.OldStartingIndex, e.NewItems[0] as ViewModel);
                    break;
            }
        }
    }
}