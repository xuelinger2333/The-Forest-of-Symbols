using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Snake : Creature
{
    int targetId;
    Player target;
    public Vector3 targetPos;
    public bool isSetted = false;
    private float distance = 1;
    private float moveTime = ConstantsValue.getMoveTimeFromSpeed(8);
    private float speed;
    int width, height;
    int total_step = 0;
    List<Vector2> dir = new List<Vector2> { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };
    public Snake()
    {
        handler = SnakeHandler.Instance;
    }
    public void Initialize(Vector2 initialPos, int TargetId, int mapwidth, int mapheight, int master = -1)
    {
        base.Initialize(initialPos, master);
        targetId = TargetId;
        width = mapwidth;
        height = mapheight;
        target = handler.PlayerDictionary[targetId];
        speed = distance / moveTime;
        SetAnimationBool("Move", true);
        SetAnimationBool("Trigger", false);
    }
    public override void Die()
    {
        base.Die();
        if (isTriggered)
        {
            SetAnimationBool("Move", false);
            SetAnimationBool("Trigger", true);
        }
        else
        {
            view1.DestroyMe();
            view2.DestroyMe();
        }
    }
    public void SetTargetForSnake()
    {
        //如果非客户端连接，进行确定下一步目标
        if (GameManager.Instance.FishNet_isUsingNetwork &&
        !handler.PlayerDictionary[0].NETWORK_isMainController)
            return;
        //如果追到玩家了，不执行任何操作
        if (pos_x == target.pos_x && pos_y == target.pos_y)
            return;
        List<Vector2> targets = new List<Vector2>();
        float pre_ManhattanDistance = Mathf.Abs(target.pos_x - pos_x) + Mathf.Abs(target.pos_y - pos_y);
        for (int i = 0; i < dir.Count; i++)
        {
            Vector2 compute_target = new Vector2(dir[i].x + pos_x, dir[i].y + pos_y);
            float cur_ManhattanDistance = Mathf.Abs(target.pos_x - compute_target.x) + Mathf.Abs(target.pos_y - compute_target.y);
            if (pre_ManhattanDistance <= cur_ManhattanDistance) continue;
            else if (compute_target.x >= 0 && compute_target.x < width && compute_target.y >= 0 && compute_target.y < height)
            {
                targets.Add(compute_target);
            }
        }
        targetPos = targets[Random.Range(0, targets.Count)];
        (handler as SnakeHandler).ControlSnakeMove(creatureId, pos_update, targetPos, new Vector2(targetPos.x - pos_x, targetPos.y - pos_y));
    }
    //追击玩家
    public override void Update()
    {
        if (isTriggered) return;
        base.Update();
        if (isSetted)
        {
            //如果到达目标，就将isSetted置为false
            if (pos_update.x == targetPos.x && pos_update.y == targetPos.y)
            {
                pos_x = (int)pos_update.x;
                pos_y = (int)pos_update.y;
                isSetted = false;
                SetTargetForSnake();
                total_step += 1;
            }
            else
            {
                float step = speed * Time.deltaTime;
                pos_update = Vector3.MoveTowards(pos_update, targetPos, step);
            }
        }
        //对于网络客户端连接，不允许其执行触发和销毁操作
        if (GameManager.Instance.FishNet_isUsingNetwork &&
        !handler.PlayerDictionary[0].NETWORK_isMainController)
        {
            return;
        }
        //如果超过最大步数限制
        if (total_step >= 15)
        {
            isTriggered = true;
            SetAnimationBool("Move", false);
            SetAnimationBool("Trigger", true);
            isSetted = true;
            return;
        }
        //如果追到玩家   
        if (!isTriggered && pos_x == target.pos_x && pos_y == target.pos_y)
        {
            Debug.Log("触发毒蛇！");
            SetAnimationBool("Move", false);
            SetAnimationBool("Trigger", true);
            TriggerEffect(targetId);
            isTriggered = true;
            return;
        }

    }
}
