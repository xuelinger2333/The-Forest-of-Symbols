using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    //为特效提供播放启动动画服务
    public string myStateName;
    public string myParamName;
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
        myAnimator.CrossFade(myStateName, 0f);
        if (!string.IsNullOrEmpty(myParamName))
        {
            myAnimator.SetBool(myParamName, true);
        }
    }
}
