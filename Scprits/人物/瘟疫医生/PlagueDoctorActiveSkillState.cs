using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class PlagueDoctorActiveSkillState : PlayerState
{
    protected PlagueDoctor player;
    private bool isChangeState = false;
    public PlagueDoctorActiveSkillState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, PlagueDoctor _player) : base(_player, _stateMachine, _aniBoolName)
    {
        player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        isChangeState = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(triggerCalled && !isChangeState)
        {
            //第一次执行
            SoundManager.PlaySound(SoundType.PDActiveSkill, 0.5f);
            player.AcitiveSkill();
            triggerCalled = false;
            isChangeState = true;
        }
        else if(triggerCalled && isChangeState)
        {
            //第二次执行
            stateMachine.ChangeState(player.idleState);
        }
    }
}
