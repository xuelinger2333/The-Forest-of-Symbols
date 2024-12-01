using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransfromSetTrigger : MonoBehaviour
{
    public Vector3 offset;
    //为特效提供根据逻辑位置，设置实际位置的服务
    //功能不完善，需要测试
    public void SetTransformPosition(Vector2 position, bool mapId, bool rotateInMap)
    {
        int rotate = (!mapId) ? MapGenerator.Instance.rotationCounts1 : MapGenerator.Instance.rotationCounts2;
        int space = MapGenerator.Instance.space;
        int width = MapGenerator.Instance.width;
        if (!rotateInMap) rotate = 0;
        Vector2 computePosition = UtilFunction.ComputePosRotationforViewAdapter(mapId, rotate, position, width, Vector2.zero, space);
        transform.position = new Vector3(computePosition.x, computePosition.y, 0);
        transform.localPosition += offset;
    }
}
