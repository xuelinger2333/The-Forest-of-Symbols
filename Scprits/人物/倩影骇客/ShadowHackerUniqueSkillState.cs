using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShadowHackerUniqueSkillState : PlayerState {
    protected ShadowHacker player;
    List<Vector2> dir = new List<Vector2> { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };
    public ShadowHackerUniqueSkillState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, ShadowHacker player) : base(player, _stateMachine, _aniBoolName)
    {
        this.player = player;
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
        if (triggerCalled)
        {
            UseUniqueSkill();
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void UseUniqueSkill()
    {
        
        for (int i = 0; i < 4; i++)
        {
            int id = Convert.ToInt32(player.playerID);
            player.uavHandler.addUavbyPosition(id, new Vector2(player.pos_x, player.pos_y), dir[i], true, i + 1);
            SoundManager.PlaySound(SoundType.无人机出现, 1f);
        }
        return;
    }
}
