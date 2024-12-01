using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTarget;
    public float smoothing;
    bool firstUpdate = false;

    void LateUpdate()
    {
        if (Time.timeScale == 0) return;
        if (followTarget != null)
        {
            if (Mathf.Abs(transform.position.x - followTarget.transform.position.x) > 10 ||
                Mathf.Abs(transform.position.y - followTarget.transform.position.y) > 10)
            {
                transform.position = new Vector3(followTarget.position.x, followTarget.position.y + 0.7f, transform.position.z);
                firstUpdate = false;
                return;
            }
            if (transform.position.x != followTarget.position.x || transform.position.y != followTarget.position.y)
            {
                Vector3 targetPos;
                targetPos.x = followTarget.position.x;
                targetPos.y = followTarget.position.y+0.7f;
                targetPos.z = transform.position.z;

                transform.position = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
            }
        }
    }
}
