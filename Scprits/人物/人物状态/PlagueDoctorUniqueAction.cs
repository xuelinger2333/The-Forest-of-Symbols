using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlagueDoctorUniqueAction : MonoBehaviour
{
    [SerializeField] PlayerViewManager viewManager;
    public void Enter()
    {
        List<GameObject> g = viewManager.GetGameObject();
        for (int i = 0; i < g.Count; i++)
        {
            TriggerManager.ExecuteTrigger("瘟疫医生绝招", g[i]);
            TriggerManager.ExecuteTrigger("瘟疫医生被动", g[i]);
        }
    }

    public void Exit() { }
}
