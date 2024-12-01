using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreControl : MonoBehaviour
{
    public Sprite[] nums;
    public Text Text_left, Text_right;

    private void Awake()
    {
        Transform targetChild = transform.Find("比分");
        targetChild.GetComponent<Image>().sprite = nums[GlobalGameManager.Instance.ReturnScore(true)];
        targetChild = transform.Find("比分 (1)");
        targetChild.GetComponent<Image>().sprite = nums[GlobalGameManager.Instance.ReturnScore(false)];
        Character_Text_Category ch;
        if (Enum.TryParse(GlobalGameManager.Instance.leftCharaName, out ch))
            Text_left.text = LocalizationManager.GetText(new TextQueryKey(ch), GlobalGameManager.systemLanguage);
        if (Enum.TryParse(GlobalGameManager.Instance.rightCharaName, out ch))
            Text_right.text = LocalizationManager.GetText(new TextQueryKey(ch), GlobalGameManager.systemLanguage);
    }
    public void ContinueGame()
    {
        SoundManager.instance.DestroyPersistentObject();
        GlobalGameManager.Instance.LoadScenebyName("DemoScene");
    }
}
