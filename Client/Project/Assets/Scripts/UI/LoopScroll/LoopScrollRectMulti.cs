using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    public abstract class LoopScrollRectMulti : LoopScrollRectBase
    {
       

        protected override RectTransform GetFromTempPool(int itemIdx)
        {
            RectTransform nextItem = this.InstantiatePrefab(itemIdx); 
            nextItem.transform.SetParent(m_Content, false);
            nextItem.gameObject.SetActive(true);

            ProvideData(nextItem, itemIdx);
            return nextItem;
        }

        protected override void ReturnToTempPool(bool fromStart, int count)
        {
            Debug.Assert(m_Content.childCount >= count);
            if (fromStart)
            {
                for (int i = count - 1; i >= 0; i--)
                {
                    ReturnObject(m_Content.GetChild(i));
                }
            }
            else
            {
                int t = m_Content.childCount - count;
                for (int i = m_Content.childCount - 1; i >= t; i--)
                {
                    ReturnObject(m_Content.GetChild(i));
                }
            }
        }

        protected override void ClearTempPool()
        {
        }
    }
}