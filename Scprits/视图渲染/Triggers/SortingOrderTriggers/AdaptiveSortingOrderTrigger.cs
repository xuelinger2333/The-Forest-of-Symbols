using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveSortingOrderTrigger : MonoBehaviour
{
    //为特效提供自动计算填写种类的sorting order服务
    //直接提取位置的y计算，无法识别超过1行的offset
    public SpriteRenderer mySpriteRenderer;
    public string SOtype;
    int transform_y;
    void Awake()
    {
        transform_y = (int)transform.position.y;
        mySpriteRenderer.sortingOrder = TileOrderComputer.TileOrder(0, (int)transform.position.y, SOtype);
    }

    // Update is called once per frame
    void Update()
    {
        if ((int) transform.position.y != transform_y)
        {
            transform_y = (int)transform.position.y;
            mySpriteRenderer.sortingOrder = TileOrderComputer.TileOrder(0, (int)transform.position.y, SOtype);
        }
    }
}
