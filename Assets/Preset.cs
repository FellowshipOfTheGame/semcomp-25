using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preset : MonoBehaviour
{

    [SerializeField] int numberOfEnemies = 0;
    [SerializeField] int numberOfPlayers = 0;

    public int TotalObjs => numberOfEnemies + numberOfPlayers;
    private List<GameObject> enemies=new List<GameObject>();
    private List<GameObject> players=new List<GameObject>();
    [SerializeField] Transform enemiesTransf;
    [SerializeField] Transform playersTransf;

    [SerializeField] Transform lastObj;
    public float SpawnPos => lastObj.position.y;
    // Start is called before the first frame update
    void Awake()
    {
        foreach(Transform child in enemiesTransf)
        {
            enemies.Add(child.gameObject);
        }
        foreach (Transform child in playersTransf)
        {
            players.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
