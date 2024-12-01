using FishNet.Managing.Scened;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

interface LevelManager
{
    public void StartLevel();
}

public class HelpGameManager : MonoBehaviour
{
    public static HelpGameManager Instance;
    public int CurrentLevel;
    [SerializeField] InputMiddleWare inputMiddleWare;
    [SerializeField] MapGenerator map;
    [SerializeField] LevelManager levelManager;
    [SerializeField] LoadEventChannelSO loadEventChannel;
    [SerializeField] Player player;
    [SerializeField] Camera playerCamera;
    [SerializeField] Volume playerVolume;
    [SerializeField] PlayableDirector instrucionIn;
    [SerializeField] ReversePlayableDirector instructionOut;
    bool isStart = false;
    bool isInstructionIn = true;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
        player.myCamera = playerCamera;
        player.myVolume = playerVolume;
        Time.timeScale = 1;

    }
    void Start()
    {
        map.rotationCounts1 = 0;
        map.rotationCounts2 = 0;
        inputMiddleWare.DisableOperation(false, Operation.All);
        inputMiddleWare.DisableOperation(true, Operation.All);
        levelManager = GetComponent<LevelManager>();
        StartLevel();
    }

    void StartLevel()
    {
        //根据当前的level，弹出对应的弹窗
        //...
        //Time.timeScale = 0;
        //enabled = false;
        //OnStartWindowClick();
    }
    public void OnTabClick()
    {
        if (isStart)
        {
            if (isInstructionIn) instructionOut.Play();
            else instrucionIn.Play();
            isInstructionIn = !isInstructionIn;
        }
        else OnStartWindowClick();
        
    }
    public void OnStartWindowClick()
    {
        Time.timeScale = 1;
        //正式开始
        //enabled = true;
        if (!isStart)
            levelManager.StartLevel();        
        instructionOut.Play();
        isStart = true;
        isInstructionIn = false;
    }

    public void NextLevel()
    {
        //加载下一个场景
        switch (CurrentLevel)
        {
            case 0:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level1HelpScene");
                break;
            case 1:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level2HelpScene");
                break;
            case 2:
                UnityEngine.SceneManagement.SceneManager.LoadScene("Level3HelpScene");
                break;
            case 3:
                //完成新手教程
                UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
                break;
        }
    }
}
