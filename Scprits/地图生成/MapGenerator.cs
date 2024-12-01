using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

abstract public class GroundState
{
    public MapGenerator map = MapGenerator.Instance;
    public abstract void applyEffectToPlayer(ref Player player, groundData ground, int x, int y);
    public abstract KeyValuePair<TileBase, TileBase> updateTile(ref List<groundData> groundDatas, groundData ground);
    public virtual void playerEnter(groundData ground, int x, int y){}
    public virtual void playerExit(groundData ground, int x, int y) { }
    public virtual void EndSick(groundData ground, int x, int y) 
    {
        ground.isPlagued = false;
    }
    public virtual void BeSick(groundData ground, int x, int y, Coroutine coroutine) 
    {
        if (ground.isPlagued) return;
        ground.isPlagued = true;
        if (coroutine == null)
            MapManager.Instance.startGroundBeSickCoroutine(map.SickTime, ground, x, y);
        else
            ground.wiredCoroutine = coroutine;
    }
    
}

abstract public class GroundBuilder
{
    public abstract void newData();
    public abstract void setData(ref List<groundData> groundDatas, int id);
    public abstract void setState();
    public abstract groundData getResult();
}
class BuilderDirector
{
    public void construct(GroundBuilder builder, ref List<groundData> data, int id)
    {
        builder.newData();
        builder.setData(ref data, id);
        builder.setState();
    }
}

[Serializable]
public class groundData
{
    public TileBase[] afterTile;
    public TileBase[] beforeTile;
    public String type;
    public int maxHP;
    
    [HideInInspector] public int hp;
    [HideInInspector] public bool isSpecial = false;
    [HideInInspector] public bool isObstacle = false;
    public int minNum;
    public int maxNum;
    [HideInInspector] public int type_id; //一级id，表示种类
    [HideInInspector] public int category_id; //二级id，表示种类中的tile细分
    [HideInInspector] public Coroutine wiredCoroutine;
    [HideInInspector] public Coroutine current_state = null;
    [HideInInspector] public bool isPlagued = false;
    private GroundState groundState;
    public void SetState(GroundState value)
    {
        groundState = value;
    }
    public void applyEffectToPlayer(ref Player player, int x, int y)
    {
        groundState.applyEffectToPlayer(ref player, this, x, y);
    }
    public void playerEnter(int x, int y)
    {
        groundState.playerEnter(this, x, y);
    }
    public void playerExit(int x, int y)
    {
        groundState.playerExit(this, x, y);
    }
    public KeyValuePair<TileBase, TileBase> updateTile(ref List<groundData> groundDatas)
    {
        return groundState.updateTile(ref groundDatas, this);
    }
    public void beSick(int x, int y, Coroutine coroutineOFLast)
    {
        groundState.BeSick(this, x, y, coroutineOFLast);
    }
    public void endSick(int x, int y)
    {
        groundState.EndSick(this, x, y);
    }
}

public class MapGenerator : MonoBehaviour, IsSickable
{
    public static MapGenerator Instance { get; private set; }
    public bool GrassAutoGrow = true;
    public int width;
    public int height;
    public int space;
    private Tilemap[] beforeTileMap;
    private Tilemap[] afterTileMap;
    private Tilemap[] beforeTileMap2;
    private Tilemap[] afterTileMap2;
    public int rotationCounts1;
    //旋转次数，1代表以地图数据为准顺时针旋转一次，以此类推
    public int rotationCounts2;
    public List<groundData> specialGroundDatas;
    public List<groundData> groundDatas;
    public int seed;
    public bool useRandomSeed;
    public groundData[,] mapData;

    public GameObject before, before_map2, after, after_map2;
    GameObject GoldenTree0, GoldenTree1;
    public float SickTime;
    public int ImageForestEdgeValue { get; private set; }

    private BuilderDirector builderDirector = new BuilderDirector();
    private GrassBuilder grassBuilder = new GrassBuilder();  
    private ThornBuilder thornBuilder = new ThornBuilder();
    private WaterBuilder waterBuilder = new WaterBuilder();
    private ObstacleBuilder obstacleBuilder = new ObstacleBuilder();
    private GoldenBranchBuilder goldenBranchBuilder = new GoldenBranchBuilder();
    private BushBuilder bushBuilder = new BushBuilder();
    private MushRoomBuilder mushroomBuilder = new MushRoomBuilder();
    private ImageForestBuilder imageForestBuilder = new ImageForestBuilder();
    private GoldenTreeBuilder goldenTreeBuilder = new GoldenTreeBuilder();
    public List<Vector2> posOfGrass = new List<Vector2>();
    Dictionary<string, int> mapTag;

    private void Awake()
    {
        rotationCounts2 = UnityEngine.Random.Range(1, 4);
        //rotationCounts2 = 1;//测试时使用1
        // 确保只有一个实例
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        TileOrderComputer.setRowNumber(height);
        TileOrderComputer.ItemRegist("beforeTile", 1);
        TileOrderComputer.ItemRegist("specialTile", 4);
        TileOrderComputer.ItemRegist("afterTile", 5);
        mapTag = new Dictionary<string, int>();

        //如果读取不到gameManager,说明读取不到游戏的config文件，不执行接下来的读取
        if (GameManager.Instance != null)
        {        
            BattleInfo mapInfo= ConfigReader.queryBattleInfo();
            string prefabMapTag = mapInfo.prefabMap;
            for (int i = 0; i < specialGroundDatas.Count; i++)
            {
                MapTag m = mapInfo.mapTag;
                var property = m.GetType().GetField(specialGroundDatas[i].type);
                if (property == null) continue;
                var tag_v = property.GetValue(m);
                mapTag.Add(specialGroundDatas[i].type, (int)tag_v);
                KeyValuePair<int, int> num_range = UtilFunction.computeByTag(specialGroundDatas[i].type, tag_v, prefabMapTag);
                specialGroundDatas[i].minNum = num_range.Key;
                specialGroundDatas[i].maxNum = num_range.Value;
            }
        }
        GenerateMap();
    }
    private void Start()
    {
        
        //如果读取不到gameManager,说明读取不到游戏的config文件，直接返回
        if (GameManager.Instance == null)
            return;
        int interval = 0;
        if (ConfigReader.queryBattleInfo().prefabMap == "")
            switch (mapTag["象征之森"])
            {
                case 1: interval = 25; break;
                case 2: interval = 20; break;
                case 3: interval = 10; break;
            }
        MapManager.Instance.StartImageForest(interval);
    }
    public void Refresh()
    {
        CleanMap();
        GenerateMapData();
        GenerateTileMap();
    }
    public void GenerateMap()
    {
        CleanObject();
        CreatObject();
        GenerateMapData();
        GenerateTileMap();
    }
    public void CreatObject()
    {
        beforeTileMap = new Tilemap[height];
        afterTileMap = new Tilemap[height];
        beforeTileMap2 = new Tilemap[height];
        afterTileMap2 = new Tilemap[height];
        for (int i = height - 1; i >= 0; i--)
        {
            if (before != null)
            {
                GameObject beforeInstance = Instantiate(before, this.transform);
                TilemapRenderer rendered = beforeInstance.GetComponent<TilemapRenderer>();
                rendered.sortingOrder = TileOrderComputer.TileOrder(0, i, "beforeTile");
                beforeTileMap[i] = beforeInstance.GetComponent<Tilemap>();
                beforeTileMap[i].ClearAllTiles();
            }
            if (before_map2 != null)
            {
                GameObject beforeInstance = Instantiate(before_map2, this.transform);
                TilemapRenderer rendered = beforeInstance.GetComponent<TilemapRenderer>();
                rendered.sortingOrder = TileOrderComputer.TileOrder(0, i, "beforeTile");
                beforeTileMap2[i] = beforeInstance.GetComponent<Tilemap>();
                beforeTileMap2[i].ClearAllTiles();
            }
            if (after != null)
            {
                GameObject beforeInstance = Instantiate(after, this.transform);
                TilemapRenderer rendered = beforeInstance.GetComponent<TilemapRenderer>();
                rendered.sortingOrder = TileOrderComputer.TileOrder(0, i, "afterTile");
                afterTileMap[i] = beforeInstance.GetComponent<Tilemap>();
                afterTileMap[i].ClearAllTiles();
            }
            if (after_map2 != null)
            {
                GameObject beforeInstance = Instantiate(after_map2, this.transform);
                TilemapRenderer rendered = beforeInstance.GetComponent<TilemapRenderer>();
                rendered.sortingOrder = TileOrderComputer.TileOrder(0, i, "afterTile");
                afterTileMap2[i] = beforeInstance.GetComponent<Tilemap>();
                afterTileMap2[i].ClearAllTiles();
            }
        }
    }
    private void GenerateMapData()   //生成地图数据
    {
        if (!useRandomSeed)
        {
            //seed = Time.time.GetHashCode();
            seed = (int)DateTime.Now.Ticks;
        }
        UnityEngine.Random.InitState(seed);

        mapData = new groundData[width , height];
        //普通地块
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                builderDirector.construct(grassBuilder, ref groundDatas, 0);
                mapData[x, y] = grassBuilder.getResult();
            }
        }
        //特殊地块
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                posOfGrass.Add(new Vector2(x, y));
            }
        }
        for (int i = 0; i < specialGroundDatas.Count; i++)
        {
            int randomInt = UnityEngine.Random.Range(specialGroundDatas[i].minNum, specialGroundDatas[i].maxNum + 1);
            for (int j = 0; j < randomInt; j++)
            {
                int index = UnityEngine.Random.Range(0, posOfGrass.Count);
                Vector2 pos = posOfGrass[index];
                SetSpecialGround(i, (int)pos.x, (int)pos.y);
                posOfGrass.RemoveAt(index);
            }
        }
        //设置黄金树
        SetSpecialGround(7, (int)width / 2, (int)height / 2);
        //设置黄金树特效
        if (mapData[width / 2, height / 2].type == "黄金树")
        {
            GoldenTree1 = TriggerManager.ExecuteTrigger("黄金树", null);
            GoldenTree1.GetComponent<TransfromSetTrigger>().SetTransformPosition(new Vector2(width / 2, height / 2), true, true);

            GoldenTree0 = TriggerManager.ExecuteTrigger("黄金树", null);
            GoldenTree0.GetComponent<TransfromSetTrigger>().SetTransformPosition(new Vector2(width / 2, height / 2), false, true);
        }
    }

    public void GenerateTileMap()
    {
        CleanMap();
        //地面
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                UpdateTile(x, y);
            }
        }
    }
    public void CleanObject()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform)
        {
            children.Add(child);
        }

        // 遍历临时列表并删除子对象
        foreach (Transform child in children)
        {
#if UNITY_EDITOR
            DestroyImmediate(child.gameObject);
#else
        Destroy(child.gameObject);
#endif
        }
    }
    public void CleanMap()
    {
        for (int  i = 0;  i < height;  i++)
        {
            beforeTileMap[i].ClearAllTiles();
            afterTileMap[i].ClearAllTiles();
            beforeTileMap2[i].ClearAllTiles();
            afterTileMap2[i].ClearAllTiles();
        }      
    }

    public groundData GetMapData(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return null;
        else return mapData[x, y];
    }

    public void UpdateMap(int x , int y, Player player)
    {
        //处理滋润状态的hp增加
        if (player!= null && player.stats.GROUND_isWaterDamp != 0 && mapData[x, y].type != "水洼")
        {
            if (mapData[x, y].hp < mapData[x, y].maxHP ) mapData[x, y].hp++;
        }
        else if (mapData[x, y].hp > 0)
            mapData[x,y].hp -= 1;

        if (player != null)
            mapData[x, y].applyEffectToPlayer(ref player, x, y);
        if (mapData[x, y].hp <= 0 && mapData[x, y].type != "草地")
        {
            
            int special_x, special_y;
            special_x = UnityEngine.Random.Range(0, width);
            special_y = UnityEngine.Random.Range(0, height);
            while (mapData[special_x, special_y].isSpecial && mapData[special_x, special_y].type != "象征之森")
            {
                special_x = UnityEngine.Random.Range(0, width);
                special_y = UnityEngine.Random.Range(0, height);
            }
            if (mapData[x, y].type != "金枝" && mapData[special_x, special_y].type != "象征之森")
            {
                TurnGrassToSpecial(special_x, special_y, mapData[x, y].type_id);
                posOfGrass.Remove(new Vector2(special_x, special_y));
            }
            //g为了存储瘟疫状态
            groundData g = mapData[x, y];
            //播放触发特效
            if (g.type != "黄金树")
            PrefabControl.Instance.createPrefabForSpecialGround(x, y, space, rotationCounts1, rotationCounts2, width,g.type);
            else
            {
                GoldenTree0.GetComponent<TriggerInfo>().EndAndDestroy();
                GoldenTree1.GetComponent<TriggerInfo>().EndAndDestroy();
            }
            SetGrass(x, y);
            mapData[x, y].hp = 0;
            //重新设置瘟疫状态
            if (g.isPlagued) mapData[x, y].beSick(x, y, g.wiredCoroutine);
        }
        UpdateTile(x, y);
    }
    public void AddImageForestByOne(Player player0, Player player1)
    {
        if (ImageForestEdgeValue >= width / 2) return;
        Vector2 startPoint = new Vector2(ImageForestEdgeValue, ImageForestEdgeValue);
        List<Vector2> dir = new List<Vector2> { new Vector2(0, 1), new Vector2(1, 0),new Vector2(0, -1),  new Vector2(-1, 0) };
        List<Player> diePlayers = new List<Player>();
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < width - 1 - ImageForestEdgeValue * 2; i++)
            { 
                startPoint += dir[j];
                int x = (int)startPoint.x; int y = (int)startPoint.y;
                SetSpecialGround(6, x, y);
                if (player0.pos_x == x && player0.pos_y == y)
                {
                    diePlayers.Add(player0); 
                }
                if (player1.pos_x == x && player1.pos_y == y)
                {
                    diePlayers.Add(player1);
                }
                UpdateTile((int)startPoint.x, (int)startPoint.y);
                posOfGrass.Remove(startPoint);
            }
        }
        if (diePlayers.Count != 0)
        {
            Player dieplayer = diePlayers[UnityEngine.Random.Range(0, diePlayers.Count)];
            mapData[dieplayer.pos_x, dieplayer.pos_y].applyEffectToPlayer(ref dieplayer, dieplayer.pos_x, dieplayer.pos_y);

        }
        
        ImageForestEdgeValue += 1;
    }
    public void TurnGrassToSpecial(int special_x, int special_y, int specialTypeId)
    {
        SetSpecialGround(specialTypeId, special_x, special_y);
        //转换为对应种类的特殊方块
        /*
        switch (specialTypeId)
        {
            case 0:
                builderDirector.construct(thornBuilder, ref specialGroundDatas, 0);
                mapData[special_x, special_y] = thornBuilder.getResult();
                break;
            case 1:
                builderDirector.construct(waterBuilder, ref specialGroundDatas, 1);
                mapData[special_x, special_y] = waterBuilder.getResult();
                break;
            case 3:
                builderDirector.construct(bushBuilder, ref specialGroundDatas, 3);
                mapData[special_x, special_y] = bushBuilder.getResult();
                break;
            case 4:
                builderDirector.construct(mushroomBuilder, ref specialGroundDatas, 4);
                mapData[special_x, special_y] = mushroomBuilder.getResult();
                break;
        }*/
        switch (mapTag["障碍物"])
        {
            case 0: break;
            case 1: 
                float v = UnityEngine.Random.value;
                if (v < 0.2)
                {
                    builderDirector.construct(obstacleBuilder, ref specialGroundDatas, 5);
                    mapData[special_x, special_y] = obstacleBuilder.getResult();
                }
                break;
            case 2:
                v = UnityEngine.Random.value;
                if (v < 0.45)
                {
                    builderDirector.construct(obstacleBuilder, ref specialGroundDatas, 5);
                    mapData[special_x, special_y] = obstacleBuilder.getResult();
                }
                break;
            case 3:
                v = UnityEngine.Random.value;
                if (v < 0.75)
                {
                    builderDirector.construct(obstacleBuilder, ref specialGroundDatas, 5);
                    mapData[special_x, special_y] = obstacleBuilder.getResult();
                }
                break;
        }
        UpdateTile(special_x, special_y);
    }

    public void UpdateTile(int x, int y)
    {
        KeyValuePair<TileBase, TileBase> TileToSet;
        if (mapData == null)
        {
            Debug.LogError("mapData is null!");
            return;
        }

        // 检查索引是否越界
        if (x < 0 || x >= mapData.GetLength(0) || y < 0 || y >= mapData.GetLength(1))
        {
            Debug.LogError($"Index out of bounds: x = {x}, y = {y}");
            return;
        }

        // 检查 mapData[x, y] 是否为空
        if (mapData[x, y] == null)
        {
            Debug.LogError($"mapData[{x}, {y}] is null!");
            return;
        }
        if (!mapData[x, y].isSpecial)
            TileToSet = mapData[x, y].updateTile(ref groundDatas);
        else
        {
            TileToSet = mapData[x, y].updateTile(ref specialGroundDatas);
        }
            
        if (true || TileToSet.Key != null)
        {
            Vector2 blockPosition1 = UtilFunction.ComputePosRotationforViewAdapter(false, rotationCounts1, new Vector2(x, y), width, Vector2.zero, 0);
            afterTileMap[(int)blockPosition1.y].SetTile(new Vector3Int((int)blockPosition1.x, (int)blockPosition1.y), TileToSet.Key);
            Vector2 blockPosition2 = UtilFunction.ComputePosRotationforViewAdapter(false, rotationCounts2, new Vector2(x, y), width, Vector2.zero, 0);
            afterTileMap2[(int)blockPosition2.y].SetTile(new Vector3Int((int)blockPosition2.x, (int)blockPosition2.y), TileToSet.Key);
        }

        if (true|| TileToSet.Value != null)
        {
            Vector2 blockPosition1 = UtilFunction.ComputePosRotationforViewAdapter(false, rotationCounts1, new Vector2(x, y), width, Vector2.zero, 0);
            beforeTileMap[(int)blockPosition1.y].SetTile(new Vector3Int((int)blockPosition1.x, (int)blockPosition1.y), TileToSet.Value);
            Vector2 blockPosition2 = UtilFunction.ComputePosRotationforViewAdapter(false, rotationCounts2, new Vector2(x, y), width, Vector2.zero, 0);
            beforeTileMap2[(int)blockPosition2.y].SetTile(new Vector3Int((int)blockPosition2.x, (int)blockPosition2.y), TileToSet.Value);
        }
    }
    // 玩家踩到草方块
    public void OnPlayerEnter(int x,int y)
    {
        mapData[x, y].playerEnter(x, y);
    }

    // 玩家离开草方块
    public void OnPlayerExit(int x,int y)
    {
        mapData[x, y].playerExit(x, y);
    }
    public void beSick(int x, int y)
    {
        mapData[x, y].beSick(x, y, null);
    }
    public void SetGrass(int x, int y)
    {
        builderDirector.construct(grassBuilder, ref groundDatas, 0);
        mapData[x, y] = grassBuilder.getResult();
    }
    public void SetSpecialGround(int i,int x,int y)
    {
        if (specialGroundDatas.Count <= i)
            return;
        if (specialGroundDatas[i].type == "荆棘")
        {
            builderDirector.construct(thornBuilder, ref specialGroundDatas, 0);
            mapData[x, y] = thornBuilder.getResult();
        }
        else if (specialGroundDatas[i].type == "水洼")
        {
            builderDirector.construct(waterBuilder, ref specialGroundDatas, 1);
            mapData[x, y] = waterBuilder.getResult();
        }
        else if (specialGroundDatas[i].type == "金枝")
        {
            builderDirector.construct(goldenBranchBuilder, ref specialGroundDatas, 2);
            mapData[x, y] = goldenBranchBuilder.getResult();
        }
        else if (specialGroundDatas[i].type == "灌木")
        {
            builderDirector.construct(bushBuilder, ref specialGroundDatas, 3);
            mapData[x, y] = bushBuilder.getResult();
        }
        else if (specialGroundDatas[i].type == "毒菇")
        {
            builderDirector.construct(mushroomBuilder, ref specialGroundDatas, 4);
            mapData[x, y] = mushroomBuilder.getResult();
        }
        else if(specialGroundDatas[i].type == "障碍物")
        {
            builderDirector.construct(obstacleBuilder, ref specialGroundDatas, 5);
            mapData[x, y] = obstacleBuilder.getResult();
        }
        else if (specialGroundDatas[i].type == "象征之森")
        {
            builderDirector.construct(imageForestBuilder, ref specialGroundDatas, 6);
            mapData[x, y] = imageForestBuilder.getResult();
        }
        else if (specialGroundDatas[i].type == "黄金树")
        {
            builderDirector.construct(goldenTreeBuilder, ref specialGroundDatas, 7);
            mapData[x, y] = goldenTreeBuilder.getResult();
        }
    }
}
