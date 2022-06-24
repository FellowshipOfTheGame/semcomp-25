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

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform alliesTransf;
    Transform currPlayer;
    IEnumerator Transition()
    {
        yield return new WaitForSeconds(0.2f);
        onTransition = true;
        fx.SetActive(false);
        while (Mathf.Abs(currPlayer.position.y-targetY)>0.1f)
        {
            Vector3 pos = transform.position;
            pos.y -= speed*Time.deltaTime;
            transform.position = pos;
            yield return null;
        }
        onTransition = false;
        int destroyed = 0;
        for (int i = allies.Count - 1; i >= 0; i--)
        {
            Transform item = allies[i];
            if (item.position.y < targetY)
            {
                Destroy(item.gameObject);
                allies.RemoveAt(i);
                destroyed++;
            }
        }
        for (int i = 0,j=16; i < destroyed; i++,j-=4)
        {
            GameObject obj = Instantiate(playerPrefab, alliesTransf);
            Vector3 pos = obj.transform.position;
            pos.y = j;
            obj.transform.position = pos;
            allies.Add(obj.transform);
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
