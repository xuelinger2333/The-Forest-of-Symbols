using HeathenEngineering.SteamworksIntegration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StatsData
{
    SKILL_EXPO_START,
    SKILL_EXPO_END,
    MODEL_EXPO
}
public interface PlayerStatsObserver
{
    void OnNotify(PlayerStats playerStats, StatsData data);
}
/// <summary>
/// 这里负责统计玩家的各项数值，以及进入各种状态的函数
/// </summary>
public partial class PlayerStats : MonoBehaviour
{
    List<PlayerStatsObserver> PlayerStatsObservers = new List<PlayerStatsObserver>(); 
    private Character player;
    [HideInInspector]
    public PrefabControl prefabControl;
    //不同步
    #region 移速变量
    public float moveTime { get; private set; } = BASIC_MOVE_TIME;
    public int currentMoveSpeed { get; private set; } = BASIC_MOVE_SPEED;

    public const int BASIC_MOVE_SPEED = 4;
    private const int MAX_MOVE_SPEED = 10; 
    private const float BASIC_MOVE_TIME = 0.2f;
    #endregion  
    //不同步
    #region 攻速变量
    public float attackTime { get; private set; } = BASIC_ATTACK_TIME;
    public int currentAttackSpeed { get; private set; } = BASIC_ATTACK_SPEED;

    public const int BASIC_ATTACK_SPEED = 4;
    private const int MAX_ATTACK_SPEED = 10;
    public const float BASIC_ATTACK_TIME = 0.4f;
    #endregion
    //不同步
    #region 擦刀速度变量
    public float attackDelayTime { get; private set; } = BASIC_ATTACK_DELAY_TIME;
    public int currentAttackDelaySpeed { get; private set; } = BASIC_ATTACK_DELAY_SPEED;

    public const int BASIC_ATTACK_DELAY_SPEED = 4;
    private const int MAX_ATTACK_DELAY_SPEED = 10;
    public const float BASIC_ATTACK_DELAY_TIME = 2.0f;
    #endregion
    //不同步
    #region 蓄力速度变量
    public float accumulateTime { get; private set; } = BASIC_ACCUMULATE_TIME;
    public int currentAccumulateSpeed { get; private set; } = BASIC_ACCUMULATE_SPEED;

    public const int BASIC_ACCUMULATE_SPEED = 4;
    private const int MAX_ACCUMULATE_SPEED = 10;
    public const float BASIC_ACCUMULATE_TIME = 1.0f;
    #endregion

    #region 状态标识
    public bool isDead = false;
    public bool GROUND_isThornImprison = false;
    public int GROUND_isWaterDamp = 0;
    public int GROUND_isMushroomPoisoned = 0;
    public bool CREATURE_isSnakeBite = false;
    public bool isSheep = false;
    public bool isImprison = false;
    public bool isWitched = false;
    public bool isEntranced = false;
    public bool isWarTired = false;
    public bool isMoving = false;
    public bool isSick = false;
    public int HACKER_emPulseLayerCount = 0;
    public int deathPlagueCount = 0;
    public bool isDream = false;
    public bool isCoffinExist = false; //记录是否有棺椁存在的bool
    public int shadowOnFace = 0;
    public int CAMERA_bushCover = 0;

    struct DeathPlagueControl
    {
        public Coroutine deathPlagueCoroutine;
        public GameObject[] deathPlaguePrefeb;
    };
    private DeathPlagueControl deathPlagueControl = new DeathPlagueControl();
    private float plagueTimer = 8;
    #endregion
    private void Start()
    {
        player = GetComponent<Character>();
        prefabControl = PrefabControl.Instance;
    }
    #region 暴露程度
    public int exposureLevel_model = 0;
    public int exposureLevel_skill = 0;
    public void StartExposureLevel_skill()
    {
        exposureLevel_skill += 1; // 记录有几个效果使玩家暴露，只要大于0就是效果暴露
        if (exposureLevel_skill == 1)
        for (int i = 0; i < PlayerStatsObservers.Count; i++)
        {
            PlayerStatsObservers[i].OnNotify(this, StatsData.SKILL_EXPO_START);
        }
    }
    public void EndExposureLevel_skill()
    {
        exposureLevel_skill -= 1;
        if (exposureLevel_skill == 0)
        for (int i = 0; i < PlayerStatsObservers.Count; i++)
        {
            PlayerStatsObservers[i].OnNotify(this, StatsData.SKILL_EXPO_END);
        }
    }
    public void SetExposureLevel_model(int val)
    {
        if (exposureLevel_model != val)
        for (int i = 0; i < PlayerStatsObservers.Count; i++)
        {
            PlayerStatsObservers[i].OnNotify(this, StatsData.MODEL_EXPO);
        }        
        exposureLevel_model = val; // 因为只有草地会赋予这个效果，所以没有做取最大值的操作，而是直接赋值

    }
    //仅供网络使用
    public void SetExposureLevel_skill_network(int val)
    {
        if (!GameManager.Instance.FishNet_isUsingNetwork) return;
        exposureLevel_skill = val;
    }
    #endregion
    #region 改变速度
    public void SetPlayerMoveSpeed(int value)
    {
        int basis = value > MAX_MOVE_SPEED ? MAX_MOVE_SPEED : value;
        moveTime = BASIC_MOVE_TIME * (1 + 0.15f * (BASIC_MOVE_SPEED - basis));
        currentMoveSpeed = value;
    }

    public void SetPlayerAttackSpeed(int value)
    {
        int basis = value > MAX_ATTACK_SPEED ? MAX_ATTACK_SPEED : value;
        attackTime = BASIC_ATTACK_TIME * (1 + 0.15f * (BASIC_ATTACK_SPEED - basis));
        currentAttackSpeed = value;
    }

    public void SetPlayerAttackDelaySpeed(int value)
    {
        int basis = value > MAX_ATTACK_DELAY_SPEED ? MAX_ATTACK_DELAY_SPEED : value;
        attackDelayTime = BASIC_ATTACK_DELAY_TIME * (1 + 0.15f * (BASIC_ATTACK_DELAY_SPEED - basis));
        currentAttackDelaySpeed = value;
    }

    public void SetPlayerAccumulateSpeed(int value)
    {
        int basis = value > MAX_ACCUMULATE_SPEED ? MAX_ACCUMULATE_SPEED : value;
        accumulateTime = BASIC_ACCUMULATE_TIME * (1 + 0.15f * (BASIC_ACCUMULATE_SPEED - basis));
        currentAccumulateSpeed = value;
    }
    #endregion
 
    public void AddStatsObserver(PlayerStatsObserver ob)
    {
        PlayerStatsObservers.Add(ob);
    }

    public void ChangeWitchedState(int time)
    {
        StartCoroutine(WitchedState(time));
    }

    public void ChangeEntrancedState(int time)
    {
        StartCoroutine(EntrancedState(time));
    }
    public void GROUND_ChangeThornState(int time)
    {
        StartCoroutine(GROUND_ThornState(time));
    }
    //public void GROUND_ChangeWaterState(int time)
    //{
    //    StartCoroutine(GROUND_WaterState(time));
    //}
    public void GROUND_ChangeBushState(int time)
    {
        StartCoroutine(GROUND_BushState(time));
    }
    public void GROUND_ChangeMushroomState(int time)
    {
        StartCoroutine(GROUND_MushroomState(time));
    }
    public void CREATURE_ChangeSnakeBiteState(int time)
    {
        StartCoroutine(CREATURE_SnakeBite(time));
    }

    public void ChangeWovenDreamsState(float time)
    {
        StartCoroutine(WovenDreamsState(time));
    }

    public void HACKER_StartEmPulse()
    {
        if (HACKER_emPulseLayerCount == 0)
        {
            player.action.ChangeAction(Actions.SHPassiveAction, 0);
        }
        HACKER_emPulseLayerCount++;
    }
    public void HACKER_EndEmPulse()
    {
        HACKER_emPulseLayerCount--;
        if (HACKER_emPulseLayerCount == 0)
        {
            player.action.ChangeAction(Actions.StopSHPassiveAction, 0);
        }
    }

    IEnumerator WitchedState(int time)
    {
        if (player.stateMachine.currentState == player.chargeState) player.stateMachine.ChangeState(player.idleState);
        isWitched = true;
        Debug.Log("人偶新娘：" + gameObject.name + "进入魅惑状态");
        yield return new WaitForSeconds(time);
        isWitched = false;
        isMoving = false;
    }
    IEnumerator EntrancedState(int time)
    {
        isEntranced = true;
        isMoving = false;
        yield return new WaitForSeconds(0.01f);
        Debug.Log("人偶新娘：" + gameObject.name + "进入神魂颠倒状态");
        yield return new WaitForSeconds(time);
        isMoving = false;
        isEntranced = false;
    }
    IEnumerator WovenDreamsState(float time)
    {
        isDream = true;
        yield return new WaitForSeconds(time);
        isDream = false;
    }
    IEnumerator GROUND_ThornState(int time)
    {
        StartExposureLevel_skill();
        GROUND_isThornImprison = true;
        yield return new WaitForSeconds(time);
        EndExposureLevel_skill();
        GROUND_isThornImprison = false;
    }
    //IEnumerator GROUND_WaterState(int time)
    //{
    //    SetPlayerAttackSpeed(currentAttackSpeed + 1);
    //    SetPlayerMoveSpeed(currentMoveSpeed - 1);
    //    if (GROUND_isWaterDamp == 0)
    //    {
    //        player.action.ChangeAction(Actions.WaterAction, 0);
    //    }

    //    GROUND_isWaterDamp += 1;
    //    Debug.Log("进入滋润状态!");
    //    yield return new WaitForSeconds(time);
    //    SetPlayerAttackSpeed(currentAttackSpeed - 1);
    //    SetPlayerMoveSpeed(currentMoveSpeed + 1);
    //    GROUND_isWaterDamp -= 1;
    //    if (GROUND_isWaterDamp == 0)
    //    {
    //        player.action.ChangeAction(Actions.StopWaterAction, 0);
    //    }

    //    Debug.Log("退出滋润状态！");
    //}
    IEnumerator GROUND_BushState(int time)
    {
        player.StartCameraCover();
        Debug.Log("进入灌木遮挡状态!");
        yield return new WaitForSeconds(time);
        Debug.Log("退出灌木遮挡状态！");
        player.EndCameraCover();
    }
    IEnumerator GROUND_MushroomState(int time)
    {
        int attackSpeed = UnityEngine.Random.Range(0, 5); //expect[0, 5)
        int moveSpeed = 4 - attackSpeed;
        float attackAdd = UnityEngine.Random.value;
        float moveAdd = UnityEngine.Random.value;

        if (attackAdd > 0.5f) attackSpeed *= -1;
        if (moveAdd > 0.5f) moveSpeed *= -1;
        SetPlayerAttackSpeed(currentAttackSpeed + attackSpeed);
        SetPlayerMoveSpeed(currentMoveSpeed + moveSpeed);
        if (GROUND_isMushroomPoisoned == 0)
        {
            player.lensDistortion.intensity.value = -0.5f;
            isMoving = false;
        }

        GROUND_isMushroomPoisoned += 1;
        Debug.Log("进入毒菇中毒状态!");
        yield return new WaitForSeconds(time);

        SetPlayerAttackSpeed(currentAttackSpeed - attackSpeed);
        SetPlayerMoveSpeed(currentMoveSpeed - moveSpeed);
        GROUND_isMushroomPoisoned -= 1;
        if (GROUND_isMushroomPoisoned == 0)
        {
            player.lensDistortion.intensity.value = 0f;
            isMoving = false;
        }

        Debug.Log("退出毒菇中毒状态！");
    }
    IEnumerator CREATURE_SnakeBite(int time)
    {
        player.stateMachine.ChangeState(player.dizzyState);
        player.dizzyTime = time;
        CREATURE_isSnakeBite = true;
        StartExposureLevel_skill();
        yield return new WaitForSeconds(time);
        CREATURE_isSnakeBite = false;
        EndExposureLevel_skill();
    }
    #region 瘟疫医生相关
    IEnumerator Cough(float time)
    {
        StartExposureLevel_skill();
        IconManager.instance.PlayIcon(1001, IconType.医生咳嗽, player.playerID);
        yield return new WaitForSeconds(time);
        EndExposureLevel_skill();
        IconManager.instance.StopIcon(player.playerID, IconType.医生咳嗽);
    }
    GameObject[] startPlague()
    {
        GameObject plagueSE = TriggerManager.ExecuteTrigger("角色瘟疫_泡泡", player.playerViewAdapter.gameObject);
        GameObject plagueSE2 = TriggerManager.ExecuteTrigger("角色瘟疫_泡泡", player.playerViewAdapter_map2.gameObject);

        GameObject CrowInstance = TriggerManager.ExecuteTrigger("角色瘟疫_乌鸦", player.playerViewAdapter.gameObject);
        GameObject CrowInstance2 = TriggerManager.ExecuteTrigger("角色瘟疫_乌鸦", player.playerViewAdapter_map2.gameObject);

        GameObject[] array = { plagueSE, plagueSE2, CrowInstance, CrowInstance2 };
        return array;
    }
    IEnumerator DeathPlague(GameObject[] child)
    {

        isSick = true;
        float coughCD = 0;
        float phantomCD = 0;
        switch (deathPlagueCount)
        {
            case 1:
                coughCD = 4f; phantomCD = 4f;
                break;
            case 2:
                coughCD = UnityEngine.Random.Range(1, 4); phantomCD = coughCD;
                break;
            case 3:
                coughCD = 1.5f; int num = UnityEngine.Random.Range(1, 4); 
                //触发幻觉（之前）：每秒1、2、3次幻觉
                //修改后：每2秒1、2、3次幻觉
                phantomCD = 2 / num;
                break;
        }
        float coughTimer = coughCD;
        float VistionTimer = phantomCD;
        while (plagueTimer >= 0)
        {
            yield return null;
            SpriteRenderer sr = child[1].GetComponent<SpriteRenderer>();
            sr.sortingOrder = player.playerViewAdapter.sr.sortingOrder - 1;
            coughTimer -= Time.deltaTime;
            plagueTimer -= Time.deltaTime;
            VistionTimer -= Time.deltaTime;
            if (coughTimer < 0)
            {
                Debug.Log("咳嗽");
                StartCoroutine(Cough(0.75f));
                if (deathPlagueCount == 2) coughCD = UnityEngine.Random.Range(1, 4);
                coughTimer = coughCD;
            }
            if (VistionTimer < 0)
            {
                CreatVisition();

                if (deathPlagueCount == 2)
                {
                    phantomCD = coughCD;
                }
                if (deathPlagueCount == 3)
                {
                    int num = UnityEngine.Random.Range(1, 4);
                    //修改后，每2秒1、2、3次幻觉
                    phantomCD = 2 / num;
                }
                VistionTimer = phantomCD;
            }
        }
        isSick = false;

        for (int i = 0; i < child.Length; i++)
        {
            child[i].GetComponent<TriggerInfo>().EndAndDestroy();
        }
        deathPlagueControl.deathPlagueCoroutine = null;

    }
    public void DeathPlagueOnPlayer()
    {
        GameObject[] g = new GameObject[4];
        if (deathPlagueCount < 3)
        {
            deathPlagueCount += 1;

        }
        plagueTimer = 8;
        IconManager.instance.PlayIcon(plagueTimer, IconType.中毒, player.playerID, deathPlagueCount - 1);
        if (deathPlagueControl.deathPlagueCoroutine != null)
        {
            g = deathPlagueControl.deathPlaguePrefeb;
            StopCoroutine(deathPlagueControl.deathPlagueCoroutine);
            deathPlagueControl.deathPlagueCoroutine = null;
        }
        else
        {
            g = startPlague();
            deathPlagueControl.deathPlaguePrefeb = g;
        }
        deathPlagueControl.deathPlagueCoroutine = StartCoroutine(DeathPlague(g));
    }
    private void CreatVisition()
    {
        GameObject visition = Instantiate(prefabControl.visitionPrefab);
        VisitionControl vc = visition.GetComponent<VisitionControl>();
        Vector2 pos;
        int x = UnityEngine.Random.Range(-2, 3);
        int y = UnityEngine.Random.Range(-2, 3);
        int pos_x = Mathf.Clamp(player.pos_x + x, 0, 15);
        int pos_y = Mathf.Clamp(player.pos_y + y, 0, 15);
        if (player.playerID)
        {

            pos = UtilFunction.ComputePosRotationforViewAdapter(!player.playerID, player.map.rotationCounts1, new Vector2(pos_x, pos_y), player.map.width, new Vector2(0.5f, 0.5f), player.map.space);
            vc.sr.sortingOrder = TileOrderComputer.TileOrder((int)pos.x, (int)pos.y, "player");
            visition.transform.position = new Vector3(pos.x, pos.y, 0);
        }
        else
        {
            pos = UtilFunction.ComputePosRotationforViewAdapter(!player.playerID, player.map.rotationCounts2, new Vector2(pos_x, pos_y), player.map.width, new Vector2(0.5f, 0.5f), player.map.space);
            vc.sr.sortingOrder = TileOrderComputer.TileOrder((int)pos.x, (int)pos.y, "player");
            visition.transform.position = new Vector3(pos.x, pos.y, 0);
        }
    }
    #endregion
}
