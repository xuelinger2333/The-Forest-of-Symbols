using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamweaverActiveSkillState : PlayerState
{
    protected Dreamweaver player;
    private bool isPlaySound = false;
    private bool isChangeState = false;

    public DreamweaverActiveSkillState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, Dreamweaver _player) : base(_player, _stateMachine, _aniBoolName)
    {
        player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        isPlaySound = false;
        isChangeState = false ;

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            if(!isPlaySound)
            {
                //第一次
                SoundManager.PlaySound(SoundType.魔女主动技能, 0.5f);
                triggerCalled = false;
                isPlaySound=true;
            }
            else if(!isChangeState)
            {
                //第二次
                player.stats.ChangeWovenDreamsState(2.5f);
                player.action.ChangeAction(Actions.DWActiveAction, 2.5f);
                triggerCalled = false;
                isChangeState = true;
            }
            
            else
            {
                //第三次
                stateMachine.ChangeState(player.idleState);
            }
        }
    }
}
