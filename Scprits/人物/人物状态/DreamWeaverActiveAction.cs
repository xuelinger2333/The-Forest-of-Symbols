using FishNet.Demo.AdditiveScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamWeaverActiveAction : MonoBehaviour
{
    [SerializeField] Timer Timer_ActiveTimer;
    [SerializeField] PlayerViewManager viewManager;
    [SerializeField] Material DWActiveMaterial;

    private void Awake()
    {
        enabled = false;
    }
    public void Enter(in float time, in bool playerID)//传的参数不会进行修改
    {
        IconManager.instance.PlayIcon(time, IconType.魔女敌方, playerID);
        viewManager.ChangeMaterial(DWActiveMaterial);
        Timer_ActiveTimer.targetTime = time;
        Timer_ActiveTimer.StartTimer(TimeOut);
    }
    void TimeOut()
    {
        viewManager.ResetMaterial();
    }

    public void Exit() { }
}
