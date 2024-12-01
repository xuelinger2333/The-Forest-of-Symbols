using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newIconCue", menuName = "IconSo")]
public class IconSO : ScriptableObject
{
    public Sprite[] _iconClipGroups = default;
    public Sprite GetClip(int index = 0)
    {
        return _iconClipGroups[index];
    }
}
