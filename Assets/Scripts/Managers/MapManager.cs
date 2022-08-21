using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] List<Preset> presetsOnMap;

    [SerializeField] private bool onTransition=false;
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
    private List<Vector2> goalPositions = new List<Vector2>();
    private List<GameObject> firstPlayerInLevels = new List<GameObject>();

    private GameManager gameManager;
    private DifficultyProgression difficultyProgression;
    
    /* Field related */
    private Field field;

    /* Pass event */
    public delegate void SuccessfulPass();
    public static event SuccessfulPass OnSuccessfulPass;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        difficultyProgression = FindObjectOfType<DifficultyProgression>();
        field = GetComponentInChildren<Field>();
    }

    public void SetBallFx(bool val)
    {
        fx.SetActive(val);
    }
    
    private void Start()
    {
        if (field != null)
        {
            field.ResetPosition();
            field.SetGoalPosition(300f);
        }
        SpawnPreset();
        SpawnPreset();
        SpawnPreset();

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
            Vector2 goalPos = preset.GetGoalPosition();
            goalPositions.Add(goalPos);
            //if (field != null)
            //    field.SetGoalPosition(goalPos.y);
        }
    }

    public Vector2 GetGoalPositionOfLevel(int level)
    {
        if (goalPositions.Count > level)
            return goalPositions[level];
        return Vector2.zero;
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
        onTransition = true;
        fx.SetActive(false);

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
        OnSuccessfulPass?.Invoke();
        if (presetsOnMap[0].SpawnPos < targetY)
        {
            SpawnPreset();
            Destroy(presetsOnMap[0].gameObject);
            presetsOnMap.RemoveAt(0);
        }
        fx.SetActive(true);
    }

    public void AllyBarPassed()
    {
        allyBarsPassed++;
        if (allyBarsPassed == totalPlayersInLevel + 1)
        {
            allyBarsPassed = 1;
            //gameManager.PassLevel();
            totalPlayersInLevel = difficultyProgression.GetTotalPlayersInLevel(gameManager.Level);
            gameManager.SetLevelProgress(1f);
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
