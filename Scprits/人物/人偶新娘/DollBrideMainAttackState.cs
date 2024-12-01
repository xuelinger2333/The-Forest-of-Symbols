using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollBrideMainAttackState : MainAttackState
{
    public DollBrideMainAttackState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, DollBride _player) : base(_playerBase, _enemy, _stateMachine, _aniBoolName, _player)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        SoundManager.PlaySound(SoundType.DBMainAttack, 1f);
    }

    public override void Update()
    {
        base.Update();
    }
}
