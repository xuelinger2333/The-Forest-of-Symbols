using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaos_switch : MonoBehaviour, Chaos
{
    [SerializeField] ChaosOracle chaosOracle;
    [SerializeField] GameObject ChaosText;
    [SerializeField] Timer Timer_chaosLong;
    [SerializeField] InputMiddleWare input;
    void Start()
    {
        if (chaosOracle)
            chaosOracle.AddChaos(this);
    }
    public void StartChaos()
    {
        Timer_chaosLong.targetTime = 8;
        Timer_chaosLong.StartTimer(StopChaos);
        ChaosText.SetActive(true);

        input.SwitchInputControl();
    }

    void StopChaos()
    {
        ChaosText.SetActive(false);
        input.SwitchInputControl();
    }

}
