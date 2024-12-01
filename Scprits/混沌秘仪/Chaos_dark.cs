using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaos_dark : MonoBehaviour, Chaos
{
    [SerializeField] ChaosOracle chaosOracle;
    [SerializeField] GameObject ChaosText;
    [SerializeField] Timer Timer_chaosLong;
    Character c0, c1;
    void Start()
    {
        if (chaosOracle)
            chaosOracle.AddChaos(this);
        c0 = GameManager.Instance.player0 as Character;
        c1 = GameManager.Instance.player1 as Character;
    }
    public void StartChaos()
    {
        Timer_chaosLong.targetTime = 8;
        Timer_chaosLong.StartTimer(StopChaos);
        ChaosText.SetActive(true);
        c0.StartCameraCover();
        c1.StartCameraCover();
    }

    void StopChaos()
    {
        c0.EndCameraCover();
        c1.EndCameraCover();
        ChaosText.SetActive(false);
    }
}