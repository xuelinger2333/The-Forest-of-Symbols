using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> PlayerPrefabList;

    static List<GameObject> PrefabList;
    private void Awake()
    {
        PrefabList = new List<GameObject>();
        for (int i = 0; i < PlayerPrefabList.Count; i++)
        {
            PrefabList.Add(PlayerPrefabList[i]);
        }
    }
    //添加特效trigger的入口函数
    public static GameObject ExecuteTrigger(string TriggerId, GameObject Target, bool startTrigger = true)
    {
        GameObject instance = null;
        for (int i = 0; i < PrefabList.Count; i++)
        {
            if (PrefabList[i].GetComponent<TriggerInfo>().myName == TriggerId)
            {
                instance = Instantiate(PrefabList[i]);
            }
        }
        if (startTrigger)
            instance.GetComponent<TriggerInfo>().StartTrigger(Target);
        return instance;
    }
}
