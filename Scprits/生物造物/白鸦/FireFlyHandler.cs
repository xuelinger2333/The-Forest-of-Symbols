using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public interface FireFlyHandlerObserver
{
    void addFireFlybyPosition(Vector2 pos);
    void TriggerFireFly(int creatureId, int playerId);
}

public class FireFlyHandler : CreatureHandler
{
    private Player target, master;
    public static FireFlyHandler Instance { get; private set; }
    public bool[,] WhiteCrowPositionRecord { get; private set; }
    List<FireFlyHandlerObserver> fireFlyHandlerObservers;
    public void AddFireFlyHandlerObserver(FireFlyHandlerObserver observer)
    {
        fireFlyHandlerObservers.Add(observer);
    }
    private FireFlyHandler()
    {
        creatureNumCap = 3; //应该根据模式随时更改，此处先预设为3
    }
    protected override void Awake()
    {
        base.Awake();
        fireFlyHandlerObservers = new List<FireFlyHandlerObserver>();
        if (Instance == null) //萤火虫管理器的单例
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    protected override void Start()
    {
        base.Start();
        int width = map.width;
        int height = map.height;
        WhiteCrowPositionRecord = new bool[width, height];        
    }

    public void addFireFlybyPosition(Vector2 pos)
    {
        for (int i = 0; i < fireFlyHandlerObservers.Count; i++)
        {
            fireFlyHandlerObservers[i].addFireFlybyPosition(pos);
        }
        FireFly creature = new FireFly();
        creature.Initialize(pos);
        AddCreaturebyObject(creature);
        WhiteCrowPositionRecord[(int)pos.x, (int)pos.y] = true;
    }

    override protected void ExecuteEffect(Creature creature)
    {
        TriggerFireFly(creature.creatureId, creature.triggerTarget);
        foreach (KeyValuePair<int, Player> kvp in PlayerDictionary)
        {
            if (kvp.Key != creature.triggerTarget)
            {
                //除了触发目标外的玩家被暴露
                StartCoroutine(WhiteCrowEffect(kvp.Value));
            }
        }
    }
    public void TriggerFireFly(int creatureId, int targetPlayer)
    {
        Creature creature = creatures[creatureId];
        for (int i = 0; i < fireFlyHandlerObservers.Count; i++)
        {
            fireFlyHandlerObservers[i].TriggerFireFly(creatureId, targetPlayer);
        }
        creature.isTriggered = true;
        //检查不合法的状况，注意这里萤火虫是没有主人的
        if (targetPlayer == -1 || targetPlayer == creature.masterId) return;
        target = PlayerDictionary[targetPlayer];
        //触发目标被暴露
        createUIHint(target.playerID);
    }
    IEnumerator WhiteCrowEffect(Player target)
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
        WhiteCrowPositionRecord[creatures[creatureId].pos_x, creatures[creatureId].pos_y] = false;
        base.RemoveCreaturebyId(creatureId);
    }
}
