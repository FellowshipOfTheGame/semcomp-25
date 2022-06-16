using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    Camera cam;
    [SerializeField] float range = 2.2f;
    [SerializeField] float speed = 0.5f;
    [SerializeField] float targetX = 0;
    [SerializeField] float startX = 0;
    [SerializeField] float startMouseX = 0;
    [SerializeField] bool selected = false;
    public bool Selected => selected;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

    }

    public void SetSelected(bool val)
    {
        selected = val; 
    }

    public void SetTarget(float currentMouseX)
    {
        float dirX = currentMouseX-startMouseX;
        targetX = startX + dirX;
        targetX = Mathf.Min(targetX, range);
        targetX = Mathf.Max(targetX, -range);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            RaycastHit2D hit = Physics2D.CircleCast(mousePos, 0.1f, new Vector3(1f, 0, 0));
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                PlayerController2 controller=hit.transform.parent.gameObject.GetComponent<PlayerController2>();
                controller.SetSelected(true);
                startMouseX = mousePos.x;
                startX = hit.transform.parent.position.x;
            }
        }


        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            RaycastHit2D hit = Physics2D.CircleCast(mousePos, 0.1f, new Vector3(1f, 0, 0));
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                PlayerController2 controller = hit.transform.parent.gameObject.GetComponent<PlayerController2>();
                if (controller.Selected)
                {
                    float _x = mousePos.x;
                    controller.SetTarget(_x);
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            RaycastHit2D hit = Physics2D.CircleCast(mousePos, 0.1f, new Vector3(1f, 0, 0));
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                PlayerController2 controller = hit.transform.parent.gameObject.GetComponent<PlayerController2>();
                controller.SetSelected(false);
            }
        }
        if (selected)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.MoveTowards(pos.x, targetX, speed * Time.deltaTime);
            transform.position = pos;
        }
    }
}
