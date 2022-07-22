using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    [SerializeField] List<GameObject> presetPrefabs;
    [SerializeField] List<Preset> presetsOnMap;
    [SerializeField] private bool onTransition=false;
    [SerializeField] Transform presetSpawner;
    [SerializeField] private float targetY=-3f;
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private GameObject fx;

    float posNextSpawn = -4f;
    Transform currPlayer;

    [SerializeField] private GameManager manager;
    public void SetBallFx(bool val)
    {
        fx.SetActive(val);
    }
    private void Start()
    {
        SpawnPreset();
        SpawnPreset();

    }
    private void SpawnPreset()
    {
        int presetCount = presetsOnMap.Count;
        if (presetCount > 0)
        {
            posNextSpawn = presetsOnMap[presetCount-1].SpawnPos+4f;
        }
        Vector2 pos = new Vector2(0, posNextSpawn);
        int id = Random.Range(0, presetPrefabs.Count);
        GameObject obj=Instantiate(presetPrefabs[id], pos, Quaternion.identity, presetSpawner);
        Preset preset = obj.GetComponent<Preset>();
        presetsOnMap.Add(preset);
  
    }
    IEnumerator Transition()
    {
        yield return new WaitForSeconds(0.1f);
        onTransition = true;
        fx.SetActive(false);
        int points=((int)Mathf.Abs(currPlayer.position.y - targetY)/4);
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

        fx.SetActive(true);
    }

    public void StartTransition(Transform newPlayer)
    {
        currPlayer = newPlayer;
        StartCoroutine(Transition());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
