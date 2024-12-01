using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHover_blood : MonoBehaviour
{
    private void OnMouseEnter()
    {
        transform.DOScale(1.1f, 0.1f);
    }
    private void OnMouseExit()
    {
        transform.DOScale(1f, 0.1f);
    }
}
