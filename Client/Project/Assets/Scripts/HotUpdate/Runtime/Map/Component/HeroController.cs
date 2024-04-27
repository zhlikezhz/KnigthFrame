using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HeroController : MonoBehaviour
{
    float moveSpeed = 0.006f;
    SpineRenderer spineRenderer;
    Queue<KeyCode> keyCodeList = new Queue<KeyCode>();
    Dictionary<KeyCode, Action<KeyCode>> opActionList = new Dictionary<KeyCode, Action<KeyCode>>();
    void Awake()
    {
        spineRenderer = GetComponent<SpineRenderer>();
        opActionList.Add(KeyCode.W, Move);
        opActionList.Add(KeyCode.S, Move);
        opActionList.Add(KeyCode.A, Move);
        opActionList.Add(KeyCode.D, Move);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            spineRenderer.PlayAnimation("run", true);
            transform.localPosition = transform.localPosition + new Vector3(0, moveSpeed, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            spineRenderer.PlayAnimation("run", true);
            transform.localPosition = transform.localPosition + new Vector3(0, -moveSpeed, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            spineRenderer.PlayAnimation("run", true);
            transform.localPosition = transform.localPosition + new Vector3(-moveSpeed, 0.0f, 0);
            transform.localScale = new Vector3(-0.2f, 0.2f, 0.2f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            spineRenderer.PlayAnimation("run", true);
            transform.localPosition = transform.localPosition + new Vector3(moveSpeed, 0.0f, 0);
            transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        else
        {
            spineRenderer.PlayAnimation("stand", true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        UnityEngine.Debug.LogError(collision.gameObject.name);
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        UnityEngine.Debug.LogError(collision.gameObject.name);
    }

    void OnCollisionStay2D(Collision2D collision)  
    {

    }

    void Stand()
    {

    }

    void Move(KeyCode key)
    {

    }
}