using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamweaverUniqueSkillState : PlayerState
{
    protected Dreamweaver player;
    private bool isPlaySound = false;
    public DreamweaverUniqueSkillState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, Dreamweaver _player) : base(_player, _stateMachine, _aniBoolName)
    {
        player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        isPlaySound = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            if(!isPlaySound)
            {
                //第一次
                SoundManager.PlaySound(SoundType.魔女绝招, 0.5f);
                triggerCalled = false;
                isPlaySound = true;
            }
            else
            {
                //第二次
                player.CreateCoffin();
                stateMachine.ChangeState(player.idleState);
            }
            
        }
    }
}
