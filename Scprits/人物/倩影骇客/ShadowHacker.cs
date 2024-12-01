using FishNet.Demo.AdditiveScenes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class ShadowHacker : Character
{
    public UavHandler uavHandler;
    #region State
    public ShadowHackerActiveSkillState activeSkillState { get; private set; }
    public ShadowHackerUniqueSkillState uniqueSkillState { get; private set; }
    public ShadowHackerASReadyState ASReadyState;
    public ShadowHackerASReturnReadyState ASReturnReadyState;
    public ShadowHackerASReturnState ASReturnState;
    #endregion

    protected override void Awake()
    {
        base.Awake();
        activeSkillCD = 12f;
        activeSkillTimer = 0;
        uniqueSkillLimit = 100;
        uniqueSkillCount = 0;
        uniqueSkillAddCountCD = 1f;
        uniqueSkillAddCountTimer = 0f;
        uniqueSkillAddCountUnit = 3;
        characterName = "倩影骇客";
        faceDir = new Vector2(1, 0);
        idleState = new ShadowHackerIdleState(this.playerViewAdapter, enemy, stateMachine, "idle", this);
        moveState = new ShadowHackerMoveState(this.playerViewAdapter, enemy, stateMachine, "move", this);
        mainAttackState = new ShadowHackerMainAttackState(this.playerViewAdapter, enemy, stateMachine, "mainAttack", this);
        mainAttackBreakState = new MainAttackBreakState(this.playerViewAdapter, enemy, stateMachine, "tired", this);

        activeSkillState = new ShadowHackerActiveSkillState(this.playerViewAdapter, enemy, stateMachine, "activeSkill", this);
        ASReadyState = new ShadowHackerASReadyState(this.playerViewAdapter, enemy, stateMachine, "activeSkillReady", this);
        ASReturnReadyState = new ShadowHackerASReturnReadyState(this.playerViewAdapter, enemy, stateMachine, "activeSkillReturnReady", this);
        ASReturnState = new ShadowHackerASReturnState(this.playerViewAdapter, enemy, stateMachine, "activeSkillReturn", this);

        uniqueSkillState = new ShadowHackerUniqueSkillState(this.playerViewAdapter, enemy, stateMachine, "uniqueSkill", this);
        chargeState = new ChargeState(this.playerViewAdapter, enemy, stateMachine, "charge", this);
        chargeAttackState = new ChargeAttackState(this.playerViewAdapter, enemy, stateMachine, "chargeAttack", this);
        dizzyState = new DizzyState(this.playerViewAdapter, enemy, stateMachine, "dizzy", this);
    }
    protected override void Start()
    {
        base.Start();
        uavHandler = UavHandler.Instance;
        stateMachine.Initialize(idleState);
    }
    public override void OnMove(Vector2 input)
    {
        if (stateMachine.currentState == ASReadyState)
        {
            inputDirection = input;
            faceDir = inputDirection;

            stateMachine.ChangeState(activeSkillState);
            CDController.setActiveSkillTimer(activeSkillCD);
        }
        base.OnMove(input);

    }
    public override void OnActiveSkill()
    {
        if (!HACKER_EmPulse_Effect()) return;
        if (activeSkillTimer <= 0 && stateMachine.currentState == idleState)
        {
            stateMachine.ChangeState(ASReadyState);
            for (int i = 0; i < characterObservers.Count; i++)
            {
                characterObservers[i].OnNotify(this, Operation.ActiveSkill);
            }
        }
        else
        {
            if (activeSkillTimer > 0 && stateMachine.currentState == idleState)
            {
                stateMachine.ChangeState(ASReturnReadyState);
            }
        }
    }
    public override void OnActiveSkillCancel()
    {
        base.OnActiveSkillCancel();
        if (stateMachine.currentState == ASReadyState)
        {
            stateMachine.ChangeState(idleState);
        }
        else
        {
            if (stateMachine.currentState == ASReturnReadyState)
            {
                stateMachine.ChangeState(idleState);
            }
        }
    }
    protected override void UniqueSkill_Execute()
    {
        CDController.AddUniqueSkillCount(-uniqueSkillLimit);
        CDController.setActiveSkillTimer(0);
        stateMachine.ChangeState(uniqueSkillState);
    }
    public override float2 getPassiveSkillInfo() { return new float2(0, 1); }
}
