using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : Creature
{
    public Crow()
    {
        handler = CrowHandler.Instance;
        
    }
    public override void Initialize(Vector2 initialPos, int master = -1)
    {
        base.Initialize(initialPos, master);
        SetAnimationBool("Idle", true);
        SetAnimationBool("Trigger", false);
    }
    public override void Die()
    {
        base.Die();
        if (isTriggered)
        {
            SetAnimationBool("Idle", false);
            SetAnimationBool("Trigger", true);
        }
        else
        {
            view1.DestroyMe();
            view2.DestroyMe();
        }
    }
    public override void Update()
    {
        if (GameManager.Instance.FishNet_isUsingNetwork &&
            !handler.PlayerDictionary[0].NETWORK_isMainController)
        {
            return;
        }
        if (isTriggered) return;
        base.Update();
        for (int i = 0; i < handler.PlayerDictionary.Count; i++)
        {
            if (i == masterId) continue;
            if (Mathf.Abs(handler.PlayerDictionary[i].pos_x - pos_x) < 2 && Mathf.Abs(handler.PlayerDictionary[i].pos_y - pos_y) < 2)
            {
                Debug.Log("触发乌鸦！");
                SetAnimationBool("Idle", false);
                SetAnimationBool("Trigger", true);
                TriggerEffect(i);
                isTriggered = true;
                break;
            }
        }
    }
}
