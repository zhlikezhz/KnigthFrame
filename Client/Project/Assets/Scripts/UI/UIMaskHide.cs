using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMaskHide : MonoBehaviour
{
    [SerializeField]
    public float startX;
    [SerializeField]
    public float length;
    [SerializeField]
    public float speed = 5;
    [SerializeField]
    public Material material;

    private float moveX;
    private bool playAni = false;

    private void Start()
    {
        if (material == null)
            material = gameObject.GetComponent<Image>().material;
    }

    private void OnEnable()
    {
        PlayAni();
    }

    private void PlayAni()
    {
        moveX = 0;
        playAni = true;
    }    

    private void Update()
    {
        if (playAni == false)
            return;
        if (moveX > length)
        {
            playAni = false;
            return;
        }
        moveX = moveX + speed;
        material.SetFloat("_StartX", startX + moveX);
    }
}