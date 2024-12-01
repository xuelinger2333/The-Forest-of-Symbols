using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerControl : MonoBehaviour
{
    public Animator ani;
    public SpriteRenderer sr;
    public virtual void Awake()
    {
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public virtual void DestroyMe()
    {
        Destroy(gameObject);
    }

}
