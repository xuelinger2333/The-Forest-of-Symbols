using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlagueDoctorIdleState : IdleState
{
    public PlagueDoctorIdleState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, PlagueDoctor _player) : base(_playerBase, _enemy, _stateMachine, _aniBoolName, _player)
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
