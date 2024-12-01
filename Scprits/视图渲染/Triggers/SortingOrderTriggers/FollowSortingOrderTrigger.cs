using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSortingOrderTrigger : MonoBehaviour
{
    public GameObject myFollowTarget;
    public SpriteRenderer spriteRenderer;
    public int offset;
    public void OnTriggerStart() { }
    void Update()
    {
        spriteRenderer.sortingOrder = myFollowTarget.GetComponent<SpriteRenderer>().sortingOrder + offset;
    }
}
