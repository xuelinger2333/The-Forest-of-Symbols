using FishNet.Demo.AdditiveScenes;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
public interface IsSickable
{
    void beSick(int x = 0, int y = 0);
}
public class PlagueDoctor : Character
{
    
    float passiveSkillCD = 6f;
    float passiveSkillTimer = 0;
    Timer skillTimer; //保证必定有一个timer
    #region State
    public PlagueDoctorActiveSkillState activeSkillState { get; private set; }
    public PlagueDoctorUniqueSkillState uniqueSkillState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        characterName = "瘟疫医生";
        activeSkillCD = 14.0f;
        activeSkillTimer = 0;
        uniqueSkillLimit = 100;
        uniqueSkillCount = 0;
        uniqueSkillAddCountCD = 1f;
        uniqueSkillAddCountTimer = 0f;
        uniqueSkillAddCountUnit = 4;

        faceDir = new Vector2(1, 0);
        idleState = new PlagueDoctorIdleState(this.playerViewAdapter, enemy, stateMachine, "idle", this);
        moveState = new PlagueDoctorMoveState(this.playerViewAdapter, enemy, stateMachine, "move", this);
        mainAttackState = new PlagueDoctorMainAttackState(this.playerViewAdapter, enemy, stateMachine, "mainAttack", this);
        mainAttackBreakState = new MainAttackBreakState(this.playerViewAdapter, enemy, stateMachine, "tired", this);
        activeSkillState = new PlagueDoctorActiveSkillState(this.playerViewAdapter, enemy, stateMachine, "activeSkill", this);
        uniqueSkillState = new PlagueDoctorUniqueSkillState(this.playerViewAdapter, enemy, stateMachine, "uniqueSkill", this);
        chargeState = new ChargeState(this.playerViewAdapter, enemy, stateMachine, "charge", this);
        chargeAttackState = new ChargeAttackState(this.playerViewAdapter, enemy, stateMachine, "chargeAttack", this);
        dizzyState = new DizzyState(this.playerViewAdapter, enemy, stateMachine, "dizzy", this);
    }
    protected override void Start()
    {
        base.Start();
        skillTimer = GetComponent<Timer>();
        stateMachine.Initialize(idleState);
    }
    public override void OnMainAttack()
    {
        base.OnMainAttack();
        //如果正在攻击状态，就触发一次被动
        if (stateMachine.currentState == mainAttackState)
        {
            PassiveSkill();
        }
    }
    protected override void ActiveSkill_Execute()
    {
        CDController.setActiveSkillTimer(activeSkillCD);
        stateMachine.ChangeState(activeSkillState);
    }

    protected override void UniqueSkill_Execute()
    {
        CDController.AddUniqueSkillCount(-uniqueSkillLimit);
        stateMachine.ChangeState(uniqueSkillState);
    }
    //正式执行主动技能的函数
    public void AcitiveSkill()
    {
        //给与一个新的被动技能cd
        passiveSkillCD = 2;
        //6秒后回归原样
        skillTimer.targetTime = 6.1f;
        skillTimer.StartTimer(ResetPassiveSkill);
    }
    void ResetPassiveSkill()
    {
        //回归原样的回调
        passiveSkillCD = 6;
    }
    public void PassiveSkill()
    {
        if (!CanUsePassiveSkill)
            return;
        //播放特效
        action.ChangeAction(Actions.PDPassiveAction, 0);
        //释放瘟疫
        applyPlague(1, true);
    }
    protected override void Update()
    {
        base.Update();
        
        if (passiveSkillTimer < passiveSkillCD) passiveSkillTimer += Time.deltaTime;
        else
        {
            PassiveSkill();
            passiveSkillTimer = 0;
        }

    }    
    public override float2 getPassiveSkillInfo() { 
        return new float2(Mathf.Clamp((passiveSkillCD - passiveSkillTimer), 0, passiveSkillCD), passiveSkillCD);
    }
    public void applyPlague(int maxDistance, bool selfNotAffected)
    {
        for (int x = -maxDistance; x <= maxDistance; x++)
        {
            for (int y = -maxDistance; y <= maxDistance; y++)
            {
                if (selfNotAffected && x == 0 && y == 0) continue;
                int real_x = pos_x + x; int real_y = pos_y + y;
                if (real_x > -1 && real_x < map.width && real_y > -1 && real_y < map.height)
                {
                    map.beSick(real_x, real_y);
                }
            }
        }
        if (enemy && Mathf.Abs(enemy.pos_x - pos_x) <= maxDistance && Mathf.Abs(enemy.pos_y - pos_y) <= maxDistance)
        {
            if (selfNotAffected && pos_x == enemy.pos_x && pos_y == enemy.pos_y)
                return;
            Character e = enemy as Character;
            if (e != null)
            {
                e.beSick(-1, -1);
            }
        }
    }
}
