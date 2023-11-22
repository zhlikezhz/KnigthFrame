using UnityEngine.EventSystems;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace UnityEngine.UI
{

    public class DoubleClikeButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IPointerExitHandler
    {

        [Serializable]
        public class DoubleClickEvent : UnityEvent { }
        [SerializeField]
        private DoubleClickEvent _onDoubleClick = new DoubleClickEvent();
        public DoubleClickEvent OnDoubleClick
        {
            get
            {
                return _onDoubleClick;
            }

            set
            {
                _onDoubleClick = value;
            }
        }
        private DateTime m_firstTime;
        private DateTime m_SecondTime;
        private void ResetTime()
        {
            m_firstTime = default(DateTime);
            m_SecondTime = default(DateTime);
        }
        private void Press()
        {
            if (OnDoubleClick != null)
                OnDoubleClick.Invoke();
            else
                ResetTime();
        }

        public  void OnPointerDown(PointerEventData eventData)
        {
            if (m_firstTime.Equals(default(DateTime)))
                m_firstTime = DateTime.Now;
            else
            {
                m_SecondTime = DateTime.Now;
            }
        }
        public  void OnPointerUp(PointerEventData eventData)
        {
            if (!m_firstTime.Equals(default(DateTime)) && !m_SecondTime.Equals(default(DateTime)))
            {
                var intervalTime = m_SecondTime - m_firstTime;
                float milliTime = intervalTime.Seconds * 1000 + intervalTime.Milliseconds;
                if (milliTime < 400)
                {
                    Press();
                }
                else
                    ResetTime();
            }

        }
        public  void OnPointerExit(PointerEventData eventData)
        {
            ResetTime();
        }
    }
}