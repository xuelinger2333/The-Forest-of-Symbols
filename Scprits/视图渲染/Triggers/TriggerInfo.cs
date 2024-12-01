using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInfo : MonoBehaviour
{
    //特效控制器
    public string myName;
    AnimationTrigger animationTrigger;
    AttachObjectTrigger attachObjectTrigger;
    LiveTimeControllTrigger liveControllTrigger;
    ObjectFollowTrigger objectFollowTrigger;
    FollowSortingOrderTrigger FollowSortingOrderTrigger;
    DestroyAnimationTrigger destroyAnimationTrigger;

    public void StartTrigger(GameObject Target)
    {        
        if (TryGetComponent(out attachObjectTrigger))
        {

            attachObjectTrigger.OnTriggerStart();
        }
        if (TryGetComponent(out animationTrigger))
        {
            animationTrigger.OnTriggerStart();
        }
        if (TryGetComponent(out liveControllTrigger))
        {
            liveControllTrigger.OnTriggerStart();
        }
        if (TryGetComponent(out objectFollowTrigger))
        {            
            if (Target != null)
            {
                objectFollowTrigger.myFollowTarget = Target;
            }
            objectFollowTrigger.OnTriggerStart();
        }
        if (TryGetComponent(out FollowSortingOrderTrigger))
        {
            if (Target != null)
            {
                FollowSortingOrderTrigger.myFollowTarget = Target;
            }
            FollowSortingOrderTrigger.OnTriggerStart();
        }
        if (TryGetComponent(out destroyAnimationTrigger))
        {
            destroyAnimationTrigger.OnTriggerStart();
        }
    }

    public void EndAndDestroy()
    {
        if (destroyAnimationTrigger)
        {
            destroyAnimationTrigger.EndAndDestroy();
        }
        else Destroy(gameObject);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
