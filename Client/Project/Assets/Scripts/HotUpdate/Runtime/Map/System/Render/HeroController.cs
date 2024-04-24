using System;
using System.Diagnostics;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    float moveSpeed = 0.003f;
    SpineRenderer spineRenderer;
    void Awake()
    {
        spineRenderer = GetComponent<SpineRenderer>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            spineRenderer.PlayAnimtion("walk", true);
            transform.localPosition = transform.localPosition + new Vector3(0, moveSpeed, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            spineRenderer.PlayAnimtion("walk", true);
            transform.localPosition = transform.localPosition + new Vector3(0, -moveSpeed, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            spineRenderer.PlayAnimtion("walk", true);
            transform.localPosition = transform.localPosition + new Vector3(-moveSpeed, 0.0f, 0);
            transform.localScale = new Vector3(-0.2f, 0.2f, 0.2f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            spineRenderer.PlayAnimtion("walk", true);
            transform.localPosition = transform.localPosition + new Vector3(moveSpeed, 0.0f, 0);
            transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        else
        {
            spineRenderer.PlayAnimtion("stand", true);
        }
    }
}