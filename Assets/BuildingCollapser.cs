using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollapser : MonoBehaviour
{

    [SerializeField] private float destroyAfter;
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject particles;
    
    public void StartCollapseSequence()
    {
        animator.SetTrigger("collapse");
        particles.SetActive(true);
        Destroy(this,destroyAfter);
    }
}
