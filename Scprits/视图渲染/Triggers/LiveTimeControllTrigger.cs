using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveTimeControllTrigger : MonoBehaviour
{
    //为特效提供控制生存时长服务
    bool myEnd = false;
    float myTimeElapsed = 0;
    public float myLiveTime;
    public void OnTriggerStart()
    {

    }
    void Update()
    {
        if (myEnd) return;
        myTimeElapsed += Time.deltaTime;
        if (myTimeElapsed >= myLiveTime)
        {
            myEnd = true;
            GetComponent<TriggerInfo>().EndAndDestroy();
            return;
        }

    }
}
