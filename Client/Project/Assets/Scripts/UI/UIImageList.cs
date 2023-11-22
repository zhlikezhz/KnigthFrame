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
            GameDebug.LogWarning("û�����ͼƬ");
            return null;
        }
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].name == name)
                return list[i];
        }
        GameDebug.LogWarning("û���ҵ���ӦͼƬ"+ name);
        return null;
    }
}