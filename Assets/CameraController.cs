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

    private void Start()
    {
        lastX = target.position.x;
    }

    private void Update()
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
