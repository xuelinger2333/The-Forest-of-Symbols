using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationNetworkSynchronizer : NetworkBehaviour, PlayerAnimationObserver
{
    public Player p;
    #region 动画同步
    public void OnNotifyBool(string aniName, bool val)
    {
        Client_NotifyAniBool(aniName, val);
    }
    [ObserversRpc]
    void Client_NotifyAniBool(string aniName, bool val)
    {
        if (IsServerInitialized) return;
        p.SetAnimationBool(aniName, val);
    }
    public void OnNotifySpeed(float val)
    {
        Client_NotifyAniSpeed(val);
    }
    [ObserversRpc]
    void Client_NotifyAniSpeed(float val)
    {
        if (IsServerInitialized) return;
        p.SetAnimationSpeed(val);
    }
    public void OnNotifyInt(string aniName, int val)
    {
        Client_NotifyAniInt(aniName, val);
    }
    [ObserversRpc]
    void Client_NotifyAniInt(string aniName, int val)
    {
        if (IsServerInitialized) return;
        p.SetAnimationInt(aniName, val);
    }
    #endregion
}