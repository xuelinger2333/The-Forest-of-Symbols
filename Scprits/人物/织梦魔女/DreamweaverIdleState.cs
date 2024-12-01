using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamweaverIdleState : IdleState
{
    public DreamweaverIdleState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, Dreamweaver _player) : base(_playerBase, _enemy, _stateMachine, _aniBoolName, _player)
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
        base.Update();
    }
}
