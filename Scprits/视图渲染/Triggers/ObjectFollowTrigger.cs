using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowTrigger : MonoBehaviour
{
    //为特效提供设置并跟随父物体服务
    //仅设置位置,不设置sorting order
    public GameObject myFollowTarget;
    public Vector3 myOffset;

    public void OnTriggerStart()
    {
        if (myFollowTarget != null)
        {
            transform.position = transform.position + myOffset;
            transform.SetParent(myFollowTarget.transform, false);
            
        }

    }
    void LateUpdate()
    {
        
    }
}
