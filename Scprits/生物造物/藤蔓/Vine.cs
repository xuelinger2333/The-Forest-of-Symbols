using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Vine : Creature
{
    public int historicalIndex{get; private set;}
    public Vine()
    {
        handler = VineHandler.Instance;
    }

    public void Initialize(Vector2 initialPos, int index, int master = -1)
    {
        base.Initialize(initialPos, master);
        historicalIndex = index;
    }

    public override void Update()
    {
        for (int i = 0; i < handler.PlayerDictionary.Count; i++)
        {
            if (i == masterId) continue;
            if (handler.PlayerDictionary[i].pos_x == pos_x && handler.PlayerDictionary[i].pos_y == pos_y)
            {
                Debug.Log("´¥·¢ÌÙÂû£¡");
                TriggerEffect(i);
                isTriggered = true;
            }
        }
    }
}
