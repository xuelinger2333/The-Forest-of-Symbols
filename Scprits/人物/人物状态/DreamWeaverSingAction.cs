using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamWeaverSingAction : MonoBehaviour
{
    [SerializeField] PlayerViewManager viewManager;
    GameObject prefab;
    public void Enter(float time)
    {
        List<GameObject> g = viewManager.GetGameObject();
        for (int i = 0; i < g.Count; i++)
        {
            prefab = TriggerManager.ExecuteTrigger("织梦魔女吟唱", g[i]);
            prefab.GetComponent<LiveTimeControllTrigger>().myLiveTime = time;
        }

    }
}
