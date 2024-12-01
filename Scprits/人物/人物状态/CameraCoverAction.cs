using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CameraCoverAction : MonoBehaviour
{
    [SerializeField] PlayableDirector FirstLevel_in;
    [SerializeField] ReversePlayableDirector FirstLevel_out;
    int CurrentLevel = 0;

    public void Enter(int level)
    {
        //重复效果，不执行
        if (level == CurrentLevel)
            return;

        if (level == 1)
        {
            //播放第一层遮挡的入场动画
            FirstLevel_in.Play();
            CurrentLevel = 1;
        }

        if (level == 0)
        {
            FirstLevel_out.Play();
            CurrentLevel = 0;
        }
    }
}
