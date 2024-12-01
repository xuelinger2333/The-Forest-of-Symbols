using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public interface CharacterObserver
{
    public void OnNotify(Character c, Operation op);
}
public class Character : Player, IsSickable
{
    public bool CanUsePassiveSkill = true;
    BlackBuff blackBuff;
    [HideInInspector]
    public bool isReadyForAttack;
    public float dizzyTime = 0;

    public  float activeSkillCD, activeSkillTimer;

    public int uniqueSkillLimit, uniqueSkillCount = 0;
    public float uniqueSkillAddCountCD, uniqueSkillAddCountTimer;
    public int uniqueSkillAddCountUnit;
    public float uniqueSkillUnit_mutiply = 1;
    protected List<CharacterObserver> characterObservers = new List<CharacterObserver>();
    
    #region State
    public IdleState idleState { get; protected set; }
    public MoveState moveState { get; protected set; }
    public MainAttackState mainAttackState { get; protected set; }
    public MainAttackBreakState mainAttackBreakState { get; protected set; }
    public ChargeState chargeState { get; protected set; }
    public ChargeAttackState chargeAttackState { get; protected set; }
    public DizzyState dizzyState { get; protected set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        blackBuff = GetComponent<BlackBuff>();
        
    }

     protected override void Start()
    {
        base.Start();
        map.UpdateMap(pos_x, pos_y, this);
    }
    protected bool HACKER_EmPulse_Effect()
    {
        int key_fail_percent = stats.HACKER_emPulseLayerCount * 8;
        int random = UnityEngine.Random.Range(0, 100);
        if (random < key_fail_percent) return false;
        return true;
    }

    public void AddCharacterObserver(CharacterObserver observer)
    {
        characterObservers.Add(observer);
    }
    public void StartCameraCover()
    {
        //设置playerStats中的数值
        stats.CAMERA_bushCover += 1;
        //最多播放两层动画
        int l = math.min(2, stats.CAMERA_bushCover);
        action.ChangeAction(Actions.CameraCoverAction, l);
    }
    public void EndCameraCover()
    {
        //设置playerStats中的数值
        stats.CAMERA_bushCover -= 1;
        //最多播放两层动画
        int l = math.min(2, stats.CAMERA_bushCover);
        action.ChangeAction(Actions.CameraCoverAction, l);
    }
    public virtual void OnActiveSkill() {
        //注意，骇客没有同步这些改动
        if (!HACKER_EmPulse_Effect()) return;
        if (stateMachine.currentState != idleState || activeSkillTimer > 0)
            return;
        ActiveSkill_Execute();
        for (int i = 0; i < characterObservers.Count; i++)
        {
            characterObservers[i].OnNotify(this, Operation.ActiveSkill);
        }
    }
    protected virtual void ActiveSkill_Execute(){ }
    public virtual void OnActiveSkillCancel() { }
    public void OnUniqueSkill() {
        if (!HACKER_EmPulse_Effect()) return;
        if (uniqueSkillCount < uniqueSkillLimit || stateMachine.currentState != idleState)
            return;
        UniqueSkill_Execute();
        for (int i = 0; i < characterObservers.Count; i++)
        {
            characterObservers[i].OnNotify(this, Operation.UniqueSkill);
        }
    }
    protected virtual void UniqueSkill_Execute() { }
    public void OnCharge()
    {
        if (!HACKER_EmPulse_Effect()) return;
        if (stateMachine.currentState == idleState)  stateMachine.ChangeState(chargeState);
    }
    public void OnChargeCancel()
    {
        if (stateMachine.currentState == chargeState) stateMachine.ChangeState(idleState);
    }
    public virtual void OnMainAttack()
    {
        if (!HACKER_EmPulse_Effect()) return;
        if (stateMachine.currentState == idleState)
        { 
            stateMachine.ChangeState(mainAttackState);
        }
            
    }
    public virtual void OnMove(Vector2 input)
    {
        if (!HACKER_EmPulse_Effect()) return;
        if (stats.isWitched || stats.GROUND_isThornImprison)
        {//在魅惑状态下不可移动
            stats.isMoving = false;
            return;
        }
        inputDirection = input;
        if (stats.GROUND_isMushroomPoisoned != 0)
        {
            inputDirection = UtilFunction.ComputeDirectionRotateforPlayer(inputDirection, 2);
        }
        if (stats.isEntranced) //人偶新娘的绝招效果
        {
            int randomInt = UnityEngine.Random.Range(1, 4);
            while (randomInt == Mathf.Abs(map.rotationCounts1 - map.rotationCounts2)) { randomInt = UnityEngine.Random.Range(1, 4); }

            inputDirection = UtilFunction.ComputeDirectionRotateforPlayer(inputDirection, randomInt);
        }
        if (stateMachine.currentState == chargeState && isReadyForAttack) stateMachine.ChangeState(chargeAttackState);
        stats.isMoving = true;
        int mapRotationCount = (playerID) ? map.rotationCounts1 : map.rotationCounts2;
        aimDir = UtilFunction.ComputeDirectionRotateforPlayer(inputDirection, mapRotationCount);
    }
    public void OnMoveCancel()
    {
        stats.isMoving = false;
        inputDirection = Vector2.zero;
        if (enemy && enemy.stats.isWitched)
        {
            enemy.stats.isMoving = false;
        }
    }
    public float2 getActiveSkillInfo() { return new float2(activeSkillTimer, activeSkillCD); }
    public float2 getUniqueSkillInfo() { return new float2(uniqueSkillCount, uniqueSkillLimit); }
    public virtual float2 getPassiveSkillInfo() { return new float2(); }

    protected override void Update()
    {
        base.Update();
        //暗黑涌动buff的三个减弱条件
        if (blackBuff)
        {
            if (!blackBuff.isEnterEnemyScreen && IsInScreenRange())
            {
                blackBuff.EnterEnemyScreen();
            }
            if (!blackBuff.isModelExposure_3 && (IsInScreenRange() && stats.exposureLevel_model >= 2))
            {
                blackBuff.ModelExposure_3();
            }
            if (!blackBuff.isSkillExposure && (stats.exposureLevel_skill > 0))
            {
                blackBuff.SkillExposure();
            }
        }


        if (stats.CREATURE_isSnakeBite && stateMachine.currentState == chargeState)
        {
            stateMachine.ChangeState(idleState);
        }
        if (stateMachine.currentState == idleState && moveCD < 0 && stats.isMoving)
        {
            if (enemy && enemy.stats.isWitched) //人偶新娘的主动技能效果
            {
                Debug.Log("正在被魅惑");
                enemy.inputDirection = inputDirection;
                enemy.stats.isMoving = true;
            }
            if (stats.isWitched || stats.GROUND_isThornImprison || stats.CREATURE_isSnakeBite) stats.isMoving = false;
            if (!(stats.GROUND_isThornImprison || stats.CREATURE_isSnakeBite))
            {
                stateMachine.ChangeState(moveState);
            }
        }

    }
    public void beSick(int x, int y)
    {
        stats.DeathPlagueOnPlayer();
    }
    public bool IsInScreenRange()
    {
        int delta_x = math.abs(enemy.pos_x - pos_x);
        int delta_y = math.abs(enemy.pos_y - pos_y);
        //通过判断x，y是否两项差距都小于等于 屏幕长的一半：2，来判断是否符合要求
        if (delta_x <= 2 && delta_y <= 2)
            return true;
        else return false;
    }
}