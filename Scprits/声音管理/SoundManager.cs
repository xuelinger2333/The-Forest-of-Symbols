using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public enum SoundType
{
    Move,
    DBMainAttack,
    DBActiveSkill,
    DBUniqueSkill,
    DBChargeAttack,
    PDMainAttack,
    PDChargeAttack,
    PDActiveSkill,
    PDPassiveSkill,
    PDUniqueSkill,
    无人机出现,
    无人机扫描,
    开伞,
    魔女蓄力攻击,
    暗影覆面,
    魔女复生,
    魔女消失,
    魔女主动技能,
    棺椁倒塌,
    魔女绝招,
}
//[RequireComponent(typeof(AudioSource)),ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    [SerializeField] private AudioClip[] musicList;
    private AudioSource soundSource;
    private AudioSource musicSource;
    public static SoundManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        soundSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound,float volume)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0,clips.Length)];
        instance.soundSource.PlayOneShot(randomClip, volume);
    }

    public static void PlayMusic(int index,float volume)
    {
        AudioClip clip = instance.musicList[(int)index];
        instance.musicSource.volume = volume;
        instance.musicSource.clip = clip;
        instance.musicSource.loop = true;
        instance.musicSource.Play();
    }

    public static void ChangeMusic(int index,float fadeDuration)
    {
        instance.StartCoroutine(instance.FadeOutIn(instance.musicList[(int)index], fadeDuration));
    }

    private IEnumerator FadeOutIn(AudioClip newClip, float fadeDuration)
    {
        // Fade out
        float startVolume = musicSource.volume;
        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.volume = 0;
        musicSource.Stop();

        // Change the clip
        musicSource.clip = newClip;
        musicSource.Play();

        // Fade in
        while (musicSource.volume < startVolume)
        {
            musicSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.volume = startVolume;
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for (int i = 0; i < names.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
#endif
    public void DestroyPersistentObject()
    {
        instance = null;
        Destroy(gameObject);
    }
}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}
