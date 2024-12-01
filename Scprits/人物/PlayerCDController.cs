using FishNet.Demo.AdditiveScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCDController : MonoBehaviour
{
    Character player;//容器类
    private void Start()
    {
        player = GetComponent<Character>();
    }
    private void Update()
    {
        player.uniqueSkillAddCountTimer += Time.deltaTime;
        if (player.uniqueSkillAddCountTimer >= player.uniqueSkillAddCountCD)
        {
            if (player.uniqueSkillCount < player.uniqueSkillLimit)
                player.uniqueSkillCount += (int)(player.uniqueSkillAddCountUnit * player.uniqueSkillUnit_mutiply);
            player.uniqueSkillAddCountTimer = 0;
        }
        if (player.activeSkillTimer >= 0) player.activeSkillTimer -= Time.deltaTime;
    }
    public void setActiveSkillTimer(float t) {
        player.activeSkillTimer = t; 
        player.activeSkillTimer = Mathf.Clamp(player.activeSkillTimer, 0, player.activeSkillCD);
    }
    public void AddUniqueSkillCount(int unit) {
        player.uniqueSkillCount += (int)(unit * player.uniqueSkillUnit_mutiply); 
        player.uniqueSkillCount = Mathf.Clamp(player.uniqueSkillCount, 0, player.uniqueSkillLimit); 
    }
}
