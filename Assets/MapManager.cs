using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    [SerializeField] List<Transform> allies;
    [SerializeField] List<Transform> enemies;
    [SerializeField] private bool onTransition=false;

    [SerializeField] private float targetY=-3f;
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private GameObject fx;

    [SerializeField] private List<GameObject> playerPrefabs;

    [SerializeField] private List<GameObject> enemyPrefabs;

    [SerializeField] private Transform alliesTransf;
    [SerializeField] private Transform enemiesTransf;
    Transform currPlayer;

    [SerializeField] private GameManager manager;
    IEnumerator Transition()
    {
        yield return new WaitForSeconds(0.1f);
        onTransition = true;
        fx.SetActive(false);
        while (Mathf.Abs(currPlayer.position.y-targetY)>0.2f)
        {
            Vector3 pos = transform.position;
            pos.y -= speed*Time.deltaTime;
            transform.position = pos;
            yield return null;
        }

        onTransition = false;
        int alliesDestroyed = 0, enemiesDestroyed=0;

        for (int i = allies.Count - 1; i >= 0; i--)
        {
            Transform item = allies[i];
            if (item.position.y < targetY)
            {
                Destroy(item.gameObject);
                allies.RemoveAt(i);
                alliesDestroyed++;
            }
        }

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            Transform item = enemies[i];
            if (item.position.y < targetY)
            {
                Destroy(item.gameObject);
                enemies.RemoveAt(i);
                enemiesDestroyed++;
            }
        }

        manager.AddPoint(alliesDestroyed);

        for (int i = 0, j = 14-(enemiesDestroyed-1)*4; i < enemiesDestroyed; i++, j += 4)
        {
            SpawnEnemy(j);
        }

        for (int i = 0,j=16-(enemiesDestroyed-1)*4; i < alliesDestroyed; i++,j+=4)
        {
            SpawnAlly(j);
        }

        fx.SetActive(true);
    }

    private void SpawnAlly(float _y)
    {
        int n = playerPrefabs.Count;
        if (allies[allies.Count - 1].CompareTag("Empty"))
            n--;
        GameObject obj = Instantiate(playerPrefabs[Random.Range(0,n)], alliesTransf);
        Vector3 pos = obj.transform.position;
        pos.y = _y;
        obj.transform.position = pos;
        allies.Add(obj.transform);
    }


    private void SpawnEnemy(float _y)
    {

        int n = enemyPrefabs.Count;
        GameObject obj = Instantiate(enemyPrefabs[Random.Range(0,n)], enemiesTransf);
        Vector3 pos = obj.transform.position;
        pos.y = _y;
        obj.transform.position = pos;
        enemies.Add(obj.transform);
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
