using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public GameObject GO;
    public bool camShake;
    public AnimationCurve shakeCurve;
    
    [Space(20)]
    public float delay = 0.5f;
    public float destroyAfter = 2f;
    
    private GameObject mainCam;
    private bool eventTriggered = false;

    void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !eventTriggered)
        {
            eventTriggered = true;
            StartCoroutine(TriggerEvent(delay));
        }
    }

    IEnumerator TriggerEvent(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        GO.SetActive(true);
        StartCoroutine(mainCam.GetComponent<CamShake>().CustomShake(shakeCurve));
        
        Destroy(GO, destroyAfter);
        Destroy(this.gameObject, destroyAfter);
    }

}
