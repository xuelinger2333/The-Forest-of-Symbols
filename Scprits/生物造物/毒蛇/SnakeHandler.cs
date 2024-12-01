using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface SnakeHandlerObserver
{
    void addSnakebyPosition(Vector2 pos, int targetId);
    void ControlSnakeMove(int creatureId, Vector2 preTarget, Vector2 targetPos, Vector2 faceDir);
    void TriggerSnake(int creatureId);
}

public class SnakeHandler : CreatureHandler
{
    private Player target;
    public static SnakeHandler Instance { get; private set; }
    List<SnakeHandlerObserver> snakeHandlerObservers;
    public void AddSnakeHandlerObserver(SnakeHandlerObserver observer)
    {
        snakeHandlerObservers.Add(observer);
    }
    private SnakeHandler()
    {
        creatureNumCap = 2; //玩家2人最多2条
    }
    protected override void Awake()
    {
        snakeHandlerObservers = new List<SnakeHandlerObserver>();
        if (Instance == null) //管理器的单例
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        base.Awake();
    }
    //网络同步
    public void addSnakebyPosition(Vector2 pos, int targetId)
    {
        for (int i = 0; i < snakeHandlerObservers.Count; i++)
        {
            snakeHandlerObservers[i].addSnakebyPosition(pos, targetId);
        }
        Snake creature = new Snake();
        creature.Initialize(pos, targetId, map.width, map.height);
        AddCreaturebyObject(creature);
        createUIHint(Convert.ToBoolean(targetId));
        creature.SetTargetForSnake();
    }
    //网络同步
    public void ControlSnakeMove(int creatureId, Vector2 preTarget, Vector2 targetPos, Vector2 faceDir)
    {
        for (int i = 0; i < snakeHandlerObservers.Count; i++)
        {
            snakeHandlerObservers[i].ControlSnakeMove(creatureId, preTarget, targetPos, faceDir);
        }
        Snake d = creatures[creatureId] as Snake;
        if (d.pos_update != preTarget)
            d.pos_update = preTarget;
        d.faceDir = faceDir;
        d.targetPos = targetPos;
        d.isSetted = true;
    }
    override protected void ExecuteEffect(Creature creature)
    {        
        int targetPlayer = creature.triggerTarget;
        //检查不合法的状况，注意这里蛇是没有主人的
        if (targetPlayer == -1 || targetPlayer == creature.masterId) return;
        target = PlayerDictionary[targetPlayer];
        TriggerSnake(creature.creatureId);
        //触发目标被暴露
        target.stats.CREATURE_ChangeSnakeBiteState(2);
    }
    public void TriggerSnake(int creatureId)
    {
        for (int i = 0; i < snakeHandlerObservers.Count; i++)
        {
            snakeHandlerObservers[i].TriggerSnake(creatureId);
        }
        Creature creature = creatures[creatureId];
        creature.isTriggered = true;
    }
    protected override void Update()
    {
        base.Update();
        RemoveInvalidCreatures();
    }
}
