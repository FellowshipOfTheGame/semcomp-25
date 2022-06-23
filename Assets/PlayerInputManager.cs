using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    Camera cam;

    [SerializeField] private bool isMoving;
    [SerializeField] private GameObject fx;
    public bool IsMoving => isMoving;
    private bool canMove=true;
    [SerializeField] private PlayerController lastSelected;
   
    public void SetCanMove(bool val)
    {
        canMove = val;
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

    }

    private void ínputClickDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            RaycastHit2D hit = Physics2D.CircleCast(mousePos, 0.1f, new Vector3(1f, 0, 0));
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                // Select player to control
                PlayerController controller = hit.transform.parent.gameObject.GetComponent<PlayerController>();
                lastSelected = controller;
                controller.SetSelected(true);
                isMoving = true;
                fx.SetActive(false);
                controller.SetStartMouseX(mousePos.x);
                controller.SetStartX(hit.transform.parent.position.x);
            }
        }
    }
    private void ínputClickHold()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            RaycastHit2D hit = Physics2D.CircleCast(mousePos, 0.1f, new Vector3(1f, 0, 0));
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                PlayerController controller = hit.transform.parent.gameObject.GetComponent<PlayerController>();
                if (controller.Selected)
                {
                    float _x = mousePos.x;
                    controller.SetTarget(_x);
                }
            }
        }
    }

    private void inputClickUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            RaycastHit2D hit = Physics2D.CircleCast(mousePos, 0.1f, new Vector3(1f, 0, 0));
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                //Unselect player
                PlayerController controller = hit.transform.parent.gameObject.GetComponent<PlayerController>();
                controller.SetSelected(false);
                isMoving = false;
                fx.SetActive(true);
            }
            else if(lastSelected!=null)
            {
                lastSelected.SetSelected(false);
                isMoving = false;
                fx.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            ínputClickDown();
            ínputClickHold();
            inputClickUp();
        }

    }
}
