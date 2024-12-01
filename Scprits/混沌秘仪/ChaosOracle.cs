using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//在网络的客户端，不应该部署ChaosOracle，只应该同步各个Chaos的结果
public interface Chaos
{
    //每个事件包括逻辑的处理，以及提示文字的处理
    public void StartChaos();
}
public class ChaosOracle : MonoBehaviour, CharacterObserver, PlayerStatsObserver
{
    //私有，重要变量：能量点数
    [SerializeField] int ChaosPoint = 0;
    const int ChoasTriggerPoint = 20;
    [SerializeField] Timer Timer_Chaos;
    [SerializeField] Timer Timer_AddChoasPoint;
    [SerializeField] Timer Timer_skillExposureCD;
    [SerializeField] Timer Timer_modelExposureCD;
    [SerializeField] PlayerStats s0, s1;
    List<Chaos> ChaosPool = new List<Chaos>();
    private void Start()
    {
        Timer_AddChoasPoint.targetTime = 5;
        Timer_AddChoasPoint.StartTimer(Timer_AddPointTimeout);
        (GameManager.Instance.player0 as Character).AddCharacterObserver(this);
        (GameManager.Instance.player1 as Character).AddCharacterObserver(this);
        s0.AddStatsObserver(this);
        s1.AddStatsObserver(this);
    }
    void Timer_AddPointTimeout()
    {
        AddChaosPoint(1); //最终应该修改为1
        Timer_AddChoasPoint.targetTime = 5;
        Timer_AddChoasPoint.StartTimer(Timer_AddPointTimeout);
    }    
    public void OnNotify(Character c, Operation op)
    {
        if (op == Operation.ActiveSkill)
        {
            AddChaosPoint(1);
        }
        if (op == Operation.UniqueSkill)
        {
            AddChaosPoint(2);
        }
    }
    public void OnNotify(PlayerStats playerStats, StatsData data)
    {
        if (data == StatsData.SKILL_EXPO_START)
        {
            if (Timer_skillExposureCD.Timeout)
            {
                Timer_skillExposureCD.targetTime = 3;
                Timer_skillExposureCD.StartTimer();
                AddChaosPoint(1);
            }
        }
        if (data == StatsData.MODEL_EXPO)
        {
            if (playerStats.exposureLevel_model >= 2 && Timer_modelExposureCD.Timeout)
            {
                Timer_modelExposureCD.targetTime = 3;
                Timer_modelExposureCD.StartTimer();
                AddChaosPoint(1);
            }
        }
    }    
    public void AddChaos(Chaos c)
    {
        ChaosPool.Add(c);
    }

    void AddChaosPoint(int num)
    {
        //控制能量点数的加减
        ChaosPoint += num;
        if (Timer_Chaos.Timeout && ChaosPoint >= ChoasTriggerPoint)
        {
            ChaosPoint -= 20;
            //触发事件
            StartChaos();
        }
    }
    void StartChaos()
    {
        //事件触发的入口函数
        //重置事件触发timer
        Timer_Chaos.targetTime = 20;
        Timer_Chaos.StartTimer();
        //从所有事件库中随机抽取一个触发
        if (ChaosPool.Count != 0)
        {
            int idx = Random.Range(0, ChaosPool.Count);
            //触发事件
            ChaosPool[idx].StartChaos();
        }
    }
}
