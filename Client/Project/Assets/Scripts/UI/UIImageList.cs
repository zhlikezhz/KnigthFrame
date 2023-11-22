using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIImageList : MonoBehaviour
{
    [SerializeField]
    public Sprite[] list;

  


    public Sprite GetImageByName(string name)
    {
        if (list == null || list.Length == 0)
        {
            GameDebug.LogWarning("没有添加图片");
            return null;
        }
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].name == name)
                return list[i];
        }
        GameDebug.LogWarning("没有找到对应图片"+ name);
        return null;
    }
}