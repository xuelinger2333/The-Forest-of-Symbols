using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnimationTrigger : MonoBehaviour
{
    MapGenerator map;
    Animator ani;
    SpriteRenderer sr;
    public SpriteRenderer child_sr;
    // Start is called before the first frame update
    void Awake()
    {
        map = MapGenerator.Instance;
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetSpriteOrder(int n)
    {
        sr.sortingOrder = n;
        child_sr.sortingOrder = n;
    }
    public void PlayAnimation(string aniBool)
    {
        ani.SetBool(aniBool, true);
    }
    void NotifyMapGenerator()
    {

    }
    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
