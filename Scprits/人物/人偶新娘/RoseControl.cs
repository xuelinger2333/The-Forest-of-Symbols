using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoseControl : MonoBehaviour
{
    //public DollBride player;
    //private SpriteRenderer sr;
    //public float fadeInTime = 0.5f; 
    //public float fadeOutTime = 0.5f;
    //private float currentAlpha;
    //private bool canSaw = false;
    //private float fadeTimer = 0.0f;

    //private void Awake()
    //{
    //    sr = GetComponent<SpriteRenderer>();
    //}

    //private void Update()
    //{
    //    if (!canSaw && player.action.DBPassiveAction.isRoseAppear)
    //    {
    //        fadeTimer += Time.deltaTime;
    //        currentAlpha = Mathf.Lerp(0.0f, 1.0f, fadeTimer / fadeInTime);
    //        SetAlpha(currentAlpha);

    //        if (fadeTimer >= fadeInTime)
    //        {
    //            canSaw = true;
    //            fadeTimer = 0.0f; 
    //        }
    //    }
    //    else if (canSaw && !player.action.DBPassiveAction.isRoseAppear)
    //    {
    //        fadeTimer += Time.deltaTime;
    //        currentAlpha = Mathf.Lerp(1.0f, 0.0f, fadeTimer / fadeOutTime);
    //        SetAlpha(currentAlpha);

    //        if (fadeTimer >= fadeOutTime)
    //        {
    //            canSaw = false;
    //            fadeTimer = 0.0f;
    //        }
    //    }
    //}

    //void SetAlpha(float alpha)
    //{
    //    Color newColor = sr.color;
    //    newColor.a = alpha;
    //    sr.color = newColor;
    //}
}
