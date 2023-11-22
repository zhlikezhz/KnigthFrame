using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using XLua;
using LuaFrame;
namespace UnityEngine.UI
{

    public abstract class LoopScrollRect : LoopScrollRectBase
    {


        protected override RectTransform GetFromTempPool(int itemIdx)
        {
            RectTransform nextItem = null;
            if (deletedItemTypeStart > 0 && m_Content.childCount > 0)
            {
                deletedItemTypeStart--;
                nextItem = m_Content.GetChild(0) as RectTransform;
                nextItem.SetSiblingIndex(itemIdx - itemTypeStart + deletedItemTypeStart);
            }
            else if (deletedItemTypeEnd > 0 && m_Content.childCount > 0)
            {
                deletedItemTypeEnd--;
                nextItem = m_Content.GetChild(m_Content.childCount - 1) as RectTransform;
                nextItem.SetSiblingIndex(itemIdx - itemTypeStart + deletedItemTypeStart);
            }
            else
            {
                nextItem = this.InstantiatePrefab(itemIdx);  //prefabSource.GetObject(itemIdx).transform as RectTransform;
                nextItem.transform.SetParent(m_Content, false);
                nextItem.gameObject.SetActive(true);
            }
            ProvideData(nextItem, itemIdx);
            return nextItem;
        }

        protected override void ReturnToTempPool(bool fromStart, int count)
        {
            if (fromStart)
                deletedItemTypeStart += count;
            else
                deletedItemTypeEnd += count;
        }

        protected override void ClearTempPool()
        {
            Debug.Assert(m_Content.childCount >= deletedItemTypeStart + deletedItemTypeEnd);
            if (deletedItemTypeStart > 0)
            {
                for (int i = deletedItemTypeStart - 1; i >= 0; i--)
                {
                    this.ReturnObject(m_Content.GetChild(i));
                }
                deletedItemTypeStart = 0;
            }
            if (deletedItemTypeEnd > 0)
            {
                int t = m_Content.childCount - deletedItemTypeEnd;
                for (int i = m_Content.childCount - 1; i >= t; i--)
                {
                    this.ReturnObject(m_Content.GetChild(i));
                }
                deletedItemTypeEnd = 0;
            }
        }
    }
}