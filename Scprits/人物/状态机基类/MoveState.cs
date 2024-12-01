using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//网络同步变量：faceDir，moveState
public class MoveState : PlayerState
{
    protected Character player;
    private Vector2 targetPos;
    private bool canMove;
    private float distance;
    private float speed;

    float moveTime;
    float CurrentTime = 0;
    public MoveState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, Character _player) : base(_player, _stateMachine, _aniBoolName)
    {
        player = _player;
    }

    public override void Enter()
    {
        base.Enter();
        moveTime = player.stats.moveTime;
        player.moveCD = 0.2f + moveTime;
        canMove = false;
        int mapRotationCount = (player.playerID)? player.map.rotationCounts1 :player.map.rotationCounts2;

        Vector2 aimDirection = UtilFunction.ComputeDirectionRotateforPlayer(player.inputDirection, mapRotationCount);
        aimDirection.x += player.pos_x;
        aimDirection.y += player.pos_y;
        targetPos = new Vector2(aimDirection.x, aimDirection.y); 
        
        player.faceDir = player.inputDirection;

        //检测目标地块是否合理
        if (targetPos.x > player.map.width - 1
            || targetPos.y > player.map.height - 1
            || targetPos.x < 0
            || targetPos.y < 0) //判断是否超出地图边界
        {   
            stateMachine.ChangeState(player.idleState);
            return;
        }
        if (player.map.mapData[(int)(targetPos.x), (int)(targetPos.y)].isObstacle) //判断目标地块是否可以移动
        {   
            stateMachine.ChangeState(player.idleState);
            return;
        }
        //最后执行移动参数的设定
        canMove = true;
        distance = 1;
        speed = distance / player.stats.moveTime;
        player.action.ChangeMovingState(targetPos, speed);

        CurrentTime = 0;
    }

    public override void Exit()
    {
        base.Exit();

        if (canMove) //如果设置了目的地，代表玩家移动了
        {
            if (player.specialStatisticCounter)
                player.specialStatisticCounter.AddStepCount();
            player.map.OnPlayerExit(player.pos_x, player.pos_y);
            player.pos_x = (int)targetPos.x;
            player.pos_y = (int)targetPos.y;
            player.pos_update = new Vector2(player.pos_x, player.pos_y);
            player.map.UpdateMap(player.pos_x, player.pos_y, player);
            player.map.OnPlayerEnter(player.pos_x, player.pos_y);
        }
    }

    public override void Update()
    {
        base.Update();
        //以移动时间作为停留在moveState的参考基准
        CurrentTime += Time.deltaTime;
        if (CurrentTime >= moveTime)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}