using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMaskN : MonoBehaviour
{
    [SerializeField]
    public Vector3 leftDownPoint;
    [SerializeField]
    public Vector3 rightUpPoint;
    [SerializeField]
    public Material material;
    public float offsetX = 0;
    public float offsetY = 0;
    float width = 0;
    float heigth = 0;
    float scale_m = 1.5f;
    float step_x = 0;
    float step_y = 0;

    private void Start()
    {
        if (material == null)
            material = gameObject.GetComponent<Image>().material;
    }

    public void SetLeftDownPoint(float x=0, float y=0, float z=0)
    {
        leftDownPoint = new Vector3(x, y, z);
    }

    public void SetRightUpPoint(float x = 0, float y = 0, float z = 0)
    {
        rightUpPoint = new Vector3(x, y, z);
    }

    public void SetMaxScale(float scale)
    {
        scale_m = scale;
    }

    private void Update()
    {
        if (offsetX <= 0 && offsetY <= 0)
            return;

        float leftDown_x = leftDownPoint.x;
        float rightUp_x = rightUpPoint.x;
        if (offsetX > 0)
        {
            offsetX -= step_x;
            leftDown_x -= offsetX;
            rightUp_x += offsetX;
        }

        float leftDown_y = leftDownPoint.y;
        float rightUp_y = rightUpPoint.y;
        if (offsetY > 0)
        {
            offsetY -= step_y;
            leftDown_y -= offsetY;
            rightUp_y += offsetY;
        }
        material.SetVector("_LeftDown", new Vector4(leftDown_x, leftDown_y, leftDownPoint.z, 0));
        material.SetVector("_RightUp", new Vector4(rightUp_x, rightUp_y, rightUpPoint.z, 0));
    }

    //ÍÚ¿×
    public void SetHeloPoint()
    {
        width = rightUpPoint.x - leftDownPoint.x;
        heigth = rightUpPoint.y - leftDownPoint.y;
        offsetX = width * scale_m;
        offsetY = heigth * scale_m;
        step_x = offsetX / 30;
        step_y = offsetY / 30;
        if (width == 0 && heigth == 0)
        {
            material.SetVector("_LeftDown",Vector4.zero);
            material.SetVector("_RightUp", Vector4.zero);
        }
    }
}