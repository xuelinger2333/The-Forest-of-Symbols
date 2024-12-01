using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public interface UavHandlerObserver
{
    void addUavbyPosition(int playerId, Vector2 pos, Vector2 direction, bool isUniqueSkillUav, int uavCategory_offset);
    void changeUavStopTime(int stopTime, int creatureId);
    void handleUavRecall(int creatureId);
}
//同步方法: 收回，销毁，移动速度
public class UavHandler : CreatureHandler
{
    private Player target, master;
    public static UavHandler Instance { get; private set; }
    List<UavHandlerObserver> uavHandlerObservers;
    public Dictionary<int, int> uav_stop_time = new Dictionary<int, int>(); //uav 的速度字典
    public void AddUavHandlerObserver(UavHandlerObserver observer)
    {
        uavHandlerObservers.Add(observer);
    }
    private UavHandler()
    {
        creatureNumCap = 225;
    }
    protected override void Awake()
    {
        base.Awake();
        uavHandlerObservers = new List<UavHandlerObserver>();
        if (Instance == null) //无人机管理器的单例
            Instance = this;
        else
            Destroy(gameObject);
    }
    //具有网络同步
    public void changeUavStopTime(int stopTime, int creatureId)
    {
        for (int i = 0; i < uavHandlerObservers.Count; i++)
        {
            uavHandlerObservers[i].changeUavStopTime(stopTime, creatureId);
        }
        uav_stop_time[creatureId] = stopTime;
    }
    //具有网络同步
    public void addUavbyPosition(int playerId, Vector2 pos, Vector2 direction, bool isUniqueSkillUav, int uavCategory_offset)
    {
        for (int i = 0; i < uavHandlerObservers.Count; i++)
        {
            uavHandlerObservers[i].addUavbyPosition(playerId, pos, direction, isUniqueSkillUav, uavCategory_offset);
        }
        Uav creature = new Uav();
        AddCreaturebyObject(creature);        
        changeUavStopTime(250, creature.creatureId);  
        creature.Initialize(pos, direction, uavCategory_offset, map.width, map.height, playerId, isUniqueSkillUav); 
    }

    override protected void ExecuteEffect(Creature creature)
    { 
        int targetPlayer = creature.triggerTarget;
        //检查不合法的状况
        if (targetPlayer == -1 || targetPlayer == creature.masterId || creature.masterId == -1) return;
        target = PlayerDictionary[targetPlayer];
        master = PlayerDictionary[creature.masterId];
        //如果触发flag为真，就施加效果，如果为假，则取消效果
        if (creature.isTriggered)
        {
            target.stats.StartExposureLevel_skill();
            target.stats.HACKER_StartEmPulse();
            //这里预期无人机目标在4秒后不会做改变
            //4人游戏有问题！！
            System.Threading.Timer timer = new System.Threading.Timer(Timeout, null, 4000, 0);
        }
        else
        { 
            target.stats.HACKER_EndEmPulse();
        }
    }
    void Timeout(System.Object state)
    {
        target.stats.EndExposureLevel_skill();
    }
    public int MasterRecallAllUAVs(int masterID)
    {
        int recall_number = 0;
        List<int> keys = new List<int>(creatures.Keys);
        for (int i = keys.Count - 1; i >= 0; i--)
        {
            int key = keys[i];
            if (creatures.ContainsKey(key) && creatures[key].masterId == masterID)
            {
                handleUavRecall(key);
                recall_number += 1;
            }
        }
        return recall_number;
    }
    //具有网络同步
    public void handleUavRecall(int creatureId)
    {
        for (int i = 0; i < uavHandlerObservers.Count; i++)
        {
            uavHandlerObservers[i].handleUavRecall(creatureId);
        }
        if (!creatures.ContainsKey(creatureId)) return;
        //如果无人机正在施加效果，将这个效果取消
        Creature uav = creatures[creatureId];
        uav.SetAnimationBool("Trigger", true);
        uav.SetAnimationBool("Idle", false);
        int targetPlayer = uav.triggerTarget;
        //检查不合法的状况
        if (targetPlayer != -1 && targetPlayer != uav.masterId && uav.masterId != -1)
        {
            target = PlayerDictionary[uav.triggerTarget];
            if (uav.isTriggered)
            {
                Debug.Log("因无人机销毁而离开无人机范围！");
                //target.stats.EndExposureLevel_skill();
                target.stats.HACKER_EndEmPulse();
            }
        }

        //销毁无人机并减少骇客的CD；
        ShadowHacker masterH = PlayerDictionary[uav.masterId] as ShadowHacker;
        masterH.CDController.setActiveSkillTimer(masterH.getActiveSkillInfo().x - 1);
        RemoveCreaturebyId(creatureId);
    }
    protected override void Update()
    {
        List<int> keys = new List<int>(creatures.Keys);

        for (int i = keys.Count - 1; i >= 0; i--)
        {
            int key = keys[i];
            if (creatures.ContainsKey(key))
            {
                if (creatures[key].pos_x >= 0 && creatures[key].pos_x < map.width &&
                    creatures[key].pos_y >= 0 && creatures[key].pos_y < map.height &&
                    map.mapData[creatures[key].pos_x, creatures[key].pos_y].type == "象征之森")
                {
                    handleUavRecall(key);
                }
                else creatures[key].Update();
            }
        }
    }
    public int QueryUAVCategoryById(int creatureId)
    {
        if (creatureId < creatureNumCap && creatures.ContainsKey(creatureId))
        {
            Uav uav = (Uav)creatures[creatureId];
            return uav.category;
        }
        else return 0;
    }
}
