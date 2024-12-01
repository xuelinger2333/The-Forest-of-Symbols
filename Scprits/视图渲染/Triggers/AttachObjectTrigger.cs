using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachObjectTrigger : MonoBehaviour
{
    //为特效提供增加预制体服务
    public GameObject myObjPrefab;

    GameObject myObject;

    public void OnTriggerStart()
    {
        myObject = Instantiate(myObjPrefab, transform.position, transform.rotation);
        myObject.transform.parent = transform;
    }
}
