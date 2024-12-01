using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabControl : MonoBehaviour
{
    public static PrefabControl Instance { get; private set; }
    [SerializeField]
    private PlayerViewAdapter player0;
    [SerializeField]
    private PlayerViewAdapter player1;
    [SerializeField]
    private PlayerViewAdapter player0_map2;
    [SerializeField]
    private PlayerViewAdapter player1_map2;
    //map
    public GameObject SpecialGroundPrefab;
    //character
    public GameObject plagueMapPrefab;
    public GameObject plaguePrarent;
    public GameObject flowerPrefab;
    public GameObject pointerPrefeb;
    public GameObject visitionPrefab;
    public GameObject CreatureHintPrefab;
    public GameObject shadowPrefab; 
    public GameObject ChargeCompleteEffect;
    public GameObject ImageForestHintEffect;
    public GameObject coffinPrefab;
    public GameObject singPrefab;
    private void Awake()
    {
        // 确保只有一个实例
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    //为特殊地块专用的触发动画专门新做一个共调用方法
    public void createPrefabForSpecialGround(int x, int y, int space, int rotate_map1, int rotate_map2, int width, string type)
    {
        GameObject instance = Instantiate(SpecialGroundPrefab);
        Vector2 blockPosition1 = UtilFunction.ComputePosRotationforViewAdapter(false, rotate_map1, new Vector2(x, y), width, new Vector2(0.5f, 0.5f), space);
        instance.transform.position = new Vector3(blockPosition1.x, blockPosition1.y, 0);
        MapAnimationTrigger mat = instance.GetComponent<MapAnimationTrigger>();
        if (type != "水洼")
            mat.SetSpriteOrder(TileOrderComputer.TileOrder(0, (int)(blockPosition1.y - 0.5f), "specialTile"));
        else
            mat.SetSpriteOrder(TileOrderComputer.TileOrder(0, (int)(blockPosition1.y - 0.5f), "beforeTile"));
        mat.PlayAnimation(type);

        instance = Instantiate(SpecialGroundPrefab);
        Vector2 blockPosition2 = UtilFunction.ComputePosRotationforViewAdapter(true, rotate_map2, new Vector2(x, y), width, new Vector2(0.5f, 0.5f), space);
        instance.transform.position = new Vector3(blockPosition2.x, blockPosition2.y, 0);
        mat = instance.GetComponent<MapAnimationTrigger>();
        if (type != "水洼")
            mat.SetSpriteOrder(TileOrderComputer.TileOrder(0, (int)(blockPosition2.y - 0.5f), "specialTile"));
        else
            mat.SetSpriteOrder(TileOrderComputer.TileOrder(0, (int)(blockPosition2.y - 0.5f), "beforeTile"));
        mat.PlayAnimation(type);
    }
    //为父节点是PlayerViewAdapter,且预制体的行为不需要作太多变化的创建行为单独做一个创建函数
    //目的是缩减代码量，增加代码的易读性，减少代码的重复性
    public GameObject createPrefeb(GameObject prefeb, bool playerId, int mapID, Vector3 localPosition)
    {
        bool mapId = (mapID == 1) ? false : true;
        PlayerViewAdapter parent = null;
        if (playerId && mapId) parent = player1_map2;
        if (!playerId && mapId) parent = player0_map2;
        if (playerId && !mapId) parent = player1;
        if (!playerId && !mapId) parent = player0;
        GameObject instance = Instantiate(prefeb);
        instance.transform.position = localPosition;
        if (parent != null)
            instance.transform.SetParent(parent.transform, false);
        return instance;
    }
    public void DestroyGameObject(GameObject g)
    {
        Destroy(g);
    }
}
