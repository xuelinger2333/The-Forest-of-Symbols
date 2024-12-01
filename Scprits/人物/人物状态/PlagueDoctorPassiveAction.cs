using FishNet.Demo.AdditiveScenes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlagueDoctorPassiveAction : MonoBehaviour
{
    [SerializeField] PlayerViewManager viewManager;
    public void Enter(Vector2 position)
    {
        SoundManager.PlaySound(SoundType.PDPassiveSkill, 0.3f);
        List<GameObject> g = viewManager.GetGameObject();
        for (int i = 0; i < g.Count; i++)
        {
            GameObject instance = TriggerManager.ExecuteTrigger("瘟疫医生被动", g[i]);
            instance.GetComponent<TransfromSetTrigger>().SetTransformPosition(position, Convert.ToBoolean(i), true);
        }
    }
}
