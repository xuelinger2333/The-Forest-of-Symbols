using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Level3 : MonoBehaviour, LevelManager
{
    [SerializeField] InputMiddleWare inputMiddleWare;
    public Character Player;
    public List<Character> GameNPCs_level3;
    public List<PlayerViewManager> GameNPC_ViewManager_level3;

    [SerializeField] Material exposureMaterial;

    [SerializeField] Timer EndLevelTimer;
    [SerializeField] GameObject 假人parent;
    [SerializeField] GameObject hint;
    [SerializeField] PlayableDirector IconTime;
    public int Level3_DiscoverNum = 0;
    void Awake()
    {
        enabled = false;
        hint.SetActive(false);
        假人parent.SetActive(false);
    }
    public void StartLevel()
    {
        enabled = true;
        hint.SetActive(true);
        假人parent.SetActive(true);
        inputMiddleWare.EnableOperation(true, Operation.All);
        inputMiddleWare.DisableOperation(true, Operation.ActiveSkill);

        inputMiddleWare.EnableOperation(false, Operation.All);
        inputMiddleWare.DisableOperation(false, Operation.ActiveSkill);
        Player.CDController.AddUniqueSkillCount(100);
        IconTime.Play();
    }
    void NextLevel()
    {
        GetComponent<HelpGameManager>().NextLevel();
    }
    void ReloadScene()
    {
        // 获取当前活动的场景
        Scene currentScene = SceneManager.GetActiveScene();
        // 重新加载当前场景
        SceneManager.LoadScene(currentScene.buildIndex);
    }
    void Observe_Level3()
    {
        //第1关的过关监听
        //如果在第一关，每帧检测是否有新的人偶被发现
        int idx = 0;
        while (idx < GameNPCs_level3.Count)
        {
            if (GameNPCs_level3[idx].stats.exposureLevel_model == 3)
            {
                //改变材质
                GameNPC_ViewManager_level3[idx].ChangeMaterial(exposureMaterial);
                //计数
                Level3_DiscoverNum += 1;
                GameNPCs_level3.RemoveAt(idx);
                GameNPC_ViewManager_level3.RemoveAt(idx);
            }
            else
            {
                idx++;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        Observe_Level3();
        if (Level3_DiscoverNum >= 24)
        {
            inputMiddleWare.DisableOperation(true, Operation.All);
            EndLevelTimer.targetTime = 1;
            EndLevelTimer.StartTimer(NextLevel);
            enabled = false;
        }
    }
}
