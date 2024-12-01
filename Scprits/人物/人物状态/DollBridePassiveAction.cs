using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollBridePassiveAction : MonoBehaviour
{
    [SerializeField] PlayerViewManager viewManager;
    GameObject rose0, rose1;
    private void Awake()
    {
        enabled = false;
    }
    public void Enter()//仅会修改isRoseAppear
    {
        //创建玫瑰特效
        List<GameObject> g = viewManager.GetGameObject();
        if (rose0 == null)
            rose0 = TriggerManager.ExecuteTrigger("人偶新娘被动", g[0]);
        if (rose1 == null)
            rose1 = TriggerManager.ExecuteTrigger("人偶新娘被动", g[1]);
    }
    public void Exit()
    {
        //销毁玫瑰特效
        if (rose0)
            rose0.GetComponent<TriggerInfo>().EndAndDestroy();
        if (rose1)
            rose1.GetComponent<TriggerInfo>().EndAndDestroy();
        rose0 = null;
        rose1 = null;
    }
}