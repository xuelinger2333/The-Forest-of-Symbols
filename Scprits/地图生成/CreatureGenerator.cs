using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureGenerator : MonoBehaviour
{
    Player p1, p2;
    CreatureTag creatureTag;
    public static CreatureGenerator Instance { get; private set; }
    public CrowHandler CrowHandler;
    public DeerHandler DeerHandler;
    public FireFlyHandler fireFlyHandler;
    public SnakeHandler SnakeHandler;

    int CrowGenerateCD;
    float CrowGenerateTimer;
    int WhiteCrowGenerateCD;
    float WhiteCrowGenerateTimer;
    int DeerGenerateCD;
    float DeerGenerateTimer;
    bool DeerGenerateSwitch = false;

    int width;
    int height;
    void Awake()
    {
        if (Instance == null) //生物创建器的单例
            Instance = this;
        else
            Destroy(gameObject);
        TileOrderComputer.ItemRegist("creature", 5);
        BattleInfo info = ConfigReader.queryBattleInfo();
        if (info == null) return;
        creatureTag = info.creatureTag;
        if (info.prefabMap != "")
        {
            creatureTag.乌鸦 = 0;
            creatureTag.九色鹿 = 0;
            creatureTag.毒蛇 = 0;
            creatureTag.萤火虫 = 0;
        }
    }
    List<Vector2Int> Generate_n_different_position(int n)
    {
        List<Vector2Int> res = new List<Vector2Int>();
        if (n <= 0) return res;

        int left = 0, right = width / n;
        float v = UnityEngine.Random.value;
        for (int i = 0; i < n; i++)
        {
            int x = UnityEngine.Random.Range(left, right);
            int y = UnityEngine.Random.Range(2, height - 2);
            x = Mathf.Clamp(x, 2, width - 2);
            if (v > 0.5f)
            {
                int t = y; y = x; x = t;
            }
            res.Add(new Vector2Int(x, y));
            left = right; right += width / n;
        }
        return res;
    }
    Vector2Int Generate_position_out_of_player(bool[,] positionRecord)
    {
        int x = UnityEngine.Random.Range(0, width);
        int y = UnityEngine.Random.Range(0, height);
        while (Mathf.Abs(x - p1.pos_x) < 3 || Mathf.Abs(x - p2.pos_x) < 3 ||
            Mathf.Abs(y - p1.pos_y) < 3 || Mathf.Abs(y - p2.pos_y) < 3 ||
            positionRecord[x, y])
        {
            x = UnityEngine.Random.Range(0, width);
            y = UnityEngine.Random.Range(0, height);
        }
        Vector2Int position = new Vector2Int(x, y);
        return position; 
    }
    private void Start()
    { 
        p1 = GameManager.Instance.player0;
        p2 = GameManager.Instance.player1;
        //如果在网络中处于客户端地位，则不执行任何操作
        if (GameManager.Instance.FishNet_isUsingNetwork 
            && !p1.NETWORK_isMainController 
            && !p2.NETWORK_isMainController)
        {
            enabled = false;
            gameObject.SetActive(false);
            return;
        }
        width = MapGenerator.Instance.width;
        height = MapGenerator.Instance.height;
        CrowHandler = CrowHandler.Instance;
        DeerHandler = DeerHandler.Instance;
        fireFlyHandler = FireFlyHandler.Instance;
        SnakeHandler = SnakeHandler.Instance;
       
        List<Vector2Int> generateList;
        int num = 0, num2 = 0;
        switch (creatureTag.乌鸦)
        {
            case 0: num = 0; CrowGenerateCD = 15;
                break;
            case 1: num = 2; CrowGenerateCD = 15;
                break;
            case 2: num = 3; CrowGenerateCD = 10;
                break;
            case 3: num = 5; CrowGenerateCD = 8;
                break;
        }
        CrowGenerateTimer = CrowGenerateCD;
        generateList = Generate_n_different_position(num);
        for (int i = 0;  i < generateList.Count; i++)
        {
            if (CrowHandler != null)
                CrowHandler.addCrowbyPosition(new Vector2(generateList[i].x, generateList[i].y));
        }

        switch (creatureTag.萤火虫)
        {
            case 0:
                num2 = 0; WhiteCrowGenerateCD = 15;
                break;
            case 1:
                num2 = 2; WhiteCrowGenerateCD = 15;
                break;
            case 2:
                num2 = 3; WhiteCrowGenerateCD = 10;
                break;
            case 3:
                num2 = 5; WhiteCrowGenerateCD = 8;
                break;
        }
        WhiteCrowGenerateTimer = WhiteCrowGenerateCD;
        generateList = Generate_n_different_position(num2);
        for (int i = 0; i < generateList.Count; i++)
        {
            if (fireFlyHandler != null)
                fireFlyHandler.addFireFlybyPosition(new Vector2(generateList[i].x, generateList[i].y));
        }
        DeerGenerateCD = 6;
        DeerGenerateTimer = 0;
        if (creatureTag.九色鹿 != 0) DeerGenerateSwitch = true;
    }
    private void Update()
    {
        if (CrowGenerateTimer >= 0) CrowGenerateTimer -= Time.deltaTime;
        else if (creatureTag.乌鸦 != 0)
        {
            Vector2Int position = Generate_position_out_of_player(CrowHandler.CrowPositionRecord);
            if (CrowHandler != null)
                CrowHandler.addCrowbyPosition(new Vector2(position.x, position.y));
            CrowGenerateTimer = CrowGenerateCD;
        }
        if (WhiteCrowGenerateTimer >= 0) WhiteCrowGenerateTimer -= Time.deltaTime;
        else if (creatureTag.萤火虫 != 0)
        {
            Vector2Int position = Generate_position_out_of_player(fireFlyHandler.WhiteCrowPositionRecord);
            if (fireFlyHandler != null)
                fireFlyHandler.addFireFlybyPosition(new Vector2(position.x, position.y));
            WhiteCrowGenerateTimer = WhiteCrowGenerateCD;
        }
        if( DeerGenerateTimer >= 0) DeerGenerateTimer -= Time.deltaTime;
        else if (DeerGenerateSwitch)
        {
            Vector2Int position = Generate_position_out_of_player(new bool[width, height]);
            if (DeerHandler != null)
                DeerHandler.addDeerbyPosition(new Vector2(position.x, position.y));
            DeerGenerateSwitch = false;
        }
    }
    public void GenerateDeer()
    {
        if (creatureTag.九色鹿 == 0) return;
        DeerGenerateSwitch = true;
        DeerGenerateTimer = DeerGenerateCD;
    }
    public void GenerateSnake(bool playerID)
    {
        if (creatureTag.毒蛇 == 0) return;
        Player p = (playerID == true) ? p2 : p1;
        if (SnakeHandler != null)
        {
            List<Vector2> dir = new List<Vector2> { new Vector2(0, 3), new Vector2(0, -3), new Vector2(3, 0), new Vector2(-3, 0) };
            List<Vector2> targets = new List<Vector2>();
            for (int i = 0; i < dir.Count; i++)
            {
                Vector2 compute_target = new Vector2(dir[i].x + p.pos_x, dir[i].y + p.pos_y);
                if (compute_target.x >= 0 && compute_target.x < width && compute_target.y >= 0 && compute_target.y < height)
                {
                    targets.Add(compute_target);
                }
            }
            Vector2 targetPos = targets[UnityEngine.Random.Range(0, targets.Count)];
            SnakeHandler.addSnakebyPosition(targetPos, Convert.ToInt32(playerID));
        }
    }
}
