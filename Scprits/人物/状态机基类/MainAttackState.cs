using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAttackState : PlayerState
{
    protected Character player;
    public MainAttackState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, Character _player) : base(_player, _stateMachine, _aniBoolName)
    {
        player = _player;
    }
    public override void Enter()
    {
        base.Enter();
        //player.SetAnimationSpeed(1 + (player.stats.currentAttackSpeed - PlayerStats.BASIC_ATTACK_SPEED ) * 0.15f);
        player.SetAnimationSpeed(PlayerStats.BASIC_ATTACK_TIME / player.stats.attackTime);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetAnimationSpeed(1);
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled) stateMachine.ChangeState(player.mainAttackBreakState);
    }
}
