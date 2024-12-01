using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//网络同步变量：chargeState
public class ChargeState : PlayerState
{
    private float chargeTimer;
    protected Character player;
    float max_camera_drag_out = 1.5f;
    float cur_camera_add;
    //GameObject effect;
    public ChargeState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, Character _player) : base(_player, _stateMachine, _aniBoolName)
    {
        player = _player;
    }

    public override void Enter()
    {
        base.Enter();

        chargeTimer = player.stats.accumulateTime;
        player.isReadyForAttack = false;
        cur_camera_add = 0;
        player.action.ChangeAction(Actions.ChargeAction, chargeTimer);
    }

    public override void Exit()
    {
        base.Exit();
        player.isReadyForAttack = false;
        player.action.ChangeAction(Actions.StopChargeAction, 0);
    }

    public override void Update()
    {
        base.Update();
        chargeTimer -= Time.deltaTime;
        if (chargeTimer < 0 && !player.isReadyForAttack)
        {
            player.isReadyForAttack = true;
                
        }
    }
}
