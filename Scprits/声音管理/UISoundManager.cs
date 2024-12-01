using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UISoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    [SerializeField] private AudioClip[] musicList;
    private AudioSource soundSource;
    private AudioSource musicSource;
    private static UISoundManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        soundSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        PlayMusic(0,0.6f);
    }

    public static void PlaySound(UISoundType sound, float volume)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.soundSource.PlayOneShot(randomClip, volume);
    }

    public static void StopPlayMusic()
    {
        if (instance != null && instance.musicSource != null)
            instance.musicSource.Stop();
    }
    public static void PlayMusic(int index,float volume)
    {
        AudioClip clip = instance.musicList[(int)index];
        instance.musicSource.clip = clip;
        instance.musicSource.loop = true;
        instance.musicSource.volume = volume;
        instance.musicSource.Play();
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(UISoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < names.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
#endif
}

public enum UISoundType
{
    木头声,
    退出木头声,
    纸张,
    渐进木头声,
    杏仁,
    强劲木头声,
    鼓声,
    木头加植物生长,
    翻页,
    木头加剑,
}
