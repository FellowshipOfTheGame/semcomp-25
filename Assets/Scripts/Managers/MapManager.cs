using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    //[SerializeField] List<GameObject> presetPrefabs;
    [SerializeField] List<Preset> presetsOnMap;
    [SerializeField] Transform goal;

    [SerializeField] private bool onTransition=false;
    [SerializeField] Transform presetSpawner;
    [SerializeField] private float targetY=-3f;
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private GameObject fx;
    [SerializeField] Transform ballTransf;
    float posNextSpawn = -4f;
    float spawnOffset = 2f;
    Transform currPlayer;

    /* Level Control */
    int playersPassed = 0;
    int totalPlayersToPass = 0;
    int currLevel = 1;

    private GameManager manager;
    private DifficultyProgression difficultyProgression;

    private void Awake()
    {
        manager = FindObjectOfType<GameManager>();
        difficultyProgression = FindObjectOfType<DifficultyProgression>();
    }
    [SerializeField] private List<GameObject> goalPrefabs;
    [SerializeField] private Transform goalTransf;
    //private bool goalSpawned = false;
    //private Transform removedAlly;

    public void SetBallFx(bool val)
    {
        fx.SetActive(val);
    }
    private void Start()
    {
        SpawnPreset();
        SpawnPreset();
        SpawnPreset();
        //SpawnPreset();

        Vector2 pos = ballTransf.position;
        pos.x = presetsOnMap[0].BallPosX;
        ballTransf.position = pos;
        totalPlayersToPass = difficultyProgression.PlayersOnLevel;
    }
    private void SpawnPreset()
    {
        int presetCount = presetsOnMap.Count;
        if (presetCount > 0)
        {
            posNextSpawn = presetsOnMap[presetCount-1].SpawnPos+ spawnOffset;
        }
        Vector2 pos = new Vector2(0, posNextSpawn);

        //int id = Random.Range(0, presetPrefabs.Count);
        //GameObject obj=Instantiate(presetPrefabs[id], pos, Quaternion.identity, presetSpawner);
        GameObject obj = Instantiate(difficultyProgression.NextPreset(), pos, Quaternion.identity, presetSpawner);

        Preset preset = obj.GetComponent<Preset>();
        presetsOnMap.Add(preset);
  
    }
    IEnumerator Transition()
    {
        yield return new WaitForSeconds(0.1f);
        onTransition = true;
        fx.SetActive(false);
        int points=((int)Mathf.Abs(currPlayer.position.y - targetY)/4);
        playersPassed += points;
        if (playersPassed >= totalPlayersToPass)
        {
            playersPassed = points - 1;
            totalPlayersToPass = difficultyProgression.PlayersOnLevel;
            currLevel++;
            manager.PassLevel();
            manager.SetLevelProgress(1f);
        }
        else
        manager.SetLevelProgress((float)playersPassed / totalPlayersToPass);

        Vector3 pos=transform.position;
        while (Mathf.Abs(currPlayer.position.y-targetY)>0.1f)
        {
            pos = transform.position;
            pos.y -= speed*Time.deltaTime;
            transform.position = pos;
            yield return null;
        }
        pos.y =Mathf.Round(pos.y);
        transform.position = pos;
        
        onTransition = false;
        manager.AddPoint(points);
        if (presetsOnMap[0].SpawnPos < targetY)
        {
            SpawnPreset();
            Destroy(presetsOnMap[0].gameObject);
            presetsOnMap.RemoveAt(0);
        }
        if(fx!=null)
        fx.SetActive(true);
    }

    public void SpawnGoal(float _y)
    {
        int numberOfGoals = goalPrefabs.Count;

        // Generate Goal object
        GameObject obj = Instantiate(goalPrefabs[Random.Range(0, numberOfGoals)], goalTransf);
        Vector3 pos = obj.transform.position;
        pos.y = _y;
        obj.transform.position = pos;

        // Set the goal transform
        goal = obj.transform;

        // Deactivate ally object where the goal is
        /*for (int i = 0; i < allies.Count; i++)
        {
            Transform ally = allies[i];
            //if (Vector3.Distance(ally.position, goal.position) < 0.1f)
            if (Mathf.Abs(ally.position.y - goal.position.y) < 0.1f)
            {
                //Destroy(ally.gameObject);
                //allies.RemoveAt(i);
                ally.gameObject.SetActive(false);
                goalSpawned = true;
                removedAlly = ally;
            }
        }*/

        obj.SetActive(true);
    }

    // Delete the Goal gameobject and 
    public void StartDeleteGoalTransition()
    {
        Destroy(goal.gameObject);
        //Debug.Log(removedAlly.gameObject.name);
        //removedAlly.gameObject.SetActive(true);

        //goalSpawned = false;
        //StartTransition(removedAlly.parent);
    }

    public void StartTransition(Transform newPlayer)
    {
        currPlayer = newPlayer;
        StartCoroutine(Transition());
    }

    /*public Transform RemovedAllyTransform()
    {
        return removedAlly.GetChild(0);
    }*/
}
