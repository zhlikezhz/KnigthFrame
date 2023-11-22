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
            //�������ȼ������ź�Ļ�����ȣ��ٰ��������Ÿ߶ȣ�
            newW = Root.designHeight * Screen.width / Screen.height;
            float scale = newW / rectTrans.rect.width;
            newH = rectTrans.rect.height * scale;
        }
        else
        {
            //����������ߵȱ����ţ�
            newW = Root.designWidth;
            float scale = newW / rectTrans.rect.width;
            newH = rectTrans.rect.height * scale;
        }
        rectTrans.sizeDelta = new Vector2(newW, newH);
    }
}