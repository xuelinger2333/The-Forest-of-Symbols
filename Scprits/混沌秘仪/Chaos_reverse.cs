using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaos_reverse : MonoBehaviour, Chaos
{
    [SerializeField] ChaosOracle chaosOracle;
    [SerializeField] GameObject ChaosText;
    [SerializeField] Camera c0, c1;
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
        ChaosText.SetActive(true);

        c0.transform.DORotate(new Vector3(0, 0, 180), 0.5f);
        c1.transform.DORotate(new Vector3(0, 0, 180), 0.5f);
    }

    void StopChaos()
    {
        ChaosText.SetActive(false);
        c0.transform.DORotate(new Vector3(0, 0, 0), 0.5f);
        c1.transform.DORotate(new Vector3(0, 0, 0), 0.5f);
    }

}
