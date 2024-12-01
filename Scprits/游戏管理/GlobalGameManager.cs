using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager Instance { get; private set; }
    int player0_win_time;
    int player1_win_time;
    public string nameOfWinner;
    public string leftCharaName;
    public string rightCharaName;
    GameMode gameMode;
    public TextAsset CharacterConfigs;
    public SystemLanguage out_systemLanguage;
    public static SystemLanguage systemLanguage;
    public Font EnglishFont;
    public Font ChineseFont;
    public void GameStart()
    {
        if (Enum.TryParse(ConfigReader.battleMode, out gameMode))
        {
            player0_win_time = 0;
            player1_win_time = 0;
            LoadScenebyName("DemoScene"); //第一次游戏
        }
    }
    public void GameEnd(bool winPlayerId)
    {
        LoadScenebyName("SettleScene");
    }
    public void GameResult(bool winPlayerId, string characterName, string character_left_name, string character_right_name)
    {
        nameOfWinner = characterName;
        leftCharaName = character_left_name;
        rightCharaName = character_right_name;
        if (winPlayerId) player1_win_time++; else player0_win_time++;
        int max_player = Math.Max(player0_win_time, player1_win_time);
        switch (gameMode)
        {
            case GameMode.mode1_1:
                GameEnd(winPlayerId);
                break;
            case GameMode.mode3_2: 
                if (max_player < 2)
                {
                    LoadScenebyName("ScoreScene");
                }
                else
                {
                    GameEnd(winPlayerId);
                }
                break;
            case GameMode.mode5_3:
                if (max_player < 3)
                {
                    LoadScenebyName("ScoreScene");
                }
                else
                {
                    GameEnd(winPlayerId);
                }
                break;
        }
    }
    public enum GameMode
    {
        mode1_1,
        mode3_2,
        mode5_3,
    }
    
    public enum Scene
    {
        StartScene,
        InstructionScene,
        ChooseMapScene,
        ChoosePrefabScene,
        ChooseTagScene,
        RandomTagScene,
        ChooseCharacterScene,
        SetBattleModeScene,
        DemoScene,
        GameEndScene,
        ScoreScene,
        SettleScene,
    }
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        systemLanguage = out_systemLanguage;
    }

    private void Start()
    {
       // LoadScenebyName("DemoScene");
    }
    public void LoadScenebyName(string sceneName)
    {
        Debug.Log(sceneName);
        Scene result;
        if (Enum.TryParse(sceneName, out result))
        {
            //StartCoroutine(LoadScene(result));
            SceneManager.LoadScene(sceneName);
            if(sceneName == "StartScene") UISoundManager.PlayMusic(0, 0.6f);
        }
    }
    IEnumerator LoadScene(Scene scene)//场景跳转
    {
        int number = (int)scene;
        AsyncOperation operation;
        if (SceneManager.LoadSceneAsync(number) != null)
        {
            operation = SceneManager.LoadSceneAsync(number);
        }
        else operation = SceneManager.LoadSceneAsync(0);

        operation.allowSceneActivation = true;

        yield return null;
    }

    public int ReturnScore(bool playerID)
    {
        if(playerID)
        {
            return player1_win_time;
        }
        return player0_win_time;
    }
}
