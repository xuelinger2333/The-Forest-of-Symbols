using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class Level0 : MonoBehaviour, LevelManager
{
    [SerializeField] InputMiddleWare inputMiddleWare;
    public List<Character> GameNPCs_level0;
    public List<PlayerViewManager> GameNPC_ViewManager_level0;

    [SerializeField] Material exposureMaterial;
    [SerializeField] Timer EndLevelTimer;
    [SerializeField] GameObject hint;
    [SerializeField] TextMeshProUGUI hint_discoverNum;
    [SerializeField] GameObject 假人Parent;
    [SerializeField] PlayableDirector instructionIn;
    [SerializeField] GameObject startPage;
    int Level0_DiscoverNum = 0;
    void Awake()
    {
        enabled = false;
        hint.SetActive(false);
        假人Parent.SetActive(false);
        hint_discoverNum.gameObject.SetActive(false);
    }
    public void OnStartClick()
    {
        startPage.SetActive(false);
        instructionIn.Play();
        PlayerPrefs.SetInt("firstGame", 1);
        PlayerPrefs.Save();
    }
    public void StartLevel()
    {
        enabled = true;
        hint.SetActive(true);
        假人Parent.SetActive(true);
        inputMiddleWare.EnableOperation(true, Operation.All);
        inputMiddleWare.DisableOperation(true, Operation.ActiveSkill);
        inputMiddleWare.DisableOperation(true, Operation.ChargeAttack);
        inputMiddleWare.DisableOperation(true, Operation.UniqueSkill);
        inputMiddleWare.DisableOperation(true, Operation.MainAttack);

        inputMiddleWare.EnableOperation(false, Operation.All);
        inputMiddleWare.DisableOperation(false, Operation.ActiveSkill);
        inputMiddleWare.DisableOperation(false, Operation.ChargeAttack);
        inputMiddleWare.DisableOperation(false, Operation.UniqueSkill);
        inputMiddleWare.DisableOperation(false, Operation.MainAttack);
    }

    void Observe_Level0()
    {
        //第1关的过关监听
        //如果在第一关，每帧检测是否有新的人偶被发现
        int idx = 0;
        while (idx < GameNPCs_level0.Count)
        {
            if (GameNPCs_level0[idx].stats.exposureLevel_model == 3)
            {
                //改变材质
                GameNPC_ViewManager_level0[idx].ChangeMaterial(exposureMaterial);
                //计数
                Level0_DiscoverNum += 1;
                hint_discoverNum.text = LocalizationManager.GetText(new TextQueryKey(Instruction_Text_Category.隐匿暴露_完成), GlobalGameManager.systemLanguage)
                    + Level0_DiscoverNum.ToString() + LocalizationManager.GetText(new TextQueryKey(Instruction_Text_Category.个), GlobalGameManager.systemLanguage);
                GameNPCs_level0.RemoveAt(idx);
                GameNPC_ViewManager_level0.RemoveAt(idx);
            }
            else
            {
                idx++;
            }
        }
    }
    void NextLevel()
    {
        GetComponent<HelpGameManager>().NextLevel();
    }
    // Update is called once per frame
    void Update()
    {
        Observe_Level0();
        if (Level0_DiscoverNum == 3)
        {
            //执行第1关完成的函数
            inputMiddleWare.DisableOperation(true, Operation.All);
            EndLevelTimer.targetTime = 1;
            EndLevelTimer.StartTimer(NextLevel);
            enabled = false;
        }
    }
}
