using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollBrideActiveSkillState : PlayerState
{
    protected DollBride player;
    private bool isSkillUsed;

    public DollBrideActiveSkillState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, DollBride _player) : base(_player, _stateMachine, _aniBoolName)
    {
        player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        isSkillUsed = false;
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(triggerCalled && !isSkillUsed)
        {
            UseActiveSkill();
            triggerCalled = false;
            isSkillUsed = true;
        }
        else if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
        
    }

    private void UseActiveSkill()
    {
        int time;
        if (player.IsInScreenRange()) time = 6;
        else time = 3;
        player.enemy.stats.ChangeEntrancedState(time);
        player.enemy.action.ChangeAction(Actions.DBActiveAction, time);
        return;

    }

}
