using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBuff : MonoBehaviour
{
    Player player;
    public bool isEnterEnemyScreen = false;
    public bool isModelExposure_3 = false;
    public bool isSkillExposure = false;
    void Start()
    {
        player = GetComponent<Player>();
        //初始条件下，加3移速
        player.stats.SetPlayerMoveSpeed(player.stats.currentMoveSpeed + 3);
    }
    public void EnterEnemyScreen()
    {
        isEnterEnemyScreen = true;
        player.stats.SetPlayerMoveSpeed(player.stats.currentMoveSpeed -1);
    }

    public void ModelExposure_3()
    {
        isModelExposure_3 = true;
        player.stats.SetPlayerMoveSpeed(player.stats.currentMoveSpeed - 1);
    }

    public void SkillExposure()
    {
        isSkillExposure = true;
        player.stats.SetPlayerMoveSpeed(player.stats.currentMoveSpeed - 1);
    }
}
