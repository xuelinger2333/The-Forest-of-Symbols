using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Creature
{
    public Vector2 pos_update;
    public int pos_x; 
    public int pos_y;
    public int masterId = -1;
    public int creatureId;
    public int triggerTarget = -1;
    protected CreatureHandler handler;
    public bool isTriggered = false;
    public CreatureViewAdapter view1;
    public CreatureViewAdapter view2;
    public Vector2 faceDir;
    public Dictionary<String, bool> aniBoolDictionary { get; private set; }
    public Creature(){}
    public virtual void Initialize(Vector2 initialPos, int master = -1) 
    {
        aniBoolDictionary = new Dictionary<string, bool>();
        pos_x = (int)initialPos.x;
        pos_y = (int)initialPos.y;
        pos_update.x = pos_x;
        pos_update.y = pos_y;
        masterId = master;
        return;
    }
    public void SetAnimationBool(string animation, bool val)
    {
        aniBoolDictionary[animation] = val;
        if (view1 != null)
            view1.UpdateAni(animation);
        
        if (view2 != null)
            view2.UpdateAni(animation);
    }

    protected virtual void TriggerEffect(int targetPlayer)
    {
        triggerTarget = targetPlayer;
        handler.HandleCreatureTrigger(creatureId);
    }
    public virtual void Die(){}

    public virtual void Update()
    {}
}
