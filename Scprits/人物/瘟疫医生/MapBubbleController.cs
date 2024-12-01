using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapBubbleController : FlowerControl {
    public GameObject child1;
    public GameObject child2;
    // Start is called before the first frame update
   public override void Awake()
    {
        base.Awake();
        if (child1 != null && child2 != null)
        ani.SetBool("Start", true);
    }
    public void ExitCurrentAni()
    {
        if (child1 != null) child1.SetActive(child1);
        if (child2 != null) child1.SetActive(child2);
        ani.SetBool("Exit", true);
        ani.SetBool("Start", false);
        int RandomInt = Random.Range(1, 7);
        ani.SetInteger("SelectAnimation", RandomInt);
    }
}
