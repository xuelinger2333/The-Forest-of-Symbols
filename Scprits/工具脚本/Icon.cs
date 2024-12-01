using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour
{
    private float time;
    private float timer = 1000f;
    private Image shadow;
    private Image image;
    public IconType type = default;
    public bool playerID;

    public void Set(int _index,bool _playerID,float _time, IconType _type)
    {
        playerID = _playerID;
        type = _type;
        time = _time;
        ResetTimer(time);
        image.sprite = IconManager.instance.icons[(int)type].GetClip(_index);
        if (time > 1000)
        {
            shadow.fillAmount = 0;
        }
    }
    private void Awake()
    {
        shadow = transform.Find("Shadow").GetComponent<Image>();
        image = transform.Find("Image").GetComponent<Image>();
    }

    private void Update()
    {
        if (timer < 1000)
        {
            timer -= Time.deltaTime;
            shadow.fillAmount = timer / time;
        }
        if(timer < 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        IconManager.instance.AddToPlayingList(gameObject, type, playerID);
    }
    public void ResetTimer(float lostTime)
    {
        time = lostTime;
        timer = lostTime;
    }
}
