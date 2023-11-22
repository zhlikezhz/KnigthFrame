using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleByTwoFinger : MonoBehaviour
{
    private RectTransform targetT;
    bool isInit;
    Vector2 touch1, touch2, oriPos, pos;
    float disX, disY;
    float scale, oriScale;
    public float scaleSpeed = 1;

    public bool isLimitScale;
    private bool isMin = false;
    public float min, max;
    private float lastV = 0f;
    private void Start()
    {
        Input.multiTouchEnabled = true;//������㴥��
        targetT = transform.gameObject.GetComponent<RectTransform>();
    }
    private void Update()
    {
        //����˫ָ�͹ر�
        if (Input.touchCount != 2)
        {
            if (isMin)
            {
                lastV += Time.deltaTime;
              
                if (lastV >= 1)
                {
                    lastV = 1;
                    isMin = false;
                }
                targetT.localScale = new Vector2(lastV, lastV);

            }
            isInit = false;
        }

        //��ʼ��
        if (Input.touchCount == 2 && !isInit)
        {
            //��ָ��λ
            touch1 = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            touch2 = Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position);

            //Ŀ���ʼ��λ
            oriPos = new Vector3(targetT.position.x, targetT.position.y, 0);

            //��ָ�е�
            pos = new Vector3((Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x + Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position).x) / 2, (Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y + Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position).y) / 2, 0);

            //��ָ�е��Ŀ�����
            disX = pos.x - oriPos.x;
            disY = pos.y - oriPos.y;
            oriScale = targetT.localScale.x;


            isInit = true;
        }

        if (Input.touchCount == 2)
        {
            //��ָ���ű���
            scale = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position)) / Vector2.Distance(touch1, touch2);

            //����scaleSpeed���������ٶ�
            scale = (scale - 1) * scaleSpeed;

            //�����ű���������
            if (isLimitScale && targetT.localScale.x <= min)
            {
                isMin = true;
                return;
            }

            if (isLimitScale && targetT.localScale.x >= max && scale > 0)
            {
                return;
            }



            //����Ŀ���С
            targetT.localScale = new Vector2(oriScale + scale * 0.1f, oriScale + scale * 0.1f);
            //�ı�Ŀ��λ�ã���λ�ñ��ֲ���
            //targetT.position = new Vector2(oriPos.x - ((targetT.localScale.x - oriScale) * disX), oriPos.y - ((targetT.localScale.y - oriScale) * disY));
        }
    }
}
