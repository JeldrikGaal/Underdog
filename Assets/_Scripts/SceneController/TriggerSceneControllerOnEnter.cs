using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerSceneControllerOnEnter : MonoBehaviour
{
    [SerializeField] private UnityEvent _onEnter;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _onEnter.Invoke();
            Destroy(gameObject);
        }
    }
}
