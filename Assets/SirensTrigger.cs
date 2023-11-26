using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirensTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<AudioSource>().Play();
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
