using FishNet.Demo.AdditiveScenes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DollBrideActiveAction : MonoBehaviour
{
    [SerializeField] Timer Timer_ActiveTimer;
    [SerializeField] PlayerViewManager viewManager;
    GameObject heart = null, heart2 = null;
    private void Awake()
    {
        enabled = false;
    }
    public void Enter(float time, bool playerID)
    {
        IconManager.instance.PlayIcon(time, IconType.人偶主动, playerID);
        SoundManager.PlaySound(SoundType.DBActiveSkill, 0.9f);
        Timer_ActiveTimer.targetTime = time;
        List<GameObject> g = viewManager.GetGameObject();
        if (heart == null)
            heart = TriggerManager.ExecuteTrigger("角色_神魂颠倒", g[0]);
        if (heart2 == null)
            heart2 = TriggerManager.ExecuteTrigger("角色_神魂颠倒", g[1]);
        Timer_ActiveTimer.StartTimer(TimeOut);
        
    }
    void TimeOut()
    {
        if (heart != null)
            heart.GetComponent<TriggerInfo>().EndAndDestroy();
        if (heart2 != null)
            heart2.GetComponent<TriggerInfo>().EndAndDestroy();
        heart = null;
        heart2 = null;
    }

    public void Exit(){}
}
