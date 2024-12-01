using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//主动技能准备状态
public class ShadowHackerASReadyState : PlayerState
{
    protected ShadowHacker player;

    public ShadowHackerASReadyState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, ShadowHacker _player) : base(_player, _stateMachine, _aniBoolName)
    {
        player = _player;
    }
}
//准备收回状态
public class ShadowHackerASReturnReadyState : PlayerState
{
    protected ShadowHacker player;
    private float chargeTimer;

    public ShadowHackerASReturnReadyState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, ShadowHacker _player) : base(_player, _stateMachine, _aniBoolName)
    {
        player = _player;
    }
    public override void Enter()
    {
        base.Enter();
        
        chargeTimer = 0.5f;
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        chargeTimer -= Time.deltaTime;
        if (chargeTimer< 0)
        {
            player.stateMachine.ChangeState(player.ASReturnState);

        }        

    }
}
//收回状态
public class ShadowHackerASReturnState : PlayerState
{
    protected ShadowHacker player;
    private bool isSkillUsed;

    public ShadowHackerASReturnState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, ShadowHacker _player) : base(_player, _stateMachine, _aniBoolName)
    {
        player = _player;
    }
    public override void Enter()
    {
        base.Enter();
        int num = player.uavHandler.MasterRecallAllUAVs(Convert.ToInt32(player.playerID));
        if (num == 0) player.stateMachine.ChangeState(player.idleState);
        else
        {
            player.CDController.setActiveSkillTimer(player.getActiveSkillInfo().x - num);
        }
    }
    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
            
    }
}
//主动技能状态
public class ShadowHackerActiveSkillState : PlayerState
{
    protected ShadowHacker player;
    private bool isSkillUsed;

    public ShadowHackerActiveSkillState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, ShadowHacker _player) : base(_player, _stateMachine, _aniBoolName)
    {
        player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        isSkillUsed = false;
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
            UseActiveSkill();
            stateMachine.ChangeState(player.idleState);
        }

    }

    private void UseActiveSkill()
    {
        //Debug.Log("创建了一个无人机");
        int id = Convert.ToInt32(player.playerID);
        int rotate = player.playerID == true ? player.map.rotationCounts1 : player.map.rotationCounts2; 
        Vector2 flyDir = UtilFunction.ComputeDirectionRotateforPlayer(player.faceDir, rotate);
        player.uavHandler.addUavbyPosition(id, new Vector2(player.pos_x, player.pos_y),flyDir,false, 0);
        SoundManager.PlaySound(SoundType.无人机出现, 1f);
        return;
    }

}
