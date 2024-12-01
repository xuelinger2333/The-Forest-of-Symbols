using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

enum CharacterName
{
    瘟疫医生 = 0,
    人偶新娘,
    倩影骇客,
    织梦魔女,
}
public class SettleControl : MonoBehaviour
{
    public Sprite[] characters;
    public Sprite[] nums;
    public Text script;
    public List<List<string>> script_text; 

    private void Awake()
    {
        
        script_text = new List<List<string>>();
        List<string> doctor = new List<string>()
        {
            "大疫渗入骨，小命匐于地。",
            "对我的疗程还满意吗？",
            "作为医生，我会的可不仅仅是救人。"
        };
        List<string> doll = new List<string>()
        {
            "爱情与生命，你皆不可得",
            "再美的玫瑰...也终将枯萎。",
            "请安息，会好好地让你成为玫瑰们的养分。"					
        };
        List<string> hacker = new List<string>()
        {
            "被我打输的人都不玩象征之林了",
            "哔嘟哔嘟，biubiu!",
            "检测到您的体温为：20℃",
            "猜猜我的背包可以装多少东西？"
        };
        List<string> witch = new List<string>()
        {
            "让我猜猜看，你此时的愿望是“别打了？”",
            "远远没有满足...！",
            "非常美味...但还是比不上她呢。",
            "安娜...快了，再等我一会。"							
        };
        script_text.Add(doctor);
        script_text.Add(doll);
        script_text.Add(hacker);
        script_text.Add(witch);
        Transform targetChild = transform.Find("比分");
        targetChild.GetComponent<Image>().sprite = nums[GlobalGameManager.Instance.ReturnScore(true)];
        if (GlobalGameManager.Instance.nameOfWinner == GlobalGameManager.Instance.leftCharaName)
            targetChild.localScale = new Vector3(1, 1, 1);
        else
            targetChild.localScale = new Vector3(0.7f, 0.7f, 1);

        targetChild = transform.Find("比分 (1)");
        targetChild.GetComponent<Image>().sprite = nums[GlobalGameManager.Instance.ReturnScore(false)];
        if (GlobalGameManager.Instance.nameOfWinner == GlobalGameManager.Instance.rightCharaName)
            targetChild.localScale = new Vector3(1, 1, 1);
        else
            targetChild.localScale = new Vector3(0.7f, 0.7f, 1);

        targetChild = transform.Find("人物");
        if (Enum.TryParse(GlobalGameManager.Instance.nameOfWinner, out CharacterName result))
        {
            targetChild.GetComponent<Image>().sprite = characters[(int)result];
            SetBackgroundColor((int)result);
        }
    }

    public void ChangeScene(string name)
    {
        UISoundManager.PlaySound(UISoundType.杏仁, 0.35F);
        SoundManager.instance.DestroyPersistentObject();
        if (name == "ChooseCharacterScene") UISoundManager.PlayMusic(1, 0.6f);
        GlobalGameManager.Instance.LoadScenebyName(name);
    }

    public void SetBackgroundColor(int characterName)
    {
        Transform targetChild = transform.Find("血液");
        targetChild.GetComponent<Image>().color = SetColor(characterName);
        targetChild = transform.Find("胜利");
        targetChild.GetComponent<Image>().color = SetColor(characterName);
        targetChild = transform.Find("台词");
        targetChild.GetComponent<Image>().color = SetColor(characterName);
        //script.DOText(SetText(characterName), 1f).SetEase(Ease.Linear); ;
    }

    public Color SetColor(int characterName)
    {
        switch (characterName)
        {
            case 0:
                return new Color(0,1, 0.1409934f,1);
            case 1:
                return new Color(1,0, 0.1409934f,1);
            case 2:
                return new Color(1, 0.3098039f, 0.7137255f,1);
            case 3:
                return new Color(0.9294118f, 0, 1, 1);
            default:
                Debug.Log("没找到");
                return Color.white;
        }
    }
    public string SetText(int characterName)
    {
        int text_id = UnityEngine.Random.Range(0, script_text[characterName].Count);
        
        return script_text[characterName][text_id];
    }
}

