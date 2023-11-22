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
        Input.multiTouchEnabled = true;//开启多点触碰
        targetT = transform.gameObject.GetComponent<RectTransform>();
    }
    private void Update()
    {
        //不是双指就关闭
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

        //初始化
        if (Input.touchCount == 2 && !isInit)
        {
            //两指点位
            touch1 = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            touch2 = Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position);

            //目标初始点位
            oriPos = new Vector3(targetT.position.x, targetT.position.y, 0);

            //两指中点
            pos = new Vector3((Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x + Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position).x) / 2, (Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y + Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position).y) / 2, 0);

            //两指中点和目标距离
            disX = pos.x - oriPos.x;
            disY = pos.y - oriPos.y;
            oriScale = targetT.localScale.x;


            isInit = true;
        }

        if (Input.touchCount == 2)
        {
            //两指缩放比例
            scale = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position)) / Vector2.Distance(touch1, touch2);

            //利用scaleSpeed控制缩放速度
            scale = (scale - 1) * scaleSpeed;

            //给缩放比例加限制
            if (isLimitScale && targetT.localScale.x <= min)
            {
                isMin = true;
                return;
            }

            if (isLimitScale && targetT.localScale.x >= max && scale > 0)
            {
                return;
            }



            //缩放目标大小
            targetT.localScale = new Vector2(oriScale + scale * 0.1f, oriScale + scale * 0.1f);
            //改变目标位置，让位置保持不变
            //targetT.position = new Vector2(oriPos.x - ((targetT.localScale.x - oriScale) * disX), oriPos.y - ((targetT.localScale.y - oriScale) * disY));
        }
    }
}
