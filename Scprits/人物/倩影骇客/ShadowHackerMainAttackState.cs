using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowHackerMainAttackState : MainAttackState
{
    public ShadowHackerMainAttackState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, ShadowHacker _player) : base(_playerBase, _enemy, _stateMachine, _aniBoolName, _player)
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
        if (attackTriggerCalled) SoundManager.PlaySound(SoundType.DBMainAttack, 0.5f);
    }
}
