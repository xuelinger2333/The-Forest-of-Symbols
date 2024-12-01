using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructionController : MonoBehaviour
{
    public Text Description;
    public GameObject arrow_l, arrow_r;
    int page_num = 0;
    public void ReturnToLastPage()
    {
        GlobalGameManager.Instance.LoadScenebyName("ChooseMapScene");
    }
    public void SetText(Button button)
    {
        Instruction_Text_Category instruction;
        Enum.TryParse(button.name + "_内容", out instruction);
        Description.text = LocalizationManager.GetText(new TextQueryKey(instruction), GlobalGameManager.systemLanguage);
        return;
    }

}
