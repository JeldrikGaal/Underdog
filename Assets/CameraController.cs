using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -10);
    public float smoothTime = 0.25f;
    Vector3 currentVelocity;

    private float lastX;

    public float targetXOffset = 3.82f;
    public float triggerDelay = 0.5f;

    private float timer = 0;

    [Space(20)] public bool xLimit1;
    public float x1Limit1 = 0;
    public float x2Limit1 = 0;
    
    [Space(20)] public bool xLimit2;
    public float x1Limit2 = 0;
    public float x2Limit2 = 0;

    [Space(20)] public bool xLimit3;
    public float x1Limit3 = 0;
    public float x2Limit3 = 0;

    private void Start()
    {
        lastX = target.position.x;
    }

    private void Update()
    {
        if (xLimit1 || xLimit2 || xLimit3)
        {
            if (xLimit1 && transform.position.x <= x1Limit1) { transform.position = new Vector3(x1Limit1,transform.position.y,transform.position.z); }
            else if (xLimit1 && transform.position.x >= x2Limit1) { transform.position = new Vector3(x2Limit1, transform.position.y,transform.position.z); }
            else if(xLimit2 && transform.position.x <= x1Limit2){transform.position = new Vector3(x1Limit2, transform.position.y,transform.position.z);}
            else if(xLimit2 && transform.position.x >= x2Limit2){transform.position = new Vector3(x2Limit2, transform.position.y,transform.position.z);} 
            else if(xLimit3 && transform.position.x <= x1Limit3){transform.position = new Vector3(x1Limit3, transform.position.y,transform.position.z);} 
            else if(xLimit3 && transform.position.x >= x2Limit3){transform.position = new Vector3(x2Limit3, transform.position.y,transform.position.z);} 
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= triggerDelay)
            {
                timer = 0;
                if (target.position.x > lastX)
                {
                    // Player walking to the right
                    offset.x = targetXOffset;

                }
                else if (target.position.x < lastX)
                {
                    // Player walking to the left
                    offset.x = -targetXOffset;
                }
                else
                {
                    // Player stopped walking.
                    offset.x = 0;
                }

                lastX = target.position.x;
            }
        }
    }

    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            target.position + offset, 
            ref currentVelocity, 
            smoothTime
        );
    }
}
