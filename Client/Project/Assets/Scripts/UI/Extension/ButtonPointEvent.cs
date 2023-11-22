using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using XLua;
using DG.Tweening;

public class ButtonPointEvent : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler, IPointerExitHandler
{
    public float toScale = 0.9f;
    public float aniTime = 0.1f;
    private Vector3 preScale = Vector3.one;

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        preScale = gameObject.transform.localScale;
        gameObject.transform.DOScale(preScale * toScale, aniTime).SetEase(Ease.OutSine);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        gameObject.transform.DOScale(preScale, aniTime).SetEase(Ease.OutSine);
    }

    void OnDestroy()
    {

    }
}
