using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnimationTrigger : MonoBehaviour
{
    //为特效提供播放销毁动画服务
    public string destroyStateName;
    public string destroyParamName;
    public Animator myAnimator;
    private void Awake()
    {
        if (!TryGetComponent(out myAnimator))
        {
            myAnimator = GetComponentInChildren<Animator>();
        }
    }
    public void OnTriggerStart()
    {

    }
    public void EndAndDestroy()
    {
        myAnimator.CrossFade(destroyStateName, 0f);
        if (!string.IsNullOrEmpty(destroyParamName))
        {
            myAnimator.SetBool(destroyParamName, true);
        }
    }
}
