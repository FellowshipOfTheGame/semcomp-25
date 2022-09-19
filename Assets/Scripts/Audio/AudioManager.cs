using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] SFX;
    public Sound[] Musics;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }

        foreach (Sound sound in Musics)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.outputAudioMixerGroup = sound.output;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }

        foreach ( Sound sound in SFX )
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.outputAudioMixerGroup = sound.output;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }    

    public void PlayMusic(string music)
    {
        this.Play(music, Musics);
    }

    public void PlaySFX(string sfx)
    {
        this.Play(sfx, SFX);
    }

    private void Play (string name, Sound[] soundList)
    {
        Sound s = Array.Find( soundList, sound => sound.name == name );

        if (s == null)
            return;

        s.source.Play();
    }
}