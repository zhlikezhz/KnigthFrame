using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIImageAdapt : MonoBehaviour
{
    private void Start()
    {
        RectTransform rectTrans = this.transform.GetComponent<RectTransform>();
        float newW = rectTrans.rect.width;
        float newH = rectTrans.rect.height;
        if (Root.designMatch == 1f)
        {
            //宽屏（先计算缩放后的画布宽度，再按比例缩放高度）
            newW = Root.designHeight * Screen.width / Screen.height;
            float scale = newW / rectTrans.rect.width;
            newH = rectTrans.rect.height * scale;
        }
        else
        {
            //长屏（宽不变高等比缩放）
            newW = Root.designWidth;
            float scale = newW / rectTrans.rect.width;
            newH = rectTrans.rect.height * scale;
        }
        rectTrans.sizeDelta = new Vector2(newW, newH);
    }
}