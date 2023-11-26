using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DudeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _dudePrefab;
    [SerializeField] private Vector2 _spawnIntervalRange;


    private void Start()
    {
        Invoke(nameof(SpawnDudeRecursive), GetRandomTime());
    }

    private void SpawnDudeRecursive()
    {
        SpawnDude();
        Invoke(nameof(SpawnDudeRecursive),GetRandomTime());
    }

    private float GetRandomTime()
    {
        return Random.Range(_spawnIntervalRange.x, _spawnIntervalRange.y);
    }
    
    private void SpawnDude()
    {
        GameObject dude = Instantiate(_dudePrefab, transform.position, Quaternion.identity);
        dude.transform.forward = transform.right;
        Destroy(dude, 10f);
    }
}
