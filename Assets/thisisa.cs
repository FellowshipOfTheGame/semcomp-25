using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thisisa : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioManager audioManager;

    void Start()
    {
        FindObjectOfType<AudioManager>().Play("test");
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            FindObjectOfType<AudioManager>().Play("test2");
        if (Input.GetMouseButtonDown(1))
            audioManager.Play("test4");
    }
}
