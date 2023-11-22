using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/LongButton", 30)]
    public class LongButton : Button
    {
        [Serializable]
        public class LongPressEvent : UnityEvent<int> { }

        [FormerlySerializedAs("longPress")]
        [SerializeField]
        private LongPressEvent m_LongPress = new LongPressEvent();


        [Tooltip("How long must pointer be down on this object to trigger a long press")]
        public float durationThreshold = 1.0f;



        private bool longPressTriggered = false;
        private float timePressStarted;
        private bool isPointerDown = false;

        public LongPressEvent onLongPress
        {
            get { return m_LongPress; }
            set { m_LongPress = value; }
        }
        protected LongButton()
        { }

        private void Update()
        {
            if (interactable && this.isPointerDown && !longPressTriggered)
            {
                if (Time.time - timePressStarted > durationThreshold)
                {
                    longPressTriggered = true;
                    m_LongPress.Invoke(1);
                }
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (IsActive() && interactable)
            {
                timePressStarted = Time.time;
                isPointerDown = true;
                longPressTriggered = false;
            }

        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            isPointerDown = false;
            m_LongPress.Invoke(0);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            isPointerDown = false;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!longPressTriggered)
            {
                base.OnPointerClick(eventData);
            }
        }
    }



}


