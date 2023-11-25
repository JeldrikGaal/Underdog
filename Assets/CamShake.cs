using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class CamShake : MonoBehaviour
{
    public bool start = false;
    public List<AnimationCurve> curves;
    public float duration = 1f;

    private void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shaking());
        }
    }

    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            AnimationCurve curve = curves[UnityEngine.Random.Range(0, curves.Count - 1)];
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + UnityEngine.Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = startPosition;
    }
}
