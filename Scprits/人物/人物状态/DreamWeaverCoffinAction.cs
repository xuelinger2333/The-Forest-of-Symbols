using FishNet.Demo.AdditiveScenes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class DreamWeaverCoffinAction : MonoBehaviour
{
    [SerializeField] PlayerViewManager viewManager;
    GameObject coffin0, coffin1;

    public void Enter(Vector2 position, bool flipX)
    {
        //创建棺椁特效
        List<GameObject> g = viewManager.GetGameObject();
        if (coffin0 == null)
            coffin0 = TriggerManager.ExecuteTrigger("织梦魔女棺椁", g[0]);
        if (coffin1 == null)
            coffin1 = TriggerManager.ExecuteTrigger("织梦魔女棺椁", g[1]);
        coffin0.GetComponent<TransfromSetTrigger>().SetTransformPosition(position, Convert.ToBoolean(0), true);
        coffin0.GetComponent<SpriteRenderer>().flipX = flipX;
        coffin1.GetComponent<TransfromSetTrigger>().SetTransformPosition(position, Convert.ToBoolean(1), true);
        coffin1.GetComponent<SpriteRenderer>().flipX = flipX;
    }

    public void Exit()
    {
        //销毁棺椁特效
        if (coffin0)
            coffin0.GetComponent<TriggerInfo>().EndAndDestroy();
        if (coffin1)
            coffin1.GetComponent<TriggerInfo>().EndAndDestroy();
        coffin0 = null;
        coffin1 = null;
    }
}
