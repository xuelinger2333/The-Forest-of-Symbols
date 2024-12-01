using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ExposurePointer : MonoBehaviour
{
    public PlayerViewAdapter main;
    public PlayerViewAdapter target;
    public float RectWidth = 2;
    public float RectHeight = 2;
    public float angle;
    public int edge;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = target.transform.position - main.transform.position;
        // 获取这个向量与正X轴之间的角度，结果将是0到180度或0到-180度
        angle = Vector2.SignedAngle(Vector2.up, direction);
        edge = (int)((angle + 45 + 360) / 90) % 4;
        
        transform.rotation =Quaternion.Euler(0, 0, angle);
        float x = 0, y = 0;
        if (direction.x == 0)
        {
            x = 0; y = (edge == 2) ? -RectHeight / 2: RectHeight / 2;
            transform.localPosition = new Vector3(x, y + 0.5f, 0);
            return;
        }
        if (direction.y == 0)
        {
            y = 0; x = (edge == 1) ? -RectHeight / 2 : RectHeight / 2;
            transform.localPosition = new Vector3(x, y + 0.5f, 0);
            return;
        }

        float tan = direction.y / direction.x;
       
        switch (edge)
        {
            case 0:
                y = RectHeight / 2; x = y / tan;
                break;
            case 1:
                x = -RectWidth / 2; y = x * tan;
                break;
            case 2:
                y = -RectHeight / 2; x = y / tan;
                break;
            case 3:
                x = RectWidth / 2; y = x * tan;
                break;
        } //Debug.Log(angle + " " + tan +" "+ edge + " " + x + " "+ y);
        transform.localPosition = new Vector3(x, y + 0.5f, 0);
    }
}
