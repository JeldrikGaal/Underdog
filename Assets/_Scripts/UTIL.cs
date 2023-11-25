using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UTIL : MonoBehaviour
{
    public static bool IsColliderPlayer(Collider collider)
    {
        return collider.transform.CompareTag("Player");
    }
}
