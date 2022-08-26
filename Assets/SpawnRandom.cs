using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandom : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabs;
    GameObject objSpawned;
    public void Spawn()
    {
        objSpawned=Instantiate(prefabs[Random.Range(0, prefabs.Count)], transform);
    }

    void Awake()
    {
        Spawn();
    }

}
