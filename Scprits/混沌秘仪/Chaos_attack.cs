using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaos_attack : MonoBehaviour, Chaos
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
        s0.SetPlayerAccumulateSpeed(s0.currentAccumulateSpeed + 5);
        s0.SetPlayerAttackDelaySpeed(s0.currentAttackDelaySpeed + 5);

        s1.SetPlayerAccumulateSpeed(s1.currentAccumulateSpeed + 5);
        s1.SetPlayerAttackDelaySpeed(s1.currentAttackDelaySpeed + 5);
        ChaosText.SetActive(true);
    }

    void StopChaos()
    {
        s0.SetPlayerAccumulateSpeed(s0.currentAccumulateSpeed - 5);
        s0.SetPlayerAttackDelaySpeed(s0.currentAttackDelaySpeed - 5);

        s1.SetPlayerAccumulateSpeed(s1.currentAccumulateSpeed - 5);
        s1.SetPlayerAttackDelaySpeed(s1.currentAttackDelaySpeed - 5);
        ChaosText.SetActive(false);
    }
}
