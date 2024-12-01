using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
//注：不指定生物的creatureID,就生成生物，有可能会导致id分配不一致问题，如果以后碰到再解决
public interface DeerHandlerObserver
{
    void addDeerbyPosition(Vector2 pos);
    void ControlDeerMove(int creatureId,Vector2 preTarget, Vector2 targetPos, Vector2 faceDir);
    void TriggerDeer(int creatureId, int playerId);
}
public class DeerHandler : CreatureHandler
{
    private Player target;
    public static DeerHandler Instance { get; private set; }
    List<DeerHandlerObserver> deerHandlerObservers;
    public void AddDeerHandlerObserver(DeerHandlerObserver observer)
    {
        deerHandlerObservers.Add(observer);
    }
    private DeerHandler()
    {
        creatureNumCap = 3; //应该根据模式随时更改，此处先预设为3
    }
    protected override void Awake()
    {
        deerHandlerObservers = new List<DeerHandlerObserver>();
        if (Instance == null) //鹿管理器的单例
            Instance = this;
        else
            Destroy(gameObject);
        base.Awake();
    }
    //网络同步
    public void addDeerbyPosition(Vector2 pos)
    {
        for (int i = 0; i < deerHandlerObservers.Count; i++)
        {
            deerHandlerObservers[i].addDeerbyPosition(pos);
        }
        Deer creature = new Deer();
        creature.Initialize(pos, map.width, map.height);
        AddCreaturebyObject(creature);
    }
    //网络同步
    public void ControlDeerMove(int creatureId,Vector2 preTarget, Vector2 targetPos, Vector2 faceDir)
    {
        for (int i = 0; i < deerHandlerObservers.Count; i++)
        {
            deerHandlerObservers[i].ControlDeerMove(creatureId, preTarget, targetPos, faceDir);
        }
        Deer d = creatures[creatureId] as Deer;
        if (d.pos_update != preTarget)
            d.pos_update = preTarget;
        d.faceDir = faceDir;
        d.targetPos = targetPos;
        d.changeAnimation = false;
        d.isSetted = true;
    }
    override protected void ExecuteEffect(Creature creature)
    {
        TriggerDeer(creature.creatureId, creature.triggerTarget);
        StartCoroutine(DeerEffect(target));
    }
    //网络同步
    public void TriggerDeer(int creatureId, int targetPlayer)
    {
        Creature creature = creatures[creatureId];
        for (int i = 0; i < deerHandlerObservers.Count; i++)
        {
            deerHandlerObservers[i].TriggerDeer(creatureId, targetPlayer);
        }
        creature.isTriggered = true;
        //检查不合法的状况，注意这里鹿是没有主人的
        if (targetPlayer == -1 || targetPlayer == creature.masterId) return;
        target = PlayerDictionary[targetPlayer];
        //触发目标被暴露
        createUIHint(target.playerID);
    }
    IEnumerator DeerEffect(Player target)
    {        
        //触发目标四速+1
        target.stats.SetPlayerMoveSpeed(target.stats.currentMoveSpeed + 1);
        target.stats.SetPlayerAttackSpeed(target.stats.currentAttackSpeed + 1);
        target.stats.SetPlayerAttackDelaySpeed(target.stats.currentAttackDelaySpeed + 1);
        target.stats.SetPlayerAccumulateSpeed(target.stats.currentAccumulateSpeed + 1);
        yield return new WaitForSeconds(3);
        target.stats.SetPlayerMoveSpeed(target.stats.currentMoveSpeed - 1);
        target.stats.SetPlayerAttackSpeed(target.stats.currentAttackSpeed - 1);
        target.stats.SetPlayerAttackDelaySpeed(target.stats.currentAttackDelaySpeed - 1);
        target.stats.SetPlayerAccumulateSpeed(target.stats.currentAccumulateSpeed - 1);
    }
    protected override void Update()
    {
        base.Update();
        RemoveInvalidCreatures();
    }
    protected override void RemoveCreaturebyId(int creatureId)
    {
        int pos_x = creatures[creatureId].pos_x;
        int pos_y = creatures[creatureId].pos_y;
        base.RemoveCreaturebyId(creatureId);
        if (map.mapData[pos_x, pos_y].type != "象征之森")
            CreatureGenerator.Instance.GenerateDeer();
    }
}
