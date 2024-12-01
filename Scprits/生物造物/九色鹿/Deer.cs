using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Deer : Creature
{
    public Vector3 targetPos;
    public bool isSetted = false; 
    public bool changeAnimation = false;
    public Coroutine waitAndMoveCoroutine;

    private float distance = 1;
    private float moveTime = ConstantsValue.getMoveTimeFromSpeed(7);
    private float speed;
   
    int width, height;
    List<Vector2> dir = new List<Vector2> {new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0)};

    public Deer()
    {
        handler = DeerHandler.Instance;
    }
    public void Initialize(Vector2 initialPos, int mapwidth, int mapheight, int master = -1)
    {
        base.Initialize(initialPos, master);
        width = mapwidth;
        height = mapheight;
        distance = 1;
        speed = distance / moveTime;
        targetPos = new Vector3(pos_x, pos_y, 0);
        UpdateTargetPos();
    }
    void UpdateTargetPos()
    {
        //对于网络客户端连接，不允许其执行更新位置和自主选择移动目标
        if (GameManager.Instance.FishNet_isUsingNetwork &&
        !handler.PlayerDictionary[0].NETWORK_isMainController)
        {
            return;
        }
        List<Vector2> targets = new List<Vector2>();
        for (int i = 0; i < dir.Count; i++)
        {
            Vector2 compute_target = new Vector2(dir[i].x + pos_x, dir[i].y + pos_y);
            if (compute_target.x >= 0 && compute_target.x < width && compute_target.y >= 0 && compute_target.y < height
                && handler.QueryPosIsGround((int)compute_target.x, (int)compute_target.y))
            {
                targets.Add(compute_target);
            }
        }
        targetPos = targets[Random.Range(0, targets.Count)];       
        //停留1秒之后移动
        int StopTime = Random.Range(1000, 3000);
        waitAndMoveCoroutine = handler.StartCoroutine(waitAndMove(StopTime));
    }
    IEnumerator waitAndMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime / 1000.0f);
        (handler as DeerHandler).ControlDeerMove(creatureId, pos_update, targetPos, new Vector2(targetPos.x - pos_x, targetPos.y - pos_y));
    }
    public override void Die()
    {
        base.Die();
        if (waitAndMoveCoroutine != null)
        {
            handler.StopCoroutine(waitAndMoveCoroutine);
        }
        if (isTriggered)
        {        
            SetAnimationBool("Idle", false);
            SetAnimationBool("Move", false);
            SetAnimationBool("Trigger", true);
        }
        else
        {
            view1.DestroyMe();
            view2.DestroyMe();
        }
    }
    public override void Update()
    {
        base.Update();
        //如果设置了目标地点，向目标移动；如果已经到达目标，则休息1秒之后随机选择目标地点
        if (isSetted)
        {
            if (!changeAnimation)
            {
                SetAnimationBool("Move", true);
                SetAnimationBool("Idle", false);
                changeAnimation = true;
            }
            if (pos_update.x == targetPos.x && pos_update.y == targetPos.y)
            {
                isSetted = false;
                SetAnimationBool("Idle", true);
                SetAnimationBool("Move", false);

                pos_x = (int)pos_update.x;
                pos_y = (int)pos_update.y;
                UpdateTargetPos();
            }
            else
            {
                float step = speed * Time.deltaTime;
                pos_update = Vector3.MoveTowards(pos_update, targetPos, step);
            }
        }
        //对于网络客户端连接，不允许其执行触发操作
        if (GameManager.Instance.FishNet_isUsingNetwork &&
        !handler.PlayerDictionary[0].NETWORK_isMainController)
        {
            return;
        }
        //检测是否有敌人位于攻击范围
        for (int i = 0; i < handler.PlayerDictionary.Count; i++)
        {
            if (i == masterId) continue;
            //当敌人位于触发范围时，触发九色鹿效果
            if (Mathf.Abs(handler.PlayerDictionary[i].pos_x - pos_x) < 2 && Mathf.Abs(handler.PlayerDictionary[i].pos_y - pos_y) < 2)
            {
                Debug.Log("触发九色鹿！");
                SetAnimationBool("Idle", false);
                SetAnimationBool("Move", false);
                SetAnimationBool("Trigger", true);
                TriggerEffect(i);
                isTriggered = true;
                break;
            }
        }
    }

}
