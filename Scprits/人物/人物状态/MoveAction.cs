using FishNet.Demo.AdditiveScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    Player player;
    Vector2 targetPos;
    float speed;
    private void Awake()
    {
        player = null;
        enabled = false;
    }

    public void Enter(Player p, Vector2 target, float v)
    {
        if (player != null && 
            enabled == true &&
            player.pos_update != targetPos) //上一次移动还没有移动完毕，则瞬移至上一次移动终点，防止影响下一次移动
        {
            player.pos_update = targetPos;
        }
        targetPos = target;
        speed = v;
        player = p;
        enabled = true;
    }

    void Update()
    {
        if (player.pos_update == targetPos)
        {
            enabled = false;
            return;
        }
        else
        {
            float step = speed * Time.deltaTime;
            player.pos_update = Vector3.MoveTowards(player.pos_update, targetPos, step);
        }
    }
}
