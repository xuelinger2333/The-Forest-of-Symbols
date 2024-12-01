using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem;
using System;
using FishNet.Object;
//server端的client请最后连接
public class NetWorkGameManager: NetworkBehaviour {
    GameManager gameManager;
    public Character player0;
    public Character player1;
    public override void OnStartClient()
    {
        base.OnStartClient();
        gameManager = GameManager.Instance;
        if (!gameManager.FishNet_isUsingNetwork)
        {
            Destroy(gameObject);
        }
        if (!base.IsOwner)
        {
            gameObject.GetComponent<NetWorkGameManager>().enabled = false;
        }
        else
        {
            
            if (!base.IsServerInitialized)
            {
                gameManager.player0.enabled = false;
                gameManager.player1.enabled = false;
                gameManager.player0.NETWORK_isMainController = false;
                gameManager.player1.NETWORK_isMainController = false;
                gameManager.creatureGenerator.SetActive(false);
            }
            else
            {
                gameManager.player0.NETWORK_isMainController = true;
                gameManager.player1.NETWORK_isMainController = true;
                gameManager.creatureGenerator.SetActive(true);
                player0 = gameManager.player0 as Character;
                player1 = gameManager.player1 as Character;
            }
            gameManager.UnPauseGame();
        }
    }
}

