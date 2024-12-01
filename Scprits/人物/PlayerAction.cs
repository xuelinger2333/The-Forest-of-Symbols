using FishNet.Demo.AdditiveScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ActionObserver
{
    void OnStartAction(Actions state, float param);
    void OnStartMoveState(Vector2 targetPos, float speed);
}
//处理所有特效显示的组件
//作用范围：pos_update，预制体特效，动画，viewAdapter效果
public enum Actions
{
    WaterAction = 0,
    StopWaterAction,

    CameraCoverAction,

    ChargeAction,
    StopChargeAction,

    DBPassiveAction,
    StopDBPassiveAction,
    DBUniqueAction,
    StopDBUniqueAction,
    DBActiveAction,
    StopDBActiveAction,

    SHPassiveAction,
    StopSHPassiveAction,

    PDPassiveAction,
    PDUniqueAction,

    DWSingAction,
    DWActiveAction,
    DWCoffinAction,
    StopDWCoffinAction
}


public class PlayerAction : MonoBehaviour
{
    Character player; //容器类
    MoveAction moveAction;
    WaterAction waterAction;
    CameraCoverAction ccAction;
    [HideInInspector]
    public ChargeAction chargeAction;
    [HideInInspector]
    public DollBridePassiveAction DBPassiveAction;
    DollBrideUniqueAction DBUniqueAction;
    DollBrideActiveAction DBActiveAction;
    ShadowHackerPassiveAction SHPassiveAction;
    PlagueDoctorUniqueAction PDUniqueAction;
    PlagueDoctorPassiveAction PDPassiveAction;
    DreamWeaverSingAction DWSingAction;
    DreamWeaverActiveAction DWActiveAction;
    DreamWeaverCoffinAction DWCoffinAction;

    

    delegate void PlayerActionDelegate(float param);
    List<PlayerActionDelegate> actionDictionary;
    List<ActionObserver> actionObservers = new List<ActionObserver>();
    private void Start()
    {
        TryGetComponent(out moveAction);
        TryGetComponent(out chargeAction);
        TryGetComponent(out DBPassiveAction);
        TryGetComponent(out DBUniqueAction);
        TryGetComponent(out DBActiveAction);
        TryGetComponent(out SHPassiveAction);
        TryGetComponent(out PDUniqueAction);
        TryGetComponent(out PDPassiveAction);
        TryGetComponent(out DWSingAction);
        TryGetComponent(out DWActiveAction);
        TryGetComponent(out DWCoffinAction);

        TryGetComponent(out waterAction);
        TryGetComponent(out ccAction);

        TryGetComponent(out player);
        actionDictionary = new List<PlayerActionDelegate>
        {
            ChangeWaterAction,
            StopWaterAction,

            ChangeCameraCoverAction,

            ChangeChargeAction,
            StopChargeAction,

            ChangeDBPassiveAction,
            StopDBPassiveAction,
            ChangeDBUniqueAction_enemy,
            StopDBUniqueAction_enemy,
            ChangeDBActiveAction_enemy,
            StopDBActiveAction_enemy,

            ChangeSHPassiveAction,
            StopSHPassiveAction,

            ChangePDPassiveAction,
            ChangePDUniqueAction,

            ChangeDWSingAction,
            ChangeDWActiveAction,
            ChangeDWCoffinAction,
            StopDWCoffinAction,
        };
    }
    public void AddActionObserver(ActionObserver observer)
    {
        actionObservers.Add(observer);
    }
    public void ChangeAction(Actions action, float param)
    {
        actionDictionary[(int)action].Invoke(param);
        for (int i = 0; i < actionObservers.Count; i++)
        {
            actionObservers[i].OnStartAction(action, param);
        }
    }
    #region 移动状态
    public void ChangeMovingState(Vector2 targetPos, float speed)
    {
        moveAction.Enter(player, targetPos, speed);
        for (int i = 0; i < actionObservers.Count; i++)
        {
            actionObservers[i].OnStartMoveState(targetPos, speed);
        }
    }
    #endregion
    #region 蓄力状态
    void ChangeChargeAction(float chargeTimer)
    {
        if (chargeAction) chargeAction.Enter(chargeTimer, player.playerID, player.characterName, player.myCamera);
    }
    void StopChargeAction(float nothing)
    {
        if (chargeAction) chargeAction.Exit();
    }
    #endregion
    #region 被动技能状态

    void ChangeDBPassiveAction(float nothing)
    {
        if (DBPassiveAction) DBPassiveAction.Enter();
    }
    void StopDBPassiveAction(float nothing)
    {
        if (DBPassiveAction) DBPassiveAction.Exit();
    }
    void ChangeSHPassiveAction(float nothing)
    {
        if (SHPassiveAction) SHPassiveAction.Enter(player);
    }
    void StopSHPassiveAction(float nothing)
    {
        if (SHPassiveAction) SHPassiveAction.Exit();
    }
    void ChangePDPassiveAction(float nothing)
    {
        if (PDPassiveAction) PDPassiveAction.Enter(new Vector2(player.pos_x, player.pos_y));
    }
    #endregion
    #region 主动技能状态
    void ChangeDBActiveAction_enemy(float time)
    {
        if (DBActiveAction) DBActiveAction.Enter(time, player.playerID);
    }
    void StopDBActiveAction_enemy(float nothing)
    {
        if (DBActiveAction) DBActiveAction.Exit();
    }
    void ChangeDWActiveAction(float time)
    {
        if (DWActiveAction) DWActiveAction.Enter(time, player.playerID);
    }
    #endregion
    #region 绝招状态
    void ChangeDBUniqueAction_enemy(float time)
    {
        if (DBUniqueAction) DBUniqueAction.Enter(time, player.playerID, player.myCamera);
    }
    void StopDBUniqueAction_enemy(float nothing)
    {
        if (DBUniqueAction) DBUniqueAction.Exit();
    }
    void ChangePDUniqueAction(float nothing)
    {
        if (PDUniqueAction) PDUniqueAction.Enter();
    }
    #endregion
    #region 其他状态
    void ChangeWaterAction(float nothing)
    {
        if (waterAction)
            waterAction.Enter();
    }
    void StopWaterAction(float nothing)
    {
        if (waterAction)
            waterAction.Exit();
    }
    void ChangeCameraCoverAction(float level)
    {
        if (ccAction)
            ccAction.Enter((int)level);
    }
    void ChangeDWSingAction(float time)
    {
        if (DWSingAction)
            DWSingAction.Enter(time);
    }
    void ChangeDWCoffinAction(float nothing)
    {
        if (DWCoffinAction)
            DWCoffinAction.Enter(new Vector2(player.pos_x, player.pos_y), (player.faceDir.x == 1));
    }
    void StopDWCoffinAction(float nothing)
    {
        if (DWCoffinAction)
            DWCoffinAction.Exit();
    }
    #endregion
}
