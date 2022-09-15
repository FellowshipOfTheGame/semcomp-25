using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    Camera cam;

    private bool isMoving;
    private bool canMove = true;
    private PlayerController lastSelected;

    public void SetCanMove(bool val)
    {
        canMove = val;
        if (!val && isMoving)
        {
            lastSelected.SetSelected(false);
            isMoving = false;
            lastSelected = null;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    private Collider2D[] results = new Collider2D[5];
    private void InputClickDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            
            // Get all colliders under click
            var size = Physics2D.OverlapPointNonAlloc(mousePos, results);
            for (int i = 0; i < size; i++)
            {
                // If collided with the bar
                if (results[i].CompareTag("Player"))
                {
                    // Select player to control
                    PlayerController controller = results[i].transform.parent.gameObject.GetComponent<PlayerController>();
                    lastSelected = controller;
                    controller.SetSelected(true);
                    isMoving = true;
                    controller.SetStartMouseX(mousePos.x);
                    controller.SetStartX(results[i].transform.parent.position.x);

                    break;
                }
            }
        }
    }
    private void InputClickHold()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            
            // Get all colliders under click
            var size = Physics2D.OverlapPointNonAlloc(mousePos, results);
            for (int i = 0; i < size; i++)
            {
                // If collided with the bar
                if (results[i].CompareTag("Player"))
                {
                    PlayerController controller = results[i].transform.parent.gameObject.GetComponent<PlayerController>();
                    if (controller.Selected)
                    {
                        float _x = mousePos.x;
                        controller.SetTarget(_x);
                    }

                    break;
                }
            }
        }
    }

    private void InputClickUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (isMoving)
            {
                lastSelected.SetSelected(false);
                isMoving = false;
                lastSelected = null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.isGamePaused && canMove)
        {
            InputClickDown();
            InputClickHold();
            InputClickUp();
        }
    }
}
