using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    List<Preset> presetsOnMap = new List<Preset>();

    [SerializeField] Transform presetSpawner;
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private GameObject fx;
    [SerializeField] Transform ballTransf;
    
    Transform currPlayer;
    
    /* Screen and presets related offsets */
    private const float SpawnOffset = 2f;
    private const float SpawnOffsetAfterGoal = 3.7f;
    private const float OffsetFromScreenBottom = 1.6f;
    private const float OffsetFromScreenTop = 1.6f;
    private float targetY;
    private float goalTargetY;
    
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
    private Field[] fields = new Field[2];

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        difficultyProgression = FindObjectOfType<DifficultyProgression>();
        fields = GetComponentsInChildren<Field>();
        ballController = FindObjectOfType<BallController>();
    }

    public void SetBallFx(bool val)
    {
        fx.SetActive(val);
    }
    
    private void Start()
    {
        targetY = Camera.main!.ScreenToWorldPoint(new Vector3(Screen.width / 2f, 0)).y + OffsetFromScreenBottom;
        goalTargetY = Camera.main!.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height)).y - OffsetFromScreenTop;
        
        // spawn until a goal is spawned
        SpawnPresetsUntilGoal();
        
        if (fields[0] != null)
        {
            RepositionFields();
        }

        ballTransf.position = presetsOnMap[0].FirstPlayer.GetComponentInChildren<Ally>().transform.position;
        totalPlayersInLevel = difficultyProgression.GetTotalPlayersInLevel(gameManager.Level);
    }

    public void SpawnPresetsUntilGoal()
    {
        // spawn presets until goal of current level is spawned
        while (goalPositions.Count <= gameManager.Level)
        {
            SpawnPreset();
        }
    }

    private void SpawnPreset()
    {
        int presetCount = presetsOnMap.Count;
        Vector2 posNextSpawn = new Vector2(0, targetY);
        if (presetCount > 0)
        {
            // if it is a preset after a goal then spawn further than normal
            if (presetsOnMap[presetCount - 1].HasGoal())
                posNextSpawn.y = presetsOnMap[presetCount-1].SpawnPos + SpawnOffsetAfterGoal;
            else
                posNextSpawn.y = presetsOnMap[presetCount-1].SpawnPos + SpawnOffset;
        }

        GameObject obj = Instantiate(difficultyProgression.NextPreset(), posNextSpawn, Quaternion.identity, presetSpawner);

        
        
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
    public void RepositionFields()
    {
        float endGoalY = GetGoalPositionOfLevel(gameManager.Level).position.y;
        float startGoalY = GetFirstPlayerOfLevel(gameManager.Level).transform.position.y;

        if (gameManager.Level == 0)
        {
            fields[0].SetGoalPosition(startGoalY - .9f, endGoalY);
            fields[1].SetGoalPosition(startGoalY - 15f, startGoalY - 5f);
        }
        else
        {
            // reposiciona o campo que está embaixo
            int index = (fields[0].transform.position.y < fields[1].transform.position.y) ? 0 : 1;
            fields[index].SetGoalPosition(startGoalY - .9f, endGoalY);
        }
        
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
        fx.SetActive(false);

        // spawn presets until goal of current level is spawned
        while (presetsOnMap.Count <= 7)
        {
            SpawnPreset();
        }

        Transform currGoal = GetGoalPositionOfLevel(gameManager.Level);
        
        // move until goal is on the top or currentPlayer is on the bottom
        float distance = Mathf.Min(currGoal.position.y - goalTargetY, currPlayer.position.y - targetY);
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
