using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DizzyState : PlayerState
{
    protected Character player;
    private float timer;
    public DizzyState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, Character _player) : base(_player, _stateMachine, _aniBoolName)
    {
        player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        timer = player.dizzyTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
