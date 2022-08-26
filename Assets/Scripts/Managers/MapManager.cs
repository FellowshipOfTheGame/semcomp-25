using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    List<Preset> presetsOnMap = new List<Preset>();

    [SerializeField] Transform presetSpawner;
    [SerializeField] private float targetY=-3f;
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private GameObject fx;
    [SerializeField] Transform ballTransf;
    float posNextSpawn = -4f;
    float spawnOffset = 2f;
    Transform currPlayer;
    
    /* Level Control  */
    private int allyBarsPassed = 0;
    private int totalPlayersInLevel = 0;
    private List<GameObject> goalPositions = new List<GameObject>();
    private List<GameObject> firstPlayerInLevels = new List<GameObject>();

    /* Cached references */
    private GameManager gameManager;
    private DifficultyProgression difficultyProgression;
    private BallController ballController;
    
    /* Field related */
    [Header("Goal positioning")]
    [SerializeField] private float goalOffsetFromMiddle;
    [SerializeField] float startGoalPosition;
    private Field field;

    /* Pass event */
    public delegate void SuccessfulPass();
    public static event SuccessfulPass OnSuccessfulPass;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        difficultyProgression = FindObjectOfType<DifficultyProgression>();
        field = GetComponentInChildren<Field>();
        ballController = FindObjectOfType<BallController>();
    }

    public void SetBallFx(bool val)
    {
        fx.SetActive(val);
    }
    
    private void Start()
    {
        // spawn until a goal is spawned
        while (goalPositions.Count == 0)
        {
            SpawnPreset();
        }
        
        if (field != null)
        {
            RepositionField();
        }

        ballTransf.position = presetsOnMap[0].FirstPlayer.transform.position;
        totalPlayersInLevel = difficultyProgression.GetTotalPlayersInLevel(gameManager.Level);
    }

    private void SpawnPreset()
    {
        int presetCount = presetsOnMap.Count;
        if (presetCount > 0)
        {
            posNextSpawn = presetsOnMap[presetCount-1].SpawnPos+ spawnOffset;
        }
        Vector2 pos = new Vector2(0, posNextSpawn);

        GameObject obj = Instantiate(difficultyProgression.NextPreset(), pos, Quaternion.identity, presetSpawner);

        Preset preset = obj.GetComponent<Preset>();
        presetsOnMap.Add(preset);

        if (firstPlayerInLevels.Count == goalPositions.Count)
        {
            firstPlayerInLevels.Add(preset.FirstPlayer); 
        }
        
        // goal check
        if (preset.HasGoal())
        {
            goalPositions.Add(preset.GetGoal());
        }
    }

    private Transform GetGoalPositionOfLevel(int level)
    {
        if (goalPositions.Count > level)
            return goalPositions[level].transform;
        return null;
    }

    // reposition field to fit into current level presets and goal
    public void RepositionField()
    {
        float goalY = GetGoalPositionOfLevel(gameManager.Level).position.y;
        field.SetGoalPosition(startGoalPosition, goalY);
    }
    
    public GameObject GetFirstPlayerOfLevel(int level)
    {
        if (firstPlayerInLevels.Count > level)
            return firstPlayerInLevels[level];
        return null;
    }
    
    IEnumerator Transition()
    {
        yield return new WaitForSeconds(0.1f);
        //onTransition = true;
        fx.SetActive(false);

        // spawn presets until goal of current level is spawned
        while (goalPositions.Count <= gameManager.Level || presetsOnMap.Count < 3)
        {
            SpawnPreset();
        }
        
        OnSuccessfulPass?.Invoke();

        Transform currGoal = GetGoalPositionOfLevel(gameManager.Level);
        
        // move until goal is on the top or currentPlayer is on the bottom
        float distance = Mathf.Min(currGoal.position.y - goalOffsetFromMiddle, currPlayer.position.y - targetY);
        var transform1 = transform;
        while (distance > 0)
        {
            float deltaMove = speed * Time.deltaTime;
            deltaMove = Mathf.Min(deltaMove, distance);
            distance -= deltaMove;

            Vector3 pos = transform.position;
            pos.y -= deltaMove;
            transform1.position = pos;
            yield return null;
        }
        
        
        if (presetsOnMap[0].SpawnPos < targetY)
        {
            Destroy(presetsOnMap[0].gameObject);
            presetsOnMap.RemoveAt(0);
        }
        if(fx!=null)
            fx.SetActive(true);
        ballController.GoalTransitionOver = true; // set variable so BallController knows the transition was finished
    }

    public void AllyBarPassed()
    {
        allyBarsPassed++;
        if (allyBarsPassed == totalPlayersInLevel + 1)
        {
            allyBarsPassed = 1;
            totalPlayersInLevel = difficultyProgression.GetTotalPlayersInLevel(gameManager.Level);
        }
        else
            gameManager.SetLevelProgress((float)allyBarsPassed / (totalPlayersInLevel+1));
    }

    public void StartTransition(Transform newPlayer)
    {
        currPlayer = newPlayer;
        currPlayer.GetComponentInChildren<AllyBar>().SetPassed();
        StartCoroutine(Transition());
    }
}
