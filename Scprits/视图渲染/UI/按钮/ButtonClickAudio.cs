using FishNet.Managing.Scened;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickAudio : MonoBehaviour
{
    public UISoundType SoundType;
    public float SoundTime;

    public int MusicIndex;
    public float MusicTime;
    public void OnClickPlaySound()
    {
        UISoundManager.PlaySound(SoundType, SoundTime);
        
    }

    public void OnClickPlayMusic()
    {
        if (SoundManager.instance != null)
            SoundManager.instance.DestroyPersistentObject();
        UISoundManager.PlayMusic(MusicIndex, MusicTime);
    }

    public void LoadScene(string str)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(str);
    }
    public void QuitApp()
    {
        Application.Quit();
    }
}
