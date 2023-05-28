using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    [Tooltip("The audio clips to be used in the game.")]
    [SerializeField] private AudioClip[] audioClips;

    private AudioSource musicAudioSource;
    private AudioSource[] vfxAudioSource = new AudioSource[2];
    private Dictionary<string, AudioClip> audioDictionary = new Dictionary<string, AudioClip>();
    private int nextVfxSource = 0;

    void Start()
    {
        StartDictionary();
        InstantiateAudioSources();
    }

    public void PlayVFX(string name)
    {
        if (audioDictionary.ContainsKey(name))
        {
            vfxAudioSource[nextVfxSource].PlayOneShot(audioDictionary[name]);
            nextVfxSource = (nextVfxSource + 1) % vfxAudioSource.Length;
        }
    }

    public void PlayMusic(string name)
    {
        if (audioDictionary.ContainsKey(name))
            musicAudioSource.PlayOneShot(audioDictionary[name]);
    }

    public void PlayMusicLoop(string name)
    {
        if (audioDictionary.ContainsKey(name))
        {
            musicAudioSource.clip = audioDictionary[name];
            musicAudioSource.Play();
        }
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }

    private void StartDictionary()
    {
        foreach (AudioClip audioClip in audioClips)
            audioDictionary.Add(audioClip.name, audioClip);
    }

    private void InstantiateAudioSources()
    {
        for (int i = 0; i < vfxAudioSource.Length; i++)
            vfxAudioSource[i] = gameObject.AddComponent<AudioSource>();

        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.loop = true;
    }
}