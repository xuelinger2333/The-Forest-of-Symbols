using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstantsValue 
{
    //�������趨�ٶȳ�������񳤶����ޣ������������
    static private List<float> speed = new List<float> { 
        0.033f,
        0.083f,
        0.133f,
        0.183f,
        0.233f,
        0.283f,
        0.333f,
        0.383f,
        0.433f,
        0.483f   };
    static public float getMoveTimeFromSpeed(int level)
    {
        if (level < 0 || level > 9) throw new Exception("�ٶȵȼ�����");
        return speed[level];
    }

}
