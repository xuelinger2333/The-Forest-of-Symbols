using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlagueDoctorMainAttackState : MainAttackState
{
    public PlagueDoctorMainAttackState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, PlagueDoctor _player) : base(_playerBase, _enemy, _stateMachine, _aniBoolName, _player)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        if(attackTriggerCalled)
        {
            SoundManager.PlaySound(SoundType.PDMainAttack, 0.8f);
        }
    }

    public override void Update()
    {
        base.Update();
    }
}
