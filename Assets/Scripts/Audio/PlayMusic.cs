using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public string music;
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        PlayIt(music);
        PlayIt("test1");
        PlayIt("test2");
    }
    
    public void PlayIt(string musicName)
    {
        audioManager.PlayMusic(musicName);
    }
}
