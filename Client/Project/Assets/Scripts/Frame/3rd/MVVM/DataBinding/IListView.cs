using System;
using System.Collections.Specialized;
using UnityEngine.UI;
using UnityEngine;
using Huge.MVVM;

namespace Huge.MVVM.DataBinding
{
    public interface IListView
    {
        Window Parent {get; set;}
        ScrollRect Scroll {get; set;}
        GameObject Content {get; set;}
        GameObject Template {get; set;}
        void Reset();
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e);
    }
}