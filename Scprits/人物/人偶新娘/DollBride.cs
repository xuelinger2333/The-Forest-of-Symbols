using FishNet.Demo.AdditiveScenes;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;

//网络同步变量: isRoseAppear, set flower, active skill cd, passive skill, unique skill cd

public class DollBride: Character
{
    private bool isUsingPassiveSkill = false;
    [HideInInspector]
    public bool isRoseAppear = false;
    #region State
    public DollBrideActiveSkillState activeSkillState { get; private set; }
    public DollBrideUniqueSkillState uniqueSkillState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        characterName = "人偶新娘";   
        activeSkillCD = 8.0f; 
        activeSkillTimer = 0;
        uniqueSkillLimit = 100;
        uniqueSkillCount = 0;
        uniqueSkillAddCountCD = 1f;
        uniqueSkillAddCountTimer = 0f;
        uniqueSkillAddCountUnit = 6;

        faceDir = new Vector2(1, 0);
        idleState = new DollBrideIdleState(this.playerViewAdapter, enemy, stateMachine, "idle", this);
        moveState = new DollBrideMoveState(this.playerViewAdapter, enemy, stateMachine, "move", this);
        mainAttackState = new DollBrideMainAttackState(this.playerViewAdapter, enemy, stateMachine, "mainAttack", this);
        mainAttackBreakState = new MainAttackBreakState(this.playerViewAdapter, enemy, stateMachine, "tired", this);
        activeSkillState = new DollBrideActiveSkillState(this.playerViewAdapter, enemy, stateMachine, "activeSkill", this);
        uniqueSkillState = new DollBrideUniqueSkillState(this.playerViewAdapter, enemy, stateMachine, "uniqueSkill", this);
        chargeState = new ChargeState(this.playerViewAdapter, enemy, stateMachine, "charge", this);
        chargeAttackState = new ChargeAttackState(this.playerViewAdapter, enemy, stateMachine, "chargeAttack", this);
        dizzyState = new DizzyState(this.playerViewAdapter, enemy, stateMachine, "dizzy", this);
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        if (!CanUsePassiveSkill)
            return;
    }

    protected override void ActiveSkill_Execute()
    {
        CDController.setActiveSkillTimer(activeSkillCD);
        stateMachine.ChangeState(activeSkillState);
    }

    protected override void UniqueSkill_Execute()
    {        
        CDController.AddUniqueSkillCount(-uniqueSkillLimit);
        stateMachine.ChangeState(uniqueSkillState);
    }


    public override float2 getPassiveSkillInfo() 
    {
        if (isUsingPassiveSkill)
            return new float2(0, 1);
        else
            return new float2(1, 1);
    }
    protected override void Update()
    {
        base.Update();
        //如果不能使用被动技能，返回
        if (!CanUsePassiveSkill) 
            return;
        //在敌人具备（视野中且未暴露）状态时，才获得被动buff，在获得被动buff的情况下，不能继续获得，敌人退出状态时，取消被动buff。
        if (!isUsingPassiveSkill)
        {
            if (IsInScreenRange() && enemy.stats.exposureLevel_model == 0 && enemy.stats.exposureLevel_skill == 0)
            {
                action.ChangeAction(Actions.DBPassiveAction, 0);

                stats.SetPlayerMoveSpeed(stats.currentMoveSpeed + 2);
                stats.SetPlayerAccumulateSpeed(stats.currentAccumulateSpeed + 2);
                stats.SetPlayerAttackDelaySpeed(stats.currentAttackDelaySpeed + 3);
                isUsingPassiveSkill = true;
            }
        }
        else
        {
            if (!(IsInScreenRange() && enemy.stats.exposureLevel_model == 0 && enemy.stats.exposureLevel_skill == 0))
            {
                action.ChangeAction(Actions.StopDBPassiveAction, 0);

                stats.SetPlayerMoveSpeed(stats.currentMoveSpeed - 2);
                stats.SetPlayerAccumulateSpeed(stats.currentAccumulateSpeed - 2);
                stats.SetPlayerAttackDelaySpeed(stats.currentAttackDelaySpeed - 3);
                isUsingPassiveSkill = false;
            }
        }

    }

    //private void SetRose()
    //{
    //    GameObject rose = TriggerManager.ExecuteTrigger("人偶新娘被动", playerViewAdapter.gameObject);
    //    rose.GetComponent<RoseControl>().player = this;
    //    rose = TriggerManager.ExecuteTrigger("人偶新娘被动", playerViewAdapter_map2.gameObject);
    //    rose.GetComponent<RoseControl>().player = this;
    //}

}
