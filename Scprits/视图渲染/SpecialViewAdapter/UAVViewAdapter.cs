using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAVViewAdapter : CreatureViewAdapter
{
    public GameObject uav;
    public GameObject RangeLight;
    Vector3 lightPosition;
    int category = -1;
    List<Vector2> offset = new List<Vector2> { new Vector2(0, 0), new Vector2(0, 0.2f), new Vector2(0.5f, 0.025f), new Vector2(0, -0.2f),  new Vector2(-0.5f, 0.025f) };
    protected override void Awake()
    {
        handler = transform.parent.GetComponent<CreatureHandler>();
        ani = GetComponent<Animator>();
        sr = uav.GetComponent<SpriteRenderer>();
    }
    public override void FixedUpdate()
    {
        if (!handler.QueryCreatureIdValid(creatureId))
        {
            //Destroy(this.transform.gameObject);
            return;
        }
        int rotate = (isInMap2 == false) ? rotationCounts1 : rotationCounts2;
        if (category == -1)
        {
            category = ((UavHandler)handler).QueryUAVCategoryById(creatureId);
            category = (category + rotate - 1 + 8) % 4 + 1;
            positionOffset = new Vector2(0.5f, 2.7f) + offset[category];
        }
        Vector2 pos_update = handler.QueryCreatureUpdatePositionbyId(creatureId);
        Vector2 intPosition = handler.QueryCreaturePositionbyId(creatureId);
        

        Vector2 RotatedPosition = UtilFunction.ComputePosRotationforViewAdapter(isInMap2, rotate, pos_update, mapWidth, positionOffset, mapSpace);
        PlayerPosition = new Vector3(RotatedPosition.x, RotatedPosition.y, 0);
        RotatedPosition = UtilFunction.ComputePosRotationforViewAdapter(isInMap2, rotate, intPosition, mapWidth, new Vector2(0.5f, 2.7f), mapSpace);
        lightPosition = new Vector3(RotatedPosition.x, RotatedPosition.y, 0);

        lightPosition.y -= 1.45f;
        if (uav.transform.position != PlayerPosition)
        {
            uav.transform.position = PlayerPosition;
        }
        if (RangeLight.transform.position != lightPosition)
        {
            RangeLight.transform.position = lightPosition;
        }
    }
}
