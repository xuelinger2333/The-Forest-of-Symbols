using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureNetworkSychronizer : NetworkBehaviour, UavHandlerObserver, CrowHandlerObserver, FireFlyHandlerObserver, DeerHandlerObserver, SnakeHandlerObserver
{
    UavHandler uavHandler;
    CrowHandler crowHandler;
    FireFlyHandler fireFlyHandler;
    DeerHandler deerHandler;
    SnakeHandler snakeHandler;
    public override void OnStartClient()
    {
        base.OnStartClient();
        uavHandler = UavHandler.Instance;
        crowHandler = CrowHandler.Instance;
        fireFlyHandler = FireFlyHandler.Instance;
        deerHandler = DeerHandler.Instance;
        snakeHandler = SnakeHandler.Instance;
        
        //update只会运行在具有服务器的客户端的自身的脚本上
        if (IsOwner && IsServerInitialized)
        {
            uavHandler.AddUavHandlerObserver(this);
            crowHandler.AddCrowHandlerObserver(this);
            fireFlyHandler.AddFireFlyHandlerObserver(this);
            deerHandler.AddDeerHandlerObserver(this);
            snakeHandler.AddSnakeHandlerObserver(this);
        }
        else
        {
            gameObject.GetComponent<CreatureNetworkSychronizer>().enabled = false;
        }
    }
    #region 无人机同步
    public void addUavbyPosition(int playerId, Vector2 pos, Vector2 direction, bool isUniqueSkillUav, int uavCategory_offset)
    {
        Client_addUavbyPosition(playerId, pos, direction, isUniqueSkillUav, uavCategory_offset);
    }
    [ObserversRpc]
    void Client_addUavbyPosition(int playerId, Vector2 pos, Vector2 direction, bool isUniqueSkillUav, int uavCategory_offset)
    {
        if (IsServerInitialized) return;
        uavHandler.addUavbyPosition(playerId, pos, direction, isUniqueSkillUav, uavCategory_offset);
    }
    public void changeUavStopTime(int stopTime, int creatureId)
    {
        Client_changeUavStopTime(stopTime, creatureId);
    }
    [ObserversRpc]
    void Client_changeUavStopTime(int stopTime, int creatureId)
    {
        if (IsServerInitialized) return;
        uavHandler.changeUavStopTime(stopTime, creatureId);
    }
    public void handleUavRecall(int creatureId)
    {
        Client_handleUavRecall(creatureId);
    }
    [ObserversRpc]
    void Client_handleUavRecall(int creatureId)
    {
        if (IsServerInitialized) return;
        uavHandler.handleUavRecall(creatureId);
    }
    #endregion
    #region 乌鸦同步
    public void addCrowbyPosition(Vector2 pos) 
    {
        Client_addCrowbyPosition(pos);
    }
    [ObserversRpc]
    void Client_addCrowbyPosition(Vector2 pos)
    {
        if (IsServerInitialized) return;
        crowHandler.addCrowbyPosition(pos);
    }
    public void TriggerCrow(int creatureId, int targetPlayer)
    {
        Client_TriggerCrow(creatureId, targetPlayer);
    }
    [ObserversRpc]
    void Client_TriggerCrow(int creatureId, int targetPlayer)
    {
        if (IsServerInitialized) return;
        crowHandler.TriggerCrow(creatureId, targetPlayer);
    }
    #endregion
    #region 萤火虫同步
    public void addFireFlybyPosition(Vector2 pos)
    {
        Client_addFireFlybyPosition(pos);
    }
    [ObserversRpc]
    void Client_addFireFlybyPosition(Vector2 pos)
    {
        if (IsServerInitialized) return;
        fireFlyHandler.addFireFlybyPosition(pos);
    }
    public void TriggerFireFly(int creatureId, int playerId)
    {
        Client_TriggerFireFly(creatureId, playerId);
    }
    [ObserversRpc]
    void Client_TriggerFireFly(int creatureId, int targetPlayer)
    {
        if (IsServerInitialized) return;
        fireFlyHandler.TriggerFireFly(creatureId, targetPlayer);
    }
    #endregion
    #region 九色鹿同步
    public void addDeerbyPosition(Vector2 pos)
    {
        Client_addDeerbyPosition(pos);
    }
    [ObserversRpc]
    void Client_addDeerbyPosition(Vector2 pos)
    {
        if (IsServerInitialized) return;
        deerHandler.addDeerbyPosition(pos);
    }
    public void ControlDeerMove(int creatureId,Vector2 preTarget, Vector2 targetPos, Vector2 faceDir)
    {
        Client_ControlDeerMove(creatureId,preTarget, targetPos, faceDir);
    }
    [ObserversRpc]
    void Client_ControlDeerMove(int creatureId,Vector2 preTarget, Vector2 targetPos, Vector2 faceDir)
    {
        if (IsServerInitialized) return;
        deerHandler.ControlDeerMove(creatureId,preTarget, targetPos, faceDir);
    }

    public void TriggerDeer(int creatureId, int playerId)
    {
        Client_TriggerDeer(creatureId, playerId);
    }
    [ObserversRpc]
    void Client_TriggerDeer(int creatureId, int playerId)
    {
        if (IsServerInitialized) return;
        deerHandler.TriggerDeer(creatureId, playerId);
    }
    #endregion
    public void addSnakebyPosition(Vector2 pos, int targetId)
    {
        Client_addSnakebyPosition(pos, targetId);
    }
    [ObserversRpc]
    void Client_addSnakebyPosition(Vector2 pos, int targetId)
    {
        if (IsServerInitialized) return;
        snakeHandler.addSnakebyPosition(pos, targetId);
    }
    public void ControlSnakeMove(int creatureId, Vector2 preTarget, Vector2 targetPos, Vector2 faceDir)
    {
        Client_ControlSnakeMove(creatureId, preTarget, targetPos, faceDir);
    }
    [ObserversRpc]
    void Client_ControlSnakeMove(int creatureId, Vector2 preTarget, Vector2 targetPos, Vector2 faceDir)
    {
        if (IsServerInitialized) return;
        snakeHandler.ControlSnakeMove(creatureId, preTarget, targetPos, faceDir);
    }
    public void TriggerSnake(int creatureId)
    {
        Client_TriggerSnake(creatureId);
    }
    [ObserversRpc]
    void Client_TriggerSnake(int creatureId)
    {
        if (IsServerInitialized) return;
        snakeHandler.TriggerSnake(creatureId);
    }
}
