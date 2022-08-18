using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("main-volume", volume * 100 - 100);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("music-volume", (volume * 100 - 100) );
    }

    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("effects-volume", (volume * 100 - 100) );
    }
}
