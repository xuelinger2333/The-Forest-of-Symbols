using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public interface CrowHandlerObserver
{
    void addCrowbyPosition(Vector2 pos);
    void TriggerCrow(int creatureId, int playerId);
}
//网络同步：创建，触发，销毁
public class CrowHandler : CreatureHandler
{
    private Player target, master;
    public static CrowHandler Instance { get; private set; }
    public bool [,] CrowPositionRecord { get; private set; }
    List<CrowHandlerObserver> crowHandlerObservers;
    public void AddCrowHandlerObserver(CrowHandlerObserver observer)
    {
        crowHandlerObservers.Add(observer);
    }
    private CrowHandler()
    {
        creatureNumCap = 3; //应该根据模式随时更改，此处先预设为3
    }
    protected override void Awake()
    {
        base.Awake();
        if (Instance == null) //乌鸦管理器的单例
            Instance = this;
        else
            Destroy(gameObject);
        crowHandlerObservers = new List<CrowHandlerObserver>();
    }
    protected override void Start()
    {
        base.Start();
        int width = map.width;
        int height = map.height;
        CrowPositionRecord = new bool[width, height];
    }
    //网络同步
    public void addCrowbyPosition(Vector2 pos)
    {
        for (int i = 0; i < crowHandlerObservers.Count; i++)
        {
            crowHandlerObservers[i].addCrowbyPosition(pos);
        }
        Crow creature = new Crow();
        creature.Initialize(pos);
        AddCreaturebyObject(creature);
        CrowPositionRecord[(int)pos.x, (int)pos.y] = true;
    }
    override protected void ExecuteEffect(Creature creature)
    {
        TriggerCrow(creature.creatureId, creature.triggerTarget);
        StartCoroutine(CrowEffect(target));
    } 
    //网络同步
    public void TriggerCrow(int creatureId, int targetPlayer)
    {
        Creature creature = creatures[creatureId];
        for (int i = 0; i < crowHandlerObservers.Count; i++)
        {
            crowHandlerObservers[i].TriggerCrow(creatureId, targetPlayer);
        }
        creature.isTriggered = true;
        //检查不合法的状况，注意这里乌鸦是没有主人的
        if (targetPlayer == -1 || targetPlayer == creature.masterId) return;
        target = PlayerDictionary[targetPlayer];
        //触发目标被暴露
        createUIHint(target.playerID);
    }
    
    IEnumerator CrowEffect(Player target)
    {
        target.stats.StartExposureLevel_skill();
        yield return new WaitForSeconds(2);
        target.stats.EndExposureLevel_skill();
    }

    protected override void Update()
    {
        base.Update();
        RemoveInvalidCreatures();
    }
    
    protected override void RemoveCreaturebyId(int creatureId)
    {
        CrowPositionRecord[creatures[creatureId].pos_x, creatures[creatureId].pos_y] = false;
        base.RemoveCreaturebyId(creatureId);
    }

}
