using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2 : MonoBehaviour, LevelManager
{
    [SerializeField] InputMiddleWare inputMiddleWare;
    public Character GameNPCs_level1;
    public Character Player;
    public PlayerViewManager GameNPC_ViewManager_level1;

    [SerializeField] Timer EndLevelTimer;
    [SerializeField] GameObject hint;
    void Awake()
    {
        enabled = false;
        hint.SetActive(false);
    }
    public void StartLevel()
    {
        enabled = true;
        hint.SetActive(true);
        inputMiddleWare.EnableOperation(true, Operation.All);
        inputMiddleWare.DisableOperation(true, Operation.ActiveSkill);
        inputMiddleWare.DisableOperation(true, Operation.UniqueSkill);

        inputMiddleWare.EnableOperation(false, Operation.All);
        inputMiddleWare.DisableOperation(false, Operation.ActiveSkill);
        inputMiddleWare.DisableOperation(false, Operation.UniqueSkill);

        GameNPCs_level1.stats.SetPlayerAttackSpeed(GameNPCs_level1.stats.currentAttackSpeed + 4);
    }
    void NextLevel()
    {
        GetComponent<HelpGameManager>().NextLevel();
    }
    void PlayerDefeat()
    {
        Time.timeScale = 0;
        GameNPC_ViewManager_level1.StopAllAnimation();
        GameNPC_ViewManager_level1.ChangeSpriteColor(new Color(1, 1, 1, 1));
        Player.playerViewAdapter.sr.sortingLayerName = "AfterAllGameObject";
        EndLevelTimer.targetTime = 2f;
        EndLevelTimer.UnScaleTime = true;
        EndLevelTimer.StartTimer(ReloadScene);
    }
    void ReloadScene()
    {
        // 获取当前活动的场景
        Scene currentScene = SceneManager.GetActiveScene();
        // 重新加载当前场景
        SceneManager.LoadScene(currentScene.buildIndex);
    }
    void PlayerWin()
    {
        Time.timeScale = 0;
        EndLevelTimer.targetTime = 2f;
        EndLevelTimer.UnScaleTime = true;
        EndLevelTimer.StartTimer(NextLevel);
    }
    // Update is called once per frame
    void Update()
    {
        //检测玩家位置，位于攻击范围立刻攻击
        if (Player.pos_y == GameNPCs_level1.pos_y && Player.pos_x == GameNPCs_level1.pos_x)
        {
            GameNPCs_level1.OnMainAttack();
        }
        if (Player.stats.isDead)
        {
            PlayerDefeat();
            enabled = false;
        }
        if (GameNPCs_level1.stats.isDead)
        {
            PlayerWin();
            enabled = false;
        }
    }
}
