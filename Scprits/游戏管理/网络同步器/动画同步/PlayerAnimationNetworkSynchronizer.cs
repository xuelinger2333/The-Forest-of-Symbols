using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Example.Scened;
using UnityEngine.InputSystem;
using FishNet.Object.Synchronizing;
using FishNet.Connection;
using System;
using Unity.VisualScripting;

public class PlayerAnimationNetworkSynchronizer : NetworkBehaviour
{
    GameManager gameManager;
    Character player0, player1;
    [SerializeField]
    AnimationNetworkSynchronizer synchronizer_1, synchronizer_0;
    public override void OnStartClient()
    {
        base.OnStartClient();
        gameManager = GameManager.Instance;        
        player0 = gameManager.player0 as Character;
        player1 = gameManager.player1 as Character;
        synchronizer_0.p = player0;
        synchronizer_1.p = player1;
        //update只会运行在具有服务器的客户端的自身的脚本上
        if (IsOwner && IsServerInitialized)
        {

            player0.AddAnimationObserver(synchronizer_0);
            player1.AddAnimationObserver(synchronizer_1);
        }
        else
        {
            gameObject.GetComponent<PlayerAnimationNetworkSynchronizer>().enabled = false;
        }
    }

}

