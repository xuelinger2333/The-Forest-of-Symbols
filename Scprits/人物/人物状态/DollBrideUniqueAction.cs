using FishNet.Demo.AdditiveScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollBrideUniqueAction : MonoBehaviour
{
    [SerializeField] Timer Timer_UniqueTimer;
    [SerializeField] PlayerViewManager viewManager;
    [SerializeField] Material DBUniqueMaterial;
    
    private void Awake()
    {
        enabled = false;
    }
    public void Enter(in float time,in bool playerID, in Camera myCamera)//传的参数不会进行修改
    {
        IconManager.instance.PlayIcon(time, IconType.人偶绝技, playerID);
        GameObject instance = Instantiate(PrefabControl.Instance.flowerPrefab);

        instance.GetComponent<FlowerControl>().ani.SetBool("4s", time == 4f);
        instance.transform.SetParent(myCamera.transform, false);
        instance.transform.localPosition = new Vector3(0, 0, 5);
        SoundManager.PlaySound(SoundType.DBUniqueSkill, 1f);

        viewManager.ChangeMaterial(DBUniqueMaterial, false);


        Timer_UniqueTimer.targetTime = time;
        Timer_UniqueTimer.StartTimer(TimeOut);
    }
    void TimeOut()
    {

        viewManager.ResetMaterial();
    }

    public void Exit(){}
}
