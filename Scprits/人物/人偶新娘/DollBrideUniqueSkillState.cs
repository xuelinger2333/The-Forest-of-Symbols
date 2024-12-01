using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//网络同步变量:set flower, entrance state
public class DollBrideUniqueSkillState : PlayerState {
    protected DollBride player;
    private bool isSkillUsed;
    public DollBrideUniqueSkillState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, DollBride player) : base(player, _stateMachine, _aniBoolName)
    {
        this.player = player;
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
        if (triggerCalled && !isSkillUsed)
        {
            UseUniqueSkill();     
            triggerCalled = false;
            isSkillUsed = true;
        }
        else if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void UseUniqueSkill()
    {
        int time;
        if (player.IsInScreenRange()) time = 4;
        else time = 3;
        player.enemy.stats.ChangeWitchedState(time); //使敌方玩家进入“魅惑”状态
        //注意：更改持续时间也需要更改action里面的动画时间
        player.enemy.action.ChangeAction(Actions.DBUniqueAction, time);
        return;
    }
}
