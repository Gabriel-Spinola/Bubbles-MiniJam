using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager I;

    private void Awake()
    {
        if (I == null) {
            I = this;
        }
        else {
            Destroy(gameObject);

            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    private void Start()
    {
        Play("Theme");
    }

    public void Play(string name)
    {
        Sound sound_ = Array.Find(sounds, sound => sound.name == name);

        if (sounds == null) {
            Debug.LogWarning($"Sound: { name } not found!");

            return;
        }

        sound_.source.Play();
    }
}
