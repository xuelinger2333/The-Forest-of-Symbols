using FishNet.Demo.AdditiveScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAction : MonoBehaviour
{
    GameObject Damp, Damp_map2;
    [SerializeField] PlayerViewManager viewManager;
    public void Enter()
    {
        List<GameObject> g = viewManager.GetGameObject();
        if (Damp == null)
            Damp = TriggerManager.ExecuteTrigger("潮湿", g[0]);
        if (Damp_map2 == null)
            Damp_map2 = TriggerManager.ExecuteTrigger("潮湿", g[1]);
    }
    public void Exit()
    {
        if (Damp != null)
            Damp.GetComponent<TriggerInfo>().EndAndDestroy();
        if (Damp_map2 != null)
            Damp_map2.GetComponent<TriggerInfo>().EndAndDestroy();
        Damp = null; Damp_map2 = null;
    }
}
