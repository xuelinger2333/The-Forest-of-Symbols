using FishNet.Object.Synchronizing;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class VirableNetworkSynchronizer : NetworkBehaviour
{
    GameManager gameManager;
    Character player0, player1;

    public readonly SyncVar<Vector2> faceDir_0 = new SyncVar<Vector2>(new SyncTypeSettings(0.2f));
    public readonly SyncVar<int> exposureLevel_model_0 = new SyncVar<int>(new SyncTypeSettings(0.2f));
    public readonly SyncVar<int> exposureLevel_skill_0 = new SyncVar<int>(new SyncTypeSettings(0.2f));

    public readonly SyncVar<Vector2> faceDir_1 = new SyncVar<Vector2>(new SyncTypeSettings(0.2f));
    public readonly SyncVar<int> exposureLevel_model_1 = new SyncVar<int>(new SyncTypeSettings(0.2f));
    public readonly SyncVar<int> exposureLevel_skill_1 = new SyncVar<int>(new SyncTypeSettings(0.2f));
    public override void OnStartClient()
    {
        base.OnStartClient();
        gameManager = GameManager.Instance;
        player0 = gameManager.player0 as Character;
        player1 = gameManager.player1 as Character;
        //update只会运行在具有服务器的客户端的自身的脚本上
        if (IsOwner && IsServerInitialized)
        {
        }
        else
        {
            gameObject.GetComponent<VirableNetworkSynchronizer>().enabled = false;
        }

        faceDir_0.OnChange += on_faceDir_0_change;
        faceDir_1.OnChange += on_faceDir_1_change;
        exposureLevel_model_0.OnChange += on_exposure_model_0_change;
        exposureLevel_model_1.OnChange += on_exposure_model_1_change;
        exposureLevel_skill_0.OnChange += on_exposure_skill_0_change;
        exposureLevel_skill_1.OnChange += on_exposure_skill_1_change;
    }
    private void FixedUpdate()
    {
        if (player0 != null && player0.NETWORK_isMainController)
        {
            //同步player0变量
            if (player0.faceDir != faceDir_0.Value)
            {
                faceDir_0.Value = player0.faceDir;
            }
            if (player0.stats.exposureLevel_model != exposureLevel_model_0.Value)
            {
                exposureLevel_model_0.Value = player0.stats.exposureLevel_model;
            }
            if (player0.stats.exposureLevel_skill != exposureLevel_skill_0.Value)
            {
                exposureLevel_skill_0.Value = player0.stats.exposureLevel_skill;
            }
        }
        if (player1 != null && player1.NETWORK_isMainController)
        {
            //同步player1变量
            if (player1.faceDir != faceDir_1.Value)
            {
                faceDir_1.Value = player1.faceDir;
            }
            if (player1.stats.exposureLevel_model != exposureLevel_model_1.Value)
            {
                exposureLevel_model_1.Value = player1.stats.exposureLevel_model;
            }
            if (player1.stats.exposureLevel_skill != exposureLevel_skill_1.Value)
            {
                exposureLevel_skill_1.Value = player1.stats.exposureLevel_skill;
            }
        }
    }
    private void on_faceDir_0_change(Vector2 prev, Vector2 next, bool asServer)
    {
        if (IsOwner) return;
        player0.faceDir = next;
    }
    private void on_faceDir_1_change(Vector2 prev, Vector2 next, bool asServer)
    {
        if (IsOwner) return;
        player1.faceDir = next;
    }
    private void on_exposure_model_0_change(int prev, int next, bool asServer)
    {
        if (IsOwner) return;
        player0.stats.SetExposureLevel_model(next);
    }
    private void on_exposure_model_1_change(int prev, int next, bool asServer)
    {
        if (IsOwner) return;
        player1.stats.SetExposureLevel_model(next);
    }
    private void on_exposure_skill_0_change(int prev, int next, bool asServer)
    {
        if (IsOwner) return;
        player0.stats.SetExposureLevel_skill_network(next);
    }
    private void on_exposure_skill_1_change(int prev, int next, bool asServer)
    {
        if (IsOwner) return;
        player1.stats.SetExposureLevel_skill_network(next);
    }
}
