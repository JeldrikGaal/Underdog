using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class ToyBin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("toy"))
        {
            ToyEnteredBin(other.gameObject);
        }
    }

    private void ToyEnteredBin(GameObject toy)
    {
        Vector3 punch = new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f), Random.Range(-1, 1f)) * 5;
        transform.DOPunchRotation(punch, 0.5f, 10, 1);
        Destroy(toy);
    }
}
