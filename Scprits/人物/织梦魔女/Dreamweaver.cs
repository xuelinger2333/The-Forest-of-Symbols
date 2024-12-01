using FishNet.Demo.AdditiveScenes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dreamweaver : Character
{
    public Vector2Int posOfCoffin;

    Timer Timer_CoffinExist; //记录棺椁存活时长的timer
    

    private List<Vector2> posOfShadow = new List<Vector2>();
    #region State
    public DreamweaverActiveSkillState activeSkillState { get; private set; }
    public DreamweaverUniqueSkillState uniqueSkillState { get; private set; }
    public DreamweaverRebornState rebornState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        characterName = "织梦魔女";
        activeSkillCD = 18.0f;
        activeSkillTimer = 0;
        uniqueSkillLimit = 100;
        uniqueSkillCount = 0;
        uniqueSkillAddCountCD = 1f;
        uniqueSkillAddCountTimer = 0f;
        uniqueSkillAddCountUnit = 4; 
        faceDir = new Vector2(1, 0);

        idleState = new DreamweaverIdleState(this.playerViewAdapter, enemy, stateMachine, "idle", this);
        moveState = new DreamweaverMoveState(this.playerViewAdapter, enemy, stateMachine, "move", this);
        mainAttackState = new DreamweaverMainAttackState(this.playerViewAdapter, enemy, stateMachine, "mainAttack", this);
        mainAttackBreakState = new MainAttackBreakState(this.playerViewAdapter, enemy, stateMachine, "mainAttackDelay", this);
        activeSkillState = new DreamweaverActiveSkillState(this.playerViewAdapter, enemy, stateMachine, "activeSkill", this);

        uniqueSkillState = new DreamweaverUniqueSkillState(this.playerViewAdapter, enemy, stateMachine, "uniqueSkill", this);
        chargeState = new ChargeState(this.playerViewAdapter, enemy, stateMachine, "charge", this);
        chargeAttackState = new DreamweaverChargeAttackState(this.playerViewAdapter, enemy, stateMachine, "chargeAttack", this);
        rebornState = new DreamweaverRebornState(this.playerViewAdapter, enemy, stateMachine, "reborn", this);
        dizzyState = new DizzyState(this.playerViewAdapter, enemy, stateMachine, "dizzy", this);
    }
    protected override void Start()
    {
        base.Start();
        Timer_CoffinExist = GetComponent<Timer>();
        stateMachine.Initialize(idleState);
        for (int x = -2; x < 3; x++)
        {
            for (int y = -2; y < 3; y++)
            {
                posOfShadow.Add(new Vector2(x, y));
            }
        }
    }
    protected override void ActiveSkill_Execute()
    {
        CDController.setActiveSkillTimer(activeSkillCD);
        //播放0.4秒的歌唱动画，不需要动画停留
        action.ChangeAction(Actions.DWSingAction, 0.75f);
        stateMachine.ChangeState(activeSkillState);
    }

    protected override void UniqueSkill_Execute()
    {
        CDController.AddUniqueSkillCount(-uniqueSkillLimit);
        action.ChangeAction(Actions.DWSingAction, 1.5f);
        //播放1.5秒的吟唱动画，需要动画停留，已做在动画机中状态
        stateMachine.ChangeState(uniqueSkillState);
    }

    public override float2 getPassiveSkillInfo() { return new float2(0, 1); }
    public void CreateCoffin()
    {
        //如果现在已经有棺椁存在，销毁棺椁，会改变存在变量
        if (stats.isCoffinExist)
        {
            DestroyCoffin();
        }
        //更改存在变量
        stats.isCoffinExist = true;
        //新建特效trigger
        action.ChangeAction(Actions.DWCoffinAction, 0);
        //记录新的棺椁位置
        posOfCoffin = new Vector2Int(pos_x, pos_y);
        //更新棺椁存活timer
        Timer_CoffinExist.targetTime = 10;
        //开始计时10秒存活时间
        Timer_CoffinExist.StartTimer(DestroyCoffin);
    }

    public void DestroyCoffin()
    {
        action.ChangeAction(Actions.StopDWCoffinAction, 0);
        stats.isCoffinExist = false;
    }

    public override void Die(string reason)
    {
        //如果主动技能状态中
        if (stats.isDream)
        {
            //进入重生状态
            stateMachine.ChangeState(rebornState);
        }
        else if (stats.isCoffinExist)
        {
            //如果那个位置既不是毒圈，也不是障碍物
            if (map.mapData[posOfCoffin.x, posOfCoffin.y].type != "象征之森" && map.mapData[posOfCoffin.x, posOfCoffin.y].type != "障碍物")
            {
                stateMachine.ChangeState(rebornState);
            }
            else base.Die(reason);
        }
        else base.Die(reason);
    }

    public void Reborn()
    {
        //根据重生方式设置重生位置，并给予重生效果
        //调用时，已保证在主动技能下，或者在棺椁可用下
        //给予重生效果
        RebornEffect();
        //如果在主动技能范围内
        if (stats.isDream)
        {
            //随机重生
            int randomInt = UnityEngine.Random.Range(0, map.posOfGrass.Count);
            while (map.mapData[(int)map.posOfGrass[randomInt].x, (int)map.posOfGrass[randomInt].y].type == "象征之森"
                    || map.mapData[(int)map.posOfGrass[randomInt].x, (int)map.posOfGrass[randomInt].y].type == "障碍物")
            {
                randomInt = UnityEngine.Random.Range(0, map.posOfGrass.Count);
            }
            Vector2 pos = map.posOfGrass[randomInt];
            pos_update = pos;
            pos_x = (int)pos.x;
            pos_y = (int)pos.y;
            Debug.Log("织梦魔女随机重生");
            return;
        }
        //如果目前有棺椁存在，则以棺椁重生
        if (stats.isCoffinExist)
        {
            pos_update = posOfCoffin;
            pos_x = posOfCoffin.x;
            pos_y = posOfCoffin.y;
            DestroyCoffin();
            Debug.Log("织梦魔女以棺椁重生");
            return;
        }
    }
    private void RebornEffect()
    {
        SoundManager.PlaySound(SoundType.暗影覆面, 0.4f);
        SoundManager.PlaySound(SoundType.魔女复生, 0.4f);
        CDController.AddUniqueSkillCount(25);
        stats.shadowOnFace += 1;
        SetAnimationInt("shadowOnFace", stats.shadowOnFace);
        for (int i = 0; i < 5; i++)
        {
            int index = UnityEngine.Random.Range(0, posOfShadow.Count);
            Camera parent;
            parent = enemy.myCamera;
            GameObject instance = Instantiate(prefabControl.shadowPrefab);
            instance.transform.position = new Vector3((int)posOfShadow[index].x, (int)posOfShadow[index].y - 0.5F, 5);
            instance.transform.SetParent(parent.transform, false);
            posOfShadow.RemoveAt(index);
        }

        switch (stats.shadowOnFace)
        {
            case 1:
                activeSkillCD = activeSkillCD / 2;
                break;
            case 3:
                //修改乘数
                uniqueSkillUnit_mutiply = 2;
                break;
            case 5:
                enemy.Die("Attack");
                break;
        }
    }

    public void CreatChargeAttackEffect(Vector3 pos)
    {
        GameObject instance = TriggerManager.ExecuteTrigger("织梦魔女蓄力", null);
        //hack： playerId居然是mapId的否？！
        instance.GetComponent<TransfromSetTrigger>().SetTransformPosition(new Vector2(pos_x + pos.x, pos_y + pos.y), !playerID, true);
    }
}
