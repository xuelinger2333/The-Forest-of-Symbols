using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float targetTime;
    public float TimeScale;
    public bool Timeout;
    public float currentTime;
    public bool isCountingTime { get; private set; } //正在计时的标记
    Action TimeoutCallback; //计时结束后的回调
    public bool UnScaleTime = false;

    private void Awake()
    {
        StopTimer();
    }
    public void StopTimer(bool InvokeCallback = true)
    {
        enabled = false;
        TimeScale = 1;
        Timeout = true; //注：Timeout在初始时需要设置为真，因为初始时还没有开始计时
        currentTime = 0;
        isCountingTime = false;
        if (InvokeCallback && TimeoutCallback != null)
            TimeoutCallback?.Invoke();
    }
    public void StartTimer(Action callback = null)
    {
        currentTime = 0;
        TimeScale = 1;
        TimeoutCallback = callback;
        isCountingTime = true;
        Timeout = false;
        enabled = true;
    }
    public void PauseForTime(float pauseTime)
    {
        if (isCountingTime == false) return;
        targetTime += pauseTime;
    }
    public void PauseTimer()
    {
        if (isCountingTime == false) return;
        enabled = false;
    }
    public void UnpauseTimer()
    {
        if (isCountingTime == false) return;
        enabled = true;
    }

    void Update()
    {
        if (!UnScaleTime)
            currentTime += Time.deltaTime * TimeScale;
        else
            currentTime += Time.unscaledDeltaTime * TimeScale;
        if (currentTime >= targetTime)
        {
            StopTimer();
        }
    }
}

