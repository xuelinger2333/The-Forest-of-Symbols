using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureViewAdapter : MonoBehaviour
{
    public bool isInMap2;
    public bool isShownAfterGrass;
    public CreatureHandler handler;
    protected Vector3 PlayerPosition;
    protected int mapWidth = 0;
    protected int mapSpace = 0;
    public int creatureId = 0;
    protected int faceD = 1;
    protected int rotationCounts1;
    protected int rotationCounts2;
    public Vector2 positionOffset = new Vector2(0.5f, 0.5f);
    public Animator ani { get; protected set; }
    public SpriteRenderer sr { get; protected set; }
    protected virtual void Awake()
    {
        handler = transform.parent.GetComponent<CreatureHandler>();
        ani = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    protected virtual void Start()
    {
        mapWidth = handler.map.width;
        mapSpace = handler.map.space;
        KeyValuePair<int, int> rotationCounts = handler.QueryMapRotationCounts();
        rotationCounts1 = rotationCounts.Key;
        rotationCounts2 = rotationCounts.Value;
    }
    public void UpdateAni(string aniName)
    {
        bool val = handler.QueryCreatureAnimationById(creatureId, aniName);
        //ani = GetComponent<Animator>();
        
        ani.SetBool(aniName, val);
        
    }
    protected void CheckFilp(int rotationCounts1, int rotationCounts2)
    {
        int rotation;
        if (!isInMap2)
        {
            rotation = 4 - rotationCounts1;
        }
        else
        {
            rotation = 4 - rotationCounts2;
            
        }
        rotation = rotation % 4;
        Vector2 faceDir = handler.QueryCreatureFaceDir(creatureId);
        
        Vector2 directionInAnotherMap = UtilFunction.ComputeDirectionRotateforPlayer(faceDir, rotation);
        if (faceD != (int)directionInAnotherMap.x && (int)directionInAnotherMap.x != 0)
        {
            transform.Rotate(0, 180, 0);
            faceD = (int)directionInAnotherMap.x;
        }

    }
    public void DestroyMe()
    {
        Destroy(gameObject);
    }
    public virtual void FixedUpdate()
    {
        if (!handler.QueryCreatureIdValid(creatureId))
        {
            //Destroy(this.transform.gameObject);
            return;
        }
        Vector2 pos_update = handler.QueryCreatureUpdatePositionbyId(creatureId);

        if (!isInMap2)
        {
            Vector2 RotatedPosition = UtilFunction.ComputePosRotationforViewAdapter(isInMap2, rotationCounts1, pos_update, mapWidth, positionOffset, mapSpace);
            PlayerPosition = new Vector3(RotatedPosition.x, RotatedPosition.y, 0);
        }
        else
        {
            Vector2 RotatedPosition = UtilFunction.ComputePosRotationforViewAdapter(isInMap2, rotationCounts2, pos_update, mapWidth, positionOffset, mapSpace);
            PlayerPosition = new Vector3(RotatedPosition.x, RotatedPosition.y, 0);
        }
        if (transform.position != PlayerPosition)
        {
            transform.position = PlayerPosition;
        }
        if (!isShownAfterGrass)
            sr.sortingOrder = TileOrderComputer.TileOrder((int)PlayerPosition.x, (int)PlayerPosition.y, "player");
        else
            sr.sortingOrder = TileOrderComputer.TileOrder((int)PlayerPosition.x, (int)PlayerPosition.y, "creature");
        CheckFilp(rotationCounts1, rotationCounts2);
    }
    public void setSpriteRendererSortingOrder(int v)
    {
        sr.sortingOrder = v;
    }
}
