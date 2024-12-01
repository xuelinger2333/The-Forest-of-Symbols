using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
//网络同步变量：faceDir，moveState
public class ChargeAttackState : PlayerState
{
    private Character player;
    private Vector3 targetPos;
    private float distance;
    private float speed;
    Vector2 aimDirection;
    bool canMove;
    public ChargeAttackState(PlayerViewAdapter _playerBase, Player _enemy, PlayerStateMachine _stateMachine, string _aniBoolName, Character _player) : base(_player, _stateMachine, _aniBoolName)
    {
        player = _player;
    }
    public override void Enter()
    {
        base.Enter();
        canMove = false;
        int mapRotationCount = (player.playerID) ? player.map.rotationCounts1 : player.map.rotationCounts2;

        aimDirection = UtilFunction.ComputeDirectionRotateforPlayer(player.inputDirection, mapRotationCount);

        targetPos = player.pos_update + aimDirection;

        player.faceDir = player.inputDirection;
        //检测目标地块是否可以移动
        if (targetPos.x > player.map.width - 1
            || targetPos.y > player.map.height - 1
            || targetPos.x < 0
            || targetPos.y < 0 //判断是否超出地图边界
            || player.map.mapData[(int)(targetPos.x), (int)(targetPos.y)].isObstacle//目标地块是否合理 
            || (player.enemy && player.enemy.pos_update == player.pos_update)) //敌人即将被杀死
        {

        }
        else
        {
            canMove = true;        
            distance = 1;
            speed = distance / player.stats.moveTime;

            //最后执行移动的更新
            player.action.ChangeMovingState(targetPos, speed);

        }
    }

    public override void Exit()
    {
        base.Exit();
        if (canMove)
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

        if(player.enemy && player.enemy.pos_update == player.pos_update) player.enemy.Die("Attack");
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            if (!canMove)
                player.stateMachine.ChangeState(player.mainAttackBreakState);
            else
            if (player.pos_update.x == targetPos.x && player.pos_update.y == targetPos.y)
            {
                player.stateMachine.ChangeState(player.mainAttackBreakState);
            }
        }
    }
}
