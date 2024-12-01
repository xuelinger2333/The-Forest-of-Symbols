using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 负责统计玩家在整个游戏流程的特殊数据，并按数据的值执行特定操作
/// </summary>

public class PlayerSpecialStatisticCounter : MonoBehaviour
{
    private static readonly object lockObject = new object(); //锁
    //这个stepCount暂时由Player在moveState中自行设置
    private List<int> Step = new List<int>();//确保线程安全，一个协程控制一个step变量

    void Start()
    {
    }
    public void AddStepCount()
    {
        lock (lockObject)
        {
            for (int i = 0; i < Step.Count; i++)
            {
                Step[i]++;
            }
        }

    }

    public void StartCountPlayerStep(float timeLimit, int targetValue, bool isLoopCount, Action OnTargetnotReached=null, Action OnTargetReached=null)
    {
        Step.Add(0);
        StartCoroutine(CountStep(Step.Count - 1, timeLimit, targetValue, isLoopCount, OnTargetnotReached, OnTargetReached));
    }

    IEnumerator CountStep(int id, float timeLimit, int targetValue, bool isLoopCount, Action OnTargetnotReached, Action OnTargetReached)
    {
        LoopLabel:
        //在timeLimit秒内统计玩家的步数是否达到了targetValue，isLoopCount标记是否应该持续统计
        lock (lockObject)
        {
            Step[id] = 0;
        }
            int count = 0;
            int delta;
            int initialValue = Step[id];
            float startTime = Time.time;
            while (Time.time - startTime < timeLimit)
            {
                delta = Step[id] - initialValue;
                initialValue = Step[id];
                
                count += delta;
                if (count >= targetValue)
                {
                    if (OnTargetReached != null)
                        OnTargetReached();
                }
                yield return null; 
            }

            if (count < targetValue)
            {
                if (OnTargetnotReached != null)
                    OnTargetnotReached();
            }

        if (isLoopCount == true) goto LoopLabel;
    }
}
