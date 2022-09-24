using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    private void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("main-volume", 0f));
        SetMusicVolume(PlayerPrefs.GetFloat("music-volume", 0f));
        SetEffectsVolume(PlayerPrefs.GetFloat("effects-volume", 0f));
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("main-volume", volume);
        PlayerPrefs.SetFloat("main-volume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("music-volume", volume);
        PlayerPrefs.SetFloat("music-volume", volume);
    }

    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("effects-volume", volume);
        PlayerPrefs.SetFloat("effects-volume", volume);
    }
}
