using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class StatNetworkSynchronizer : NetworkBehaviour, ActionObserver
{
    public PlayerAction playerAction;
    public void OnStartAction(Actions state, float param)
    {
        Client_NotifyAction(state, param);
    }
    [ObserversRpc]
    void Client_NotifyAction(Actions state, float param)
    {
        if (IsServerInitialized) return;
        playerAction.ChangeAction(state, param);
    }
    public void OnStartMoveState(Vector2 target, float speed)
    {
        Client_NotifyMoveState(target, speed);
    }
    [ObserversRpc]
    void Client_NotifyMoveState(Vector2 target, float speed)
    {
        if (IsServerInitialized) return;
        playerAction.ChangeMovingState(target, speed);
    }
}
