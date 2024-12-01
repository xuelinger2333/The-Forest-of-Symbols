using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitionControl : FlowerControl
{
    float moveTime = 0.25f;
    List<Vector2> dir = new List<Vector2> { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };
    public override void Awake()
    {
        base.Awake();
        //随机抽一个数播放动画
        int RandomInt = Random.Range(0, 5);
        int ColorRandomInt = Random.Range(0, 3);
        ani.SetInteger("ColorID", ColorRandomInt);
        ani.SetInteger("ID", RandomInt);
        //随机翻转图像
        float flipX = Random.Range(0f, 1f);
        if (flipX > 0.5f) gameObject.GetComponent<SpriteRenderer>().flipX = true;
    }
    public override void DestroyMe()
    {
        base.DestroyMe();
    }
    public void MoveForward()
    {
        int idx = Random.Range(0, dir.Count);
        Vector3 targetDir = dir[idx];
        Vector3 target = transform.position + targetDir;
        gameObject.transform.DOMove(target, moveTime);
    }
}
