using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DreamweaverChargeAttackState : ChargeAttackState
{
    protected Dreamweaver player;
    private string aniBoolName;
    private Vector2 aimDirection;
    public DreamweaverChargeAttackState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, Dreamweaver _player) : base(_playerBase, _enemy, _stateMachine, _aniBoolName, _player)
    {
        player = _player;
        aniBoolName = _aniBoolName;
    }

    public override void Enter()
    {
        player.SetAnimationBool(aniBoolName, true);
        triggerCalled = false;
        attackTriggerCalled = false;
        int mapRotationCount = 0;
        if (player.playerID) mapRotationCount = player.map.rotationCounts1;
        else mapRotationCount = player.map.rotationCounts2;
        aimDirection = UtilFunction.ComputeDirectionRotateforPlayer(player.inputDirection, mapRotationCount);
        player.faceDir = player.inputDirection;
        player.CreatChargeAttackEffect(aimDirection);
    }

    public override void Exit()
    {
        player.SetAnimationBool(aniBoolName, false);
    }

    public override void Update()
    {
        if ((attackTriggerCalled))
        {
            //SoundManager.PlaySound(SoundType.魔女蓄力攻击, 0.6f);
            if (player.enemy.pos_update == player.pos_update || player.enemy.pos_update == player.pos_update + aimDirection)
            {
                player.enemy.Die("Attack");
            }
            attackTriggerCalled = false ;
        }
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
