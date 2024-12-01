using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamweaverRebornState : PlayerState
{
    private Dreamweaver player;
    private bool isDisappear;
    public DreamweaverRebornState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, Dreamweaver _player) : base(_player, _stateMachine, _aniBoolName)
    {
        player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        isDisappear = false;
    }

    public override void Exit()
    {
        base.Exit();
        player.SetAnimationBool("isdisappear", false);
    }

    public override void Update()
    {
        base.Update();
        if(triggerCalled)
        {
            if(!isDisappear)
            {
                //第一次调用
                player.Reborn();
                player.SetAnimationBool("isdisappear", true);
                isDisappear = true;
                triggerCalled = false;
            }
            else
            {
                //第二次调用
                stateMachine.ChangeState(player.idleState);
            }
        }
    }
}
