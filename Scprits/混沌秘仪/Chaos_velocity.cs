using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaos_velocity : MonoBehaviour, Chaos
{
    [SerializeField] ChaosOracle chaosOracle;
    [SerializeField] GameObject ChaosText;
    [SerializeField] PlayerStats s0, s1;
    [SerializeField] Timer Timer_chaosLong;
    void Start()
    {
        if (chaosOracle)
            chaosOracle.AddChaos(this);
    }
    public void StartChaos()
    {
        Timer_chaosLong.targetTime = 8;
        Timer_chaosLong.StartTimer(StopChaos);
        s0.SetPlayerMoveSpeed(s0.currentMoveSpeed + 2);
        s0.SetPlayerAttackSpeed(s0.currentAttackSpeed + 2);

        s1.SetPlayerMoveSpeed(s1.currentMoveSpeed + 2);
        s1.SetPlayerAttackSpeed(s1.currentAttackSpeed + 2);
        ChaosText.SetActive(true);
    }

    void StopChaos()
    {
        s0.SetPlayerMoveSpeed(s0.currentMoveSpeed - 2);
        s0.SetPlayerAttackSpeed(s0.currentAttackSpeed - 2);

        s1.SetPlayerMoveSpeed(s1.currentMoveSpeed - 2);
        s1.SetPlayerAttackSpeed(s1.currentAttackSpeed - 2);
        ChaosText.SetActive(false);
    }

}
