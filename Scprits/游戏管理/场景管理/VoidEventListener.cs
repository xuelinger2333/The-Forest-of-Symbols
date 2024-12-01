using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 负责监听退出游戏事件
/// </summary>
public class VoidEventListener : MonoBehaviour
{
    public VoidGameEvent voidGameEvent;
    public UnityEvent OnEventRaised;

    private void OnEnable()
    {
        if (voidGameEvent == null)
        {
            return;
        }
        voidGameEvent.eventRaised += Respond;
    }

    private void OnDisable()
    {
        if (voidGameEvent == null)
        {
            return;
        }
        voidGameEvent.eventRaised -= Respond;
    }

    private void Respond()
    {
        if (gameObject == null)
        {
            return;
        }
        OnEventRaised.Invoke();
    }

    public void Exit()
    {
        Debug.Log("退出");
    }
}
