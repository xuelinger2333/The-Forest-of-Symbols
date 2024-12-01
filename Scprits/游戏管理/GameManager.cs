using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

//全局游戏管理器
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerInputControl1 inputControl1;
    public PlayerInputControl2 inputControl2;
    public GameObject Grid;
    public GameObject player0_game;
    public GameObject player1_game;
    public GameObject creatureGenerator;
    public bool winPlayerId;
    public Text goToNextPageHint;
    bool hintStart = false;
    public PlayerViewAdapter player_0_map_0, player_0_map_1, player_1_map_0, player_1_map_1;
    [HideInInspector]
    public Player player0;
    [HideInInspector]
    public Player player1;
    public Camera camera_left;
    public Camera camera_right;
    public Volume volume_left;
    public Volume volume_right;
    public Light2D light_left;
    public Light2D light_right;
    
    public TextAsset BattleConfigs;
    public bool playerId { get; private set; }
    public bool isGameRunning;
    public Text legacyText;
    [SerializeField]
    GameObject networkGameManager;
    [SerializeField]
    public GameObject playerParent;
    public bool FishNet_isUsingNetwork;
    private int bgmStage = 1;
    private float counterForChangeBGM;
    private bool mode;
    // Start is called before the first frame update
    void Awake()
    {
        UISoundManager.StopPlayMusic();
        isGameRunning = true;
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
        Grid.SetActive(false);
        GeneratePlayerByConfig();
    }
    private void Start()
    {
        mode = UnityEngine.Random.value > 0.5f;
        if (mode) SoundManager.PlayMusic(0,1f);
        else SoundManager.PlayMusic(3, 0.3f);
        counterForChangeBGM = 3;
        if (FishNet_isUsingNetwork)
        {
            creatureGenerator.SetActive(false);
            PauseGame();
            //StartCoroutine(Network_startGameCountDown());
        }
    }
    public void DisablePlayerInput(bool playerId)
    {
        if (playerId) inputControl1.Disable();
        else inputControl2.Disable();
    }
    public void EnablePlayerInput(bool playerId)
    {
        if (playerId) inputControl1.Enable();
        else inputControl2.Enable();
    }
    IEnumerator Network_startGameCountDown()
    {
        yield return new WaitForSecondsRealtime(1);
        BootStrapNetworkManager.Instance.SpawnGameSceneObject(networkGameManager);
    }
    //most Important part of Initialize
    public void GeneratePlayerByConfig()
    {
        BattleInfo info = ConfigReader.queryBattleInfo();
        if (info == null) return;

        //stop player
        player0_game.SetActive(false);
        player1_game.SetActive(false);
        //add script
        player0_game.AddComponent(Type.GetType(info.player0Id));
        player0 = player0_game.GetComponent<Player>();
       
        player1_game.AddComponent(Type.GetType(info.player1Id));
        player1 = player1_game.GetComponent<Player>();
        player1.playerID = true;
        //set enemy
        player1.enemy = player0;
        player0.enemy = player1;
        //set start position
        int positionNum_player1 = UnityEngine.Random.Range(0, 4);
        int positionNum_player0 = UnityEngine.Random.Range(0, 4);
        while (positionNum_player0 == positionNum_player1) positionNum_player0 = UnityEngine.Random.Range(0, 4); //确保两个数不同
        //设置随机的x位置, need to use again
        if (positionNum_player1 > 1) player1.pos_x = 14; else player1.pos_x = 0;
        if (positionNum_player0 > 1) player0.pos_x = 14; else player0.pos_x = 0;
        if (positionNum_player1 % 2 == 0) player1.pos_y = 14; else player1.pos_y = 0;
        if (positionNum_player0 % 2 == 0) player0.pos_y = 14; else player0.pos_y = 0;
        //set view Adapter
        player0.playerViewAdapter = player_0_map_0;
        player0.playerViewAdapter_map2 = player_0_map_1;
        player1.playerViewAdapter = player_1_map_0;
        player1.playerViewAdapter_map2 = player_1_map_1;
        //set camera and volumn
        player0.myCamera = camera_right;
        player1.myCamera = camera_left;

        player0.myVolume = volume_right;
        player1.myVolume = volume_left;
        //start player
        player0_game.SetActive(true);
        player1_game.SetActive(true);
        //ChangeLightColor
        CharacterColorInfo colorinfo_right = ConfigReader.queryColorByCharacter(player0.characterName);
        light_right.color = colorinfo_right.lightColor;
        CharacterColorInfo colorinfo_left = ConfigReader.queryColorByCharacter(player1.characterName);
        light_left.color = colorinfo_left.lightColor;
    }
    //Methods To Control Game Time Scale
    public void PauseGame()
    {
        Time.timeScale = 0;
        inputControl1.Disable();
        inputControl2.Disable();
    }
    public void UnPauseGame()
    {
        Time.timeScale = 1;
        inputControl1.Enable();
        inputControl2.Enable();
    }
    // Update is called once per frame
    void Update()
    {
        ChangeBGN();
        if(!isGameRunning)
        {
            if (goToNextPageHint.text == "" && !hintStart)
            {
                hintStart = true;
                Sequence s = DOTween.Sequence();
                s.AppendInterval(2);
                s.Append(goToNextPageHint.DOText(LocalizationManager.GetText(new TextQueryKey(UI_Text_Category.空格继续), 
                    GlobalGameManager.systemLanguage), 0.5F).SetEase(Ease.Linear));
                s.SetUpdate(true);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.Instance.UnPauseGame();
                //GlobalGameManager.Instance.LoadScenebyName("SettleScene");
                if (winPlayerId)
                    GlobalGameManager.Instance.GameResult(winPlayerId, player1.characterName, player1.characterName, player0.characterName);
                else
                    GlobalGameManager.Instance.GameResult(winPlayerId, player0.characterName, player1.characterName, player0.characterName);
            }
        }
    }
    private void ChangeBGN()
    {
        if (counterForChangeBGM > 0)
        {
            counterForChangeBGM -= Time.deltaTime;
        }
        else
        {
            if (mode)
            {
                if (Math.Abs(player0.pos_x - player1.pos_x) < 6 && Math.Abs(player0.pos_y - player1.pos_y) < 6 && bgmStage != 3)
                {
                    bgmStage = 3;
                    SoundManager.ChangeMusic(2, 0.5f);
                    counterForChangeBGM = 4f;
                }
                else if (Math.Abs(player0.pos_x - player1.pos_x) < 11 && Math.Abs(player0.pos_y - player1.pos_y) < 11 && bgmStage != 2)
                {
                    bgmStage = 2;
                    SoundManager.ChangeMusic(1, 0.5f);
                    counterForChangeBGM = 4f;
                }
                else if (bgmStage != 1 && (Math.Abs(player0.pos_x - player1.pos_x) > 11 || Math.Abs(player0.pos_y - player1.pos_y) > 11))
                {
                    bgmStage = 1;
                    SoundManager.ChangeMusic(0, 0.5f);
                    counterForChangeBGM = 4f;
                }
            }
            else
            {
                if (Math.Abs(player0.pos_x - player1.pos_x) < 8 && Math.Abs(player0.pos_y - player1.pos_y) < 8 && bgmStage != 2)
                {
                    bgmStage = 2;
                    SoundManager.ChangeMusic(4, 0.5f);
                    counterForChangeBGM = 4f;
                }
                else if ((Math.Abs(player0.pos_x - player1.pos_x) > 8 || Math.Abs(player0.pos_y - player1.pos_y) > 8) && bgmStage != 1)
                {
                    bgmStage = 1;
                    SoundManager.ChangeMusic(3, 0.5f);
                    counterForChangeBGM = 4f;
                }
            }
        }
    }
}
