using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Huge.MVVM.DataBinding
{
    public class UnityEventProxy<TDelegate> : IProxy
    {
        public string PropertyName {get;set;}
        public TDelegate OnClick {get;set;}
        public TDelegate OnAction {get;set;}

        Button button;
        InputField input;
        public void To(TDelegate action)
        {
            OnAction = action;
        }

        public void Update()
        {

        }
    }
}