using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlagueDoctorUniqueSkillState : PlayerState
{
    protected PlagueDoctor player;
    private bool isChangeState = false;
    private bool isSet = false;
    public PlagueDoctorUniqueSkillState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, PlagueDoctor _player) : base(_player, _stateMachine, _aniBoolName)
    {
        player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        isChangeState = false;
        isSet = false;
        SoundManager.PlaySound(SoundType.PDUniqueSkill, 0.7f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled && !isSet)
        {
            //第一次执行
            player.action.ChangeAction(Actions.PDUniqueAction, 0);
            triggerCalled = false;
            isSet = true;
        }
        else if (triggerCalled && !isChangeState)
        {
            //第二次执行
            UseUniqueSkill();
            triggerCalled = false;
            isChangeState = true;
        }
        else if (triggerCalled)
        {
            //第三次执行
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void UseUniqueSkill()
    {
        player.applyPlague(2, false);
    }
}
