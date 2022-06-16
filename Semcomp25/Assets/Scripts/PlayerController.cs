using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Camera cam;
    [SerializeField] float range=2.2f;
    [SerializeField] float speed = 0.5f;
    [SerializeField] float targetX = 0;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
       
    }
    IEnumerator GoToTarget(Transform player, Vector3 target)
    {
        target.x = Mathf.Max(-range, target.x);
        target.x = Mathf.Min(range, target.x);
        
        while (Mathf.Abs(player.position.x-target.x) > 0.5f)
        {
            Vector3 pos=player.position;
            pos.x = Mathf.MoveTowards(pos.x, target.x, speed * Time.deltaTime);
            player.position = pos;
            yield return null;
        }
    }
    public void SetTarget(float target)
    {
        targetX = target;

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            RaycastHit2D hit =Physics2D.CircleCast(mousePos, 0.1f,new Vector3(1f,0,0));
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {

                float _x=mousePos.x;
                _x = Mathf.Max(-range, _x);
                _x = Mathf.Min(range, _x);
                hit.transform.parent.gameObject.GetComponent<PlayerController>().SetTarget(_x);
            }
        }

        Vector3 pos = transform.position;
        pos.x = Mathf.MoveTowards(pos.x, targetX, speed * Time.deltaTime);
        transform.position = pos;
    }
}
