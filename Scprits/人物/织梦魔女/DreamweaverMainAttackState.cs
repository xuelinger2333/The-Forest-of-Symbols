using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamweaverMainAttackState : MainAttackState
{
    private float timer;
    public DreamweaverMainAttackState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, Dreamweaver _player) : base(_playerBase, _enemy, _stateMachine, _aniBoolName, _player)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (triggerCalled)
        {
            SoundManager.PlaySound(SoundType.¿ªÉ¡, 0.6f);
            triggerCalled = false;
        }
        if(attackTriggerCalled) stateMachine.ChangeState(player.mainAttackBreakState);
    }
}
