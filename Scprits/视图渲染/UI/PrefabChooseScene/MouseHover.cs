using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseHover : MonoBehaviour
{
    bool isClick = false;
    public Toggle toggle;
    private void OnMouseEnter()
    {
        if (!toggle.isOn)
            transform.DORotate(new Vector3(0, 0, -10), 0.1f);
    }
    private void OnMouseExit()
    {
        if (!toggle.isOn)
            transform.DORotate(new Vector3(0, 0, 0), 0.1f);
    }
}
