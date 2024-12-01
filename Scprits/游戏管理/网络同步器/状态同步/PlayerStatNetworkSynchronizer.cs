using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Object.Synchronizing;

public class PlayerStatNetworkSynchronizer : NetworkBehaviour
{
    GameManager gameManager;
    Character player0, player1;
    [SerializeField]
    StatNetworkSynchronizer synchronizer_1, synchronizer_0;

    public override void OnStartClient()
    {
        base.OnStartClient();
        gameManager = GameManager.Instance;
        player0 = gameManager.player0 as Character;
        player1 = gameManager.player1 as Character;
        synchronizer_0.playerAction = player0.action;
        synchronizer_1.playerAction = player1.action;
        //update只会运行在具有服务器的客户端的自身的脚本上
        if (IsOwner && IsServerInitialized)
        {
            player0.action.AddActionObserver(synchronizer_0);
            player1.action.AddActionObserver(synchronizer_1);
        }
        else
        {
            gameObject.GetComponent<PlayerStatNetworkSynchronizer>().enabled = false;
        }
    }

}
