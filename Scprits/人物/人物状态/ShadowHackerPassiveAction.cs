using FishNet.Demo.AdditiveScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class ShadowHackerPassiveAction : MonoBehaviour
{
    bool playerID = false;
    GameObject Emp = null, Emp_map2 = null;
    private void Awake()
    {
        enabled = false;
    }
    public void Enter(in Player player)//仅会修改isRoseAppear
    {
        playerID = player.playerID;
        IconManager.instance.PlayIcon(1001, IconType.骇客被动, player.playerID);
        if (Emp == null)
            Emp = TriggerManager.ExecuteTrigger("角色_电磁脉冲", player.playerViewAdapter.gameObject);
        if (Emp_map2 == null)
            Emp_map2 = TriggerManager.ExecuteTrigger("角色_电磁脉冲", player.playerViewAdapter_map2.gameObject);
    }
    public void Exit()
    {
        IconManager.instance.StopIcon(playerID, IconType.骇客被动);
        if (Emp != null)
            Emp.GetComponent<TriggerInfo>().EndAndDestroy();
        if (Emp_map2 != null)
            Emp_map2.GetComponent<TriggerInfo>().EndAndDestroy();
        Emp = null;
        Emp_map2 = null;
    }
}
