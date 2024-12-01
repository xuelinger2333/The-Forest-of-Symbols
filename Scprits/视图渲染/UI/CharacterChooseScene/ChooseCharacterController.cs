using DG.Tweening;
using FishNet.Demo.AdditiveScenes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class ChooseCharacterController : MonoBehaviour
{
    public PlayableDirector awakeTimeLine_l;
    public PlayableDirector awakeTimeLine_r;
    public PlayableDirector changeCharacterTimeLine_l;
    public PlayableDirector changeCharacterTimeLine_r;
    public PlayableDirector changeCharacterTimeLine_l_toleft;    
    public PlayableDirector changeCharacterTimeLine_r_toright;

    public PlayableDirector changeCharacterTimeLine_skill_left;
    public PlayableDirector changeCharacterTimeLine_skill_right;


    public PlayableDirector changeSkillPage_left;
    public PlayableDirector changeSkillPage_right;

    public List<GameObject> character_left;
    public List<GameObject> character_right;

    public GameObject skill_left;
    public GameObject skill_right;
    public GameObject text_left;
    public GameObject text_right;

    public GameObject active_text_left;
    public GameObject unique_text_left;
    public GameObject passive_text_left;
    public GameObject active_text_right;
    public GameObject unique_text_right;
    public GameObject passive_text_right;

    public GameObject arrow_left_1, arrow_right_1, arrow_left_0, arrow_right_0;
    public ParticleSystem particle_left, particle_right;

    public int character_left_id;
    public int character_right_id;

    public Sprite select_on;
    public Sprite select_off;

    public GameObject toggle_left;
    public GameObject toggle_right;

    bool left_select_complete = false;
    bool right_select_complete = false;

    Dictionary<int, string> character_id2Name;
    Dictionary<int, string> character_id2ChiName;
    Coroutine changeSkillPage_left_reverse, changeSkillPage_right_reverse;
    bool left_in_skill_page = false;
    bool right_in_skill_page = false;

    public List<Sprite> countDown;
    public GameObject countDownObject;
    Sequence s;
    private void Update()
    {
        if (left_select_complete && right_select_complete && !countDownObject.activeSelf)
        {
            UISoundManager.PlaySound(UISoundType.鼓声, 1f);
            countDownObject.SetActive(true);
            countDownObject.GetComponent<Image>().sprite = countDown[3];
            s = DOTween.Sequence();
            s.AppendInterval(3);
            s.InsertCallback(1, () => countDownObject.GetComponent<Image>().sprite = countDown[2]);
            s.InsertCallback(2, () => countDownObject.GetComponent<Image>().sprite = countDown[1]);
            s.InsertCallback(3, () => countDownObject.GetComponent<Image>().sprite = countDown[0]);
            s.OnComplete(() => {ConfigReader.player1Id = character_id2Name[character_left_id];
                ConfigReader.player0Id = character_id2Name[character_right_id];
                ConfigReader.SaveBattleInfo();
                GlobalGameManager.Instance.GameStart();
                });
        }
        else
        {
            if (countDownObject.activeSelf && !(left_select_complete && right_select_complete))
            {
                s.Kill();
                countDownObject.SetActive(false);
            }
        }
    }
    public void ReturnToChooseMap()
    {
        UISoundManager.PlaySound(UISoundType.退出木头声, 0.5f);
        GlobalGameManager.Instance.LoadScenebyName("SetBattleModeScene");
    }
    private void Awake()
    {
        character_left_id = 0;
        character_right_id = 0;

        character_id2Name = new Dictionary<int, string>();
        character_id2Name[0] = "DollBride";
        character_id2Name[1] = "PlagueDoctor";
        character_id2Name[2] = "ShadowHacker";
        character_id2Name[3] = "Dreamweaver";

        character_id2ChiName = new Dictionary<int, string>();
        character_id2ChiName[0] = "人偶新娘";
        character_id2ChiName[1] = "瘟疫医生";
        character_id2ChiName[2] = "倩影骇客";
        character_id2ChiName[3] = "织梦魔女";
        Sequence awake = DOTween.Sequence();
        awake.AppendInterval(0.1f);
        awake.OnComplete(() => { awakeTimeLine_l.Play(); awakeTimeLine_r.Play(); });
    }
    void PlayChangeCharacterTimeLine(PlayableDirector timeLine, GameObject oldCharacter, GameObject newCharacter)
    {
        var bindings = timeLine.playableAsset.outputs.GetEnumerator();
        bindings.MoveNext();
        UnityEngine.Object source = bindings.Current.sourceObject;

        timeLine.SetGenericBinding(source, oldCharacter.GetComponent<Animator>());

        bindings.MoveNext();
        source = bindings.Current.sourceObject;
        timeLine.SetGenericBinding(source, newCharacter.GetComponent<Animator>());
        newCharacter.SetActive(true);
        timeLine.Play();
    }
    public void changeCharacter(int arrow_num)
    {
        UISoundManager.PlaySound(UISoundType.翻页, 0.4f);
        if (arrow_num < 2)
        {
            awakeTimeLine_l.Stop();

            int new_character_left_id = (arrow_num == 0) ? (character_left_id + 3) % 4 : (character_left_id + 1) % 4;
            for (int i = 0; i < character_left.Count; i++)
            {
                if (i != new_character_left_id && i != character_left_id)
                {
                    character_left[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    character_left[i].SetActive(false);
                }
            }
            if (!left_in_skill_page && !left_select_complete)
            {
                changeCharacterTimeLine_l.Stop();
                changeCharacterTimeLine_l_toleft.Stop();
                
                if (arrow_num == 1)
                {
                    PlayChangeCharacterTimeLine(changeCharacterTimeLine_l, character_left[character_left_id], character_left[new_character_left_id]);
                }
                else
                {
                    PlayChangeCharacterTimeLine(changeCharacterTimeLine_l_toleft, character_left[character_left_id], character_left[new_character_left_id]);
                }
                CharacterColorInfo colorInfo_old = ConfigReader.queryColorByCharacter(character_id2ChiName[character_left_id]);
                CharacterColorInfo colorInfo_new = ConfigReader.queryColorByCharacter(character_id2ChiName[new_character_left_id]);
                DOVirtual.Color(colorInfo_old.selectCharacterColor1, colorInfo_new.selectCharacterColor1, 0.183f, c => { toggle_left.GetComponent<Image>().color = c; });

                arrow_left_1.transform.DOScale(1.175f, 0.05f).OnComplete(() =>
                {
                    arrow_left_1.transform.DOScale(1f, 0.167f);
                    DOVirtual.Color(colorInfo_old.selectCharacterColor1, colorInfo_new.selectCharacterColor1, 0.167f, c => { arrow_left_1.GetComponent<Image>().color = c; });
                });
                arrow_right_1.transform.DOScale(1.175f, 0.05f).OnComplete(() =>
                {
                    arrow_right_1.transform.DOScale(1f, 0.167f);
                    DOVirtual.Color(colorInfo_old.selectCharacterColor1, colorInfo_new.selectCharacterColor1, 0.167f, c => { arrow_right_1.GetComponent<Image>().color = c; });
                });
                var main = particle_left.main;
                main.startColor = new MinMaxGradient(colorInfo_new.selectCharacterColor2, colorInfo_new.selectCharacterColor1);
                character_left_id = new_character_left_id;
            }
        }
        else
        {
            if (right_in_skill_page || right_select_complete) return;
            changeCharacterTimeLine_r.Stop();
            changeCharacterTimeLine_r_toright.Stop();
            awakeTimeLine_r.Stop();

            int new_character_right_id = (arrow_num == 2) ? (character_right_id + 3) % 4 : (character_right_id + 1) % 4;
            for (int i = 0; i < character_left.Count; i++)
            {
                if (i != new_character_right_id && i != character_right_id)
                {
                    character_right[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    character_right[i].SetActive(false);
                }
            }
            if (arrow_num == 3)
            {
                PlayChangeCharacterTimeLine(changeCharacterTimeLine_r, character_right[character_right_id], character_right[new_character_right_id]);
            }
            else
            {
                PlayChangeCharacterTimeLine(changeCharacterTimeLine_r_toright, character_right[character_right_id], character_right[new_character_right_id]);

            }
            CharacterColorInfo colorInfo_old = ConfigReader.queryColorByCharacter(character_id2ChiName[character_right_id]);
            CharacterColorInfo colorInfo_new = ConfigReader.queryColorByCharacter(character_id2ChiName[new_character_right_id]);
            DOVirtual.Color(colorInfo_old.selectCharacterColor1, colorInfo_new.selectCharacterColor1, 0.183f, c => { toggle_right.GetComponent<Image>().color = c; });
            arrow_left_0.transform.DOScale(1.175f, 0.05f).OnComplete(() =>
            {
                arrow_left_0.transform.DOScale(1f, 0.167f);
                DOVirtual.Color(colorInfo_old.selectCharacterColor1, colorInfo_new.selectCharacterColor1, 0.167f, c => { arrow_left_0.GetComponent<Image>().color = c; });
            });
            arrow_right_0.transform.DOScale(1.175f, 0.05f).OnComplete(() =>
            {
                arrow_right_0.transform.DOScale(1f, 0.167f);
                DOVirtual.Color(colorInfo_old.selectCharacterColor1, colorInfo_new.selectCharacterColor1, 0.167f, c => { arrow_right_0.GetComponent<Image>().color = c; });
            });
            var main = particle_right.main;
            main.startColor = new MinMaxGradient(colorInfo_new.selectCharacterColor2, colorInfo_new.selectCharacterColor1);
            character_right_id = new_character_right_id;
        }
    }
    void ChangeSkillButton(GameObject root, int playerId)
    {
        CharacterColorInfo ColorInfo = ConfigReader.queryColorByCharacter(character_id2ChiName[playerId]);
        root.transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>(ColorInfo.passiveSkillPath);
        root.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(ColorInfo.activeSkillPath);
        root.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(ColorInfo.uniqueSkillPath);
    }
    void ChangeSkillText(GameObject u, GameObject a, GameObject p, int playerId)
    {
        Character_Text_Category ch;

        Enum.TryParse(character_id2ChiName[playerId] + "_绝招", out ch);
        u.GetComponent<Text>().text = LocalizationManager.GetText(new TextQueryKey(ch), GlobalGameManager.systemLanguage);

        Enum.TryParse(character_id2ChiName[playerId] + "_主动", out ch);
        a.GetComponent<Text>().text = LocalizationManager.GetText(new TextQueryKey(ch), GlobalGameManager.systemLanguage);

        Enum.TryParse(character_id2ChiName[playerId] + "_被动", out ch);
        p.GetComponent<Text>().text = LocalizationManager.GetText(new TextQueryKey(ch), GlobalGameManager.systemLanguage);
    }
    public void changeSkillPage(int page_num)
    {
        //awakeTimeLine.Stop();
        if (page_num == 1)
        {
            UISoundManager.PlaySound(UISoundType.杏仁, 0.6f);
            if (left_in_skill_page)
            {
                returnCharacterPage(page_num);
                return;
            }
            left_in_skill_page = true;
            if (changeSkillPage_left_reverse != null) StopCoroutine(changeSkillPage_left_reverse);
            //设置技能图标，名字，颜色
            Character_Text_Category ch;
            Enum.TryParse(character_id2ChiName[character_left_id], out ch);
            text_left.GetComponent<Text>().text = LocalizationManager.GetText(new TextQueryKey(ch), GlobalGameManager.systemLanguage);
            
            text_left.transform.GetComponentInParent<Image>().color = ConfigReader.queryColorByCharacter(character_id2ChiName[character_left_id]).selectCharacterColor1;
            ChangeSkillText(unique_text_left, active_text_left, passive_text_left, character_left_id);
            ChangeSkillButton(skill_left, character_left_id);
            //控制其他角色的轨道不播放
            for (int i = 0; i < character_left.Count; i++)
            {
                if (i != character_left_id)
                {
                    character_left[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    character_left[i].SetActive(false);
                }
            }
            skill_left.SetActive(true);
            changeSkillPage_left.Play();
        }
        else
        {
            if (right_in_skill_page)
            {
                returnCharacterPage(page_num);
                return;
            }
            right_in_skill_page = true;
            if (changeSkillPage_right_reverse != null) StopCoroutine(changeSkillPage_right_reverse);
            //设置技能图标，名字，颜色
            Character_Text_Category ch;
            Enum.TryParse(character_id2ChiName[character_right_id], out ch);
            text_right.GetComponent<Text>().text = LocalizationManager.GetText(new TextQueryKey(ch), GlobalGameManager.systemLanguage);

            text_right.transform.GetComponentInParent<Image>().color = ConfigReader.queryColorByCharacter(character_id2ChiName[character_right_id]).selectCharacterColor1;
            ChangeSkillText(unique_text_right, active_text_right, passive_text_right, character_right_id);
            ChangeSkillButton(skill_right, character_right_id);
            //控制其他角色的轨道不播放
            for (int i = 0; i < character_right.Count; i++)
            {
                if (i != character_right_id)
                {
                    character_right[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    character_right[i].SetActive(false);
                }
            }
            skill_right.SetActive(true);
            changeSkillPage_right.Play();
        }
    }
    void returnCharacterPage(int page_num)
    {
        
        if (page_num == 1)
        {
            awakeTimeLine_l.Stop();
            if (!left_in_skill_page) return;
            left_in_skill_page = false;
            skill_left.SetActive(false);
            changeSkillPage_left.Stop();
            changeSkillPage_left_reverse = StartCoroutine(Reverse(changeSkillPage_left));
        }
        else
        {
            awakeTimeLine_r.Stop();
            if (!right_in_skill_page) return;
            right_in_skill_page = false;
            skill_right.SetActive(false);
            changeSkillPage_right.Stop();
            changeSkillPage_right_reverse = StartCoroutine(Reverse(changeSkillPage_right));
        }
    }

    public void SelectCharacter(Toggle toggle)
    {
        
        if (toggle.name == "left")
        {
            if(character_left_id == character_right_id && right_select_complete)
            {
                toggle.isOn = false;
                toggle_left.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                return;
            }
            if (toggle.isOn)
            {
                
                toggle_left.GetComponent<Image>().sprite = select_on;
                left_select_complete = true;
                arrow_left_1.SetActive(false);
                arrow_right_1.SetActive(false);
                UISoundManager.PlaySound(UISoundType.强劲木头声, 0.6f);
            }
            else
            {
                toggle_left.GetComponent<Image>().sprite = select_off;
                left_select_complete = false;
                arrow_left_1.SetActive(true);
                arrow_right_1.SetActive(true);
                UISoundManager.PlaySound(UISoundType.退出木头声, 0.4f);
            }
                
        }
        if (toggle.name == "right")
        {
            if (character_left_id == character_right_id && left_select_complete)
            {
                toggle.isOn = false;
                toggle_right.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                return;
            }
            if (toggle.isOn)
            {
                
                toggle_right.GetComponent<Image>().sprite = select_on;
                right_select_complete = true;
                arrow_left_0.SetActive(false);
                arrow_right_0.SetActive(false);
                UISoundManager.PlaySound(UISoundType.强劲木头声, 0.6f);
            }
            else
            {
                toggle_right.GetComponent<Image>().sprite = select_off;
                right_select_complete = false;
                arrow_left_0.SetActive(true);
                arrow_right_0.SetActive(true);
                UISoundManager.PlaySound(UISoundType.退出木头声, 0.4f);
            }
                
        }
    }
    private IEnumerator Reverse(PlayableDirector playable)
    {
        float dt = (float)playable.duration;

        while (dt > 0)
        {
            dt -= Time.deltaTime / (float)playable.duration;

            playable.time = Mathf.Max(dt, 0);
            playable.Evaluate();
            yield return null;
        }
    }
}
