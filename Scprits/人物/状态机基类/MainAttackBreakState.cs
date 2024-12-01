using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAttackBreakState : PlayerState
{
    protected Character player;
    public MainAttackBreakState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, Character _player) : base(_player, _stateMachine, _aniBoolName)
    {
        player = _player;
    }
    public override void Enter()
    {
        base.Enter();
        //float speed = (player.stats.currentAttackDelaySpeed - 4) * 0.15f + 1;
        //player.SetAnimationSpeed(speed);
        player.SetAnimationSpeed(PlayerStats.BASIC_ATTACK_DELAY_TIME / player.stats.attackDelayTime);
    }

    public override void Exit()
    {
        base.Exit();
        player.SetAnimationSpeed(1);
    }

    public override void Update()
    {
        base.Update();
        if(triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
