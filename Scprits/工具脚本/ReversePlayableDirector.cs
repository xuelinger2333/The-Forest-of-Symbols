using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

//将Unity内置timeline逆向播放，并在播放完毕时执行回调的类
public class ReversePlayableDirector : MonoBehaviour
{
    [SerializeField] PlayableDirector playable;
    public bool isReversed = true;
    Coroutine coroutine;
    Action callback = null;
    public void Play(Action callback = null)
    {
        if (isReversed)
        {
            coroutine = StartCoroutine(Reverse(playable));
            this.callback = callback;
        }
        else
        {
            playable.Play();
            playable.stopped += OnPlayableDirectorStopped;
        }
    }
    public void Stop()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        callback = null;
        playable.Stop();
    }
    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (callback != null)
        callback.Invoke();
    }
    private IEnumerator Reverse(PlayableDirector playable)
    {
        float dt = (float)playable.duration;

        while (dt > 0)
        {
            dt -= Time.deltaTime / (float)playable.duration;

            playable.time = Mathf.Max(dt, 0);
            playable.Evaluate();
            yield return null;
        }
        if (callback != null)
        {
            callback.Invoke();
        }
    }
}
