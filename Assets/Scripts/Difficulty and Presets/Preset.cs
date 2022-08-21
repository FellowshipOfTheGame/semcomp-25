using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preset : MonoBehaviour
{
    /*
     int numberOfEnemies =>enemies.Count;
     int numberOfPlayers =>players.Count;
    */
    private List<GameObject> enemies=new List<GameObject>();
    private List<GameObject> players=new List<GameObject>();
    [SerializeField] Transform enemiesTransf;
    [SerializeField] Transform playersTransf;
    [SerializeField] private Transform goal;

    Transform firstObj;
    Transform lastObj;

    [SerializeField] bool randomizePos = false;
    public float SpawnPos => lastObj.position.y;

    public float BallPosX => firstObj.position.x;
    public GameObject FirstPlayer => firstObj.gameObject;
    
    // Start is called before the first frame update
    void Awake()
    {
        float firstY = float.MaxValue;
        float lastY = 0;
        foreach(Transform child in enemiesTransf)
        {
            enemies.Add(child.gameObject);
            Vector2 pos = child.transform.position;
            if (pos.y < firstY)
            {
                firstY = pos.y;
                firstObj = child.transform;
            }
            if (pos.y > lastY)
            {
                lastY = pos.y;
                lastObj = child.transform;
            }
        }
        foreach (Transform child in playersTransf)
        {
            players.Add(child.gameObject);
            Vector2 pos = child.transform.position;
            if (pos.y < firstY)
            {
                firstY = pos.y;
                firstObj = child.transform;
            }
            if (pos.y > lastY)
            {
                lastY = pos.y;
                lastObj = child.transform;
            }
        }

        if (goal)
        {
            lastObj = goal.transform;
        }

        if (randomizePos)
        {
            RandomizePlayerPos();
        }
    }
    void RandomizePlayerPos()
    {
        foreach(GameObject player in players)
        {
            if (player.GetComponent<PlayerController>() != null)
            {

                float r = player.GetComponent<PlayerController>().Range;
                float randomX = Random.Range(-r, r);
                player.transform.position = new Vector2(randomX, player.transform.position.y);

            }
        }
    }

    public bool HasGoal()
    {
        return (goal != null);
    }

    public Vector3 GetGoalPosition()
    {
        return goal.transform.position;
    }
}
