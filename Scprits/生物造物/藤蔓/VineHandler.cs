using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using UnityEditor.Rendering;
using UnityEngine;

public class VineHandler : CreatureHandler
{
    public static int TotalVineNum = 0;
    private Player target, master;
    public static VineHandler Instance { get; private set; }
    private VineHandler() {
        creatureNumCap = 3;
    }
    protected override void Awake() 
    {
        base.Awake();
        if (Instance == null) //藤蔓管理器的单例
            Instance = this;
        else
            Destroy(gameObject);
    }
    public void addVinebyPosition(int playerId, Vector2 pos)
    {
        TotalVineNum++;
        Vine creature = new Vine();
        creature.Initialize(pos, TotalVineNum, playerId);

        int firstCreatedIndex = int.MaxValue;
        int firstCreatedId = 0;
        foreach(KeyValuePair<int, Creature> kvp in creatures)
        {
            Vine vine = kvp.Value as Vine;
            if (vine != null)
            {
                if (vine.historicalIndex < firstCreatedIndex)
                {
                    firstCreatedIndex = vine.historicalIndex;
                    firstCreatedId = vine.creatureId;
                }
            }
        }
        //当场地藤蔓已经达到3棵时，先除去第一棵，再添加藤蔓
        AddCreaturebyObject(creature, firstCreatedId); 
    }

    override protected void ExecuteEffect(Creature creature)
    {
        int targetPlayer = creature.triggerTarget;
        //检查不合法的状况
        if (targetPlayer == -1 || targetPlayer == creature.masterId || creature.masterId == -1) return;

        target = PlayerDictionary[targetPlayer];
        master = PlayerDictionary[creature.masterId];
        //藤蔓目标受到禁锢效果
        //target.stats.ChangeImprisonState();
        target.stats.isMoving = false;
        //藤蔓主人获得2秒双速加1
        master.stats.SetPlayerMoveSpeed(master.stats.currentMoveSpeed + 1);
        master.stats.SetPlayerAttackSpeed(master.stats.currentAttackSpeed + 1);
        //Timer timer = new Timer(Timeout, null, 2000, 0); 
    }

    private void Timeout(System.Object state)
    {
        master.stats.SetPlayerMoveSpeed(master.stats.currentMoveSpeed - 1);
        master.stats.SetPlayerAttackSpeed(master.stats.currentAttackSpeed - 1);
    }

    protected override void Update()
    {
        base.Update();
        RemoveInvalidCreatures();
    }
}
