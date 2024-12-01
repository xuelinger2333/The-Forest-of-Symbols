using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ͳ�������������Ϸ���̵��������ݣ��������ݵ�ִֵ���ض�����
/// </summary>

public class PlayerSpecialStatisticCounter : MonoBehaviour
{
    private static readonly object lockObject = new object(); //��
    //���stepCount��ʱ��Player��moveState����������
    private List<int> Step = new List<int>();//ȷ���̰߳�ȫ��һ��Э�̿���һ��step����

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
        //��timeLimit����ͳ����ҵĲ����Ƿ�ﵽ��targetValue��isLoopCount����Ƿ�Ӧ�ó���ͳ��
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
