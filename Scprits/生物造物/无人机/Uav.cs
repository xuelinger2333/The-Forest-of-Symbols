using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class Uav : Creature
{
    private Vector3 targetPos;
    Vector3 finalPos;
    private bool isSetted = false;
    private float distance = 0;
    private float moveTime = 0.2f;// ConstantsValue.getMoveTimeFromSpeed(4);
    private float speed;
    //int stopTime;
    bool startScan = false;
    private UavHandler uavhandler; //用单例重载但不覆盖基类的handler
    public int category;
    private float timer = 0f;
    public Uav()
    {
        handler = UavHandler.Instance;
        uavhandler = handler as UavHandler;
    }
    public void Initialize(Vector2 initialPos, Vector2 dir, int category, int width, int height, int master, bool isUniqueSkillUav)
    {
        base.Initialize(initialPos, master);
        this.category = category;
        SetAnimationBool("Trigger", false);
        SetAnimationBool("Idle", true);
        startScan = false;
        isSetted = false;
        //初始化移动的最终目标地点
        faceDir = dir;
        finalPos = initialPos;
        while (finalPos.x >= 0 && finalPos.x < width && finalPos.y >= 0 && finalPos.y < height)
        {
            finalPos.x += dir.x;
            finalPos.y += dir.y;
            distance += 1;
        }
        //speed = distance / (moveTime * distance);
        if (!isUniqueSkillUav)
        {
            SetTarget();
        }         
        else
        {
            //停留3秒之后移动
            System.Threading.Timer timer = new System.Threading.Timer(Timeout, null, 3000, 0);
        }
            
    }
    private void Timeout(System.Object state)
    {
        SetTarget();
    }
    void SetTarget()
    {
        startScan = true;
        int stopTime = uavhandler.uav_stop_time[creatureId];
        System.Threading.Timer timer = new System.Threading.Timer(Timeout, null, stopTime - (int)(moveTime * 1000), 0);
        targetPos = new Vector3(pos_x + faceDir.x, pos_y + faceDir.y, 0);
        float distance = Vector2.Distance(targetPos, pos_update);
        isSetted = true;
        speed = distance / ((float)stopTime / 1000 * distance);
    }
    public override void Die()
    {
        base.Die();
        SetAnimationBool("Idle", false);
        SetAnimationBool("Trigger", true);
    }
    public override void Update()
    {

        //向目标移动
        if (isSetted)
        {
            
            if (pos_update.x == targetPos.x && pos_update.y == targetPos.y)
            {
                isSetted = false;
                SetTarget();
            }
            else
            {
                float step = speed * Time.deltaTime;
                pos_update = Vector3.MoveTowards(pos_update, targetPos, step);
                pos_x = (int) pos_update.x;
                pos_y = (int) pos_update.y;
            }
        }
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            SoundManager.PlaySound(SoundType.无人机扫描, 1f);
            timer = 1f;
        }
        //当网络为接收的客户端时，不做效果，除此之外的情况进行无人机收回检测
        if (GameManager.Instance.FishNet_isUsingNetwork &&
            !handler.PlayerDictionary[masterId].NETWORK_isMainController)
        {
            return;
        }
        else
        {
            if (startScan && pos_update.x == finalPos.x && pos_update.y == finalPos.y)
            {
                uavhandler.handleUavRecall(creatureId);
            }
        }
        //检测是否有敌人位于攻击范围
        for (int i = 0; i < handler.PlayerDictionary.Count; i++)
        {
            if (i == masterId) continue;
            //当敌人位于触发范围时，仅触发一次异常效果，在触发条件下，如果敌人已经离开，就取消触发的效果，
            //上层用isTriggered变量来标识触发还是取消
            if (isTriggered)
            {
                if (Mathf.Abs(handler.PlayerDictionary[i].pos_x - pos_x) >= 3 || Mathf.Abs(handler.PlayerDictionary[i].pos_y - pos_y) >= 3)
                {
                    Debug.Log("离开无人机范围！");
                    uavhandler.changeUavStopTime(250, creatureId );
                    isTriggered = false;
                    TriggerEffect(i);
                }
            }
            else if (Mathf.Abs(handler.PlayerDictionary[i].pos_x - pos_x) < 3 && Mathf.Abs(handler.PlayerDictionary[i].pos_y - pos_y) < 3)
            {
                Debug.Log("触发无人机！");
                uavhandler.changeUavStopTime( 500,creatureId);
                isTriggered = true;
                TriggerEffect(i);
            }
        }

    }
}
