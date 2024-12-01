using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PrefabChooseController : MonoBehaviour
{
    public GameObject Arrow;
    public GameObject Description;
    public List<GameObject> toggleButtonGroup;
    public ToggleGroup toggleGroup;
    public Toggle currentToggle;
    public Toggle emptyToggle;
    public Text Title1;
    public Text Title2;
    public Text Content;
    string prefabMap; //这页输出的唯一变量：选择预制地图的种类
    [SerializeField] PlayableDirector chooseTimeline;
    [SerializeField] PlayableDirector SceneInTimeline;
    [SerializeField] Image BackGround;
    Color gray = new Color(0.084185f, 0.113f, 0.1125198f, 0.75294f);
    Coroutine timeLineReverse;
    bool isInChoosingMap = false;
    int BoardIsFlip = 1;
    // Start is called before the first frame update
    void Awake()
    {
        Sequence awake = DOTween.Sequence();
        awake.AppendInterval(0.1f);
        awake.OnComplete(() => { SceneInTimeline.Play(); });
        currentToggle = emptyToggle;
    }

    void SetText(string type, bool needDesciption)
    {
        Map_Text_Category t1 = Map_Text_Category.Null, t2 = Map_Text_Category.Null, content = Map_Text_Category.Null;       
        if (needDesciption)
        {
            switch (type)
            {
                case "路标_毒菇":
                    content = Map_Text_Category.毒菇_解释;
                    break;
                case "路标_灌木":
                    content = Map_Text_Category.灌木_解释;
                    break;
                case "路标_水洼":
                    content = Map_Text_Category.水洼_解释;
                    break;
                case "路标_荆棘":
                    content = Map_Text_Category.荆棘_解释;
                    break;
                case "路标_金枝":
                    content = Map_Text_Category.金枝_解释;
                    break;
            }
            Content.text = LocalizationManager.GetText(new TextQueryKey(content), GlobalGameManager.systemLanguage);
        }
        else
        {

            switch (type)
            {
                case "路标_毒菇":
                    t1 = Map_Text_Category.毒菇预制_标题1;
                    t2 = Map_Text_Category.毒菇预制_标题2;
                    content = Map_Text_Category.毒菇预制_内容;
                    break;
                case "路标_灌木":
                    t1 = Map_Text_Category.灌木预制_标题1;
                    t2 = Map_Text_Category.灌木预制_标题2;
                    content = Map_Text_Category.灌木预制_内容;
                    break;
                case "路标_水洼":
                    t1 = Map_Text_Category.水洼预制_标题1;
                    t2 = Map_Text_Category.水洼预制_标题2;
                    content = Map_Text_Category.水洼预制_内容;
                    break;
                case "路标_荆棘":
                    t1 = Map_Text_Category.荆棘预制_标题1;
                    t2 = Map_Text_Category.荆棘预制_标题2;
                    content = Map_Text_Category.荆棘预制_内容;
                    break;
                case "路标_金枝":
                    t1 = Map_Text_Category.金枝预制_标题1;
                    t2 = Map_Text_Category.金枝预制_标题2;
                    content = Map_Text_Category.金枝预制_内容;
                    break;
            }
            Title1.text = LocalizationManager.GetText(new TextQueryKey(t1), GlobalGameManager.systemLanguage);
            Title2.text = LocalizationManager.GetText(new TextQueryKey(t2), GlobalGameManager.systemLanguage);
            Content.text = LocalizationManager.GetText(new TextQueryKey(content), GlobalGameManager.systemLanguage);
        }


    }
    public void GoToNextPage()
    {
        ConfigReader.prefabMap = prefabMap;
        ConfigReader.SaveBattleInfo();
        GlobalGameManager.Instance.LoadScenebyName("SetBattleModeScene");
        UISoundManager.PlaySound(UISoundType.木头加植物生长, 0.7f);
    }
    public void ReturnToLastPage()
    {
        UISoundManager.PlaySound(UISoundType.退出木头声, 0.5f);
        if (isInChoosingMap)
        {
            isInChoosingMap = false;

            chooseTimeline.Stop();
            timeLineReverse = StartCoroutine(Reverse());
            
            DOVirtual.Color(BackGround.color, new Color(0.084185f, 0.113f, 0.1125198f, 0f), 0.167f, c => { BackGround.color = c; });
            for (int i = 0; i < toggleButtonGroup.Count; i++)
            {
                if (toggleButtonGroup[i].name != currentToggle.name)
                {
                    GameObject otherToggle = toggleButtonGroup[i];
                    if (otherToggle.GetComponent<Image>().color == new Color(0.6f, 0.6f, 0.6f, 1))
                        DOVirtual.Color(otherToggle.GetComponent<Image>().color, new Color(1f, 1f, 1f, 1f), 0.167f, c => { otherToggle.GetComponent<Image>().color = c; });
                }
                if (toggleButtonGroup[i].name == currentToggle.name)
                {
                    GameObject curToggle = toggleButtonGroup[i];
                    curToggle.transform.DORotate(new Vector3(0, 0, 0f), 0.167f);
                }
            }
            emptyToggle.isOn = true;
            currentToggle = emptyToggle;
        }
        else
        {
            GlobalGameManager.Instance.LoadScenebyName("ChooseMapScene");
        }
    }
    private IEnumerator Reverse()
    {
        float dt = (float)chooseTimeline.duration;

        while (dt > 0)
        {
            dt -= Time.deltaTime / (float)chooseTimeline.duration;

            chooseTimeline.time = Mathf.Max(dt, 0);
            chooseTimeline.Evaluate();
            yield return null;
        }
    }
    
    public void SwitchBoard()
    {
        BoardIsFlip *= -1;
        Description.transform.Find("路牌").DOScaleX(BoardIsFlip, 0.2f).SetEase(Ease.InOutCirc);
        bool f = (BoardIsFlip == -1) ? true : false;
        SetText(currentToggle.name, f);
        UISoundManager.PlaySound(UISoundType.木头声, 0.45f);
    }
    public void CallPageTimeLineByToggle()
    {
        if (BoardIsFlip == -1)
        {
            SwitchBoard();
        }
        SceneInTimeline.Stop();
        Toggle toggle1 = toggleGroup.ActiveToggles().FirstOrDefault();
        if (toggle1 == currentToggle || toggle1 == emptyToggle) return;
        else
        {
            chooseTimeline.Stop();
            if (timeLineReverse != null) StopCoroutine(timeLineReverse); 
            if (!isInChoosingMap) isInChoosingMap = true;
            //if (!Particle.activeSelf)
            //{
            //    Particle.SetActive(true);
            //    Particle.GetComponent<ParticleSystem>().Play();
            //}
            if (BackGround.color != gray)
            {
                DOVirtual.Color(BackGround.color, gray, 0.167f, c => { BackGround.color = c; });
            }
            Arrow.SetActive(true);
            Description.SetActive(true);
            for (int i = 0; i < toggleButtonGroup.Count; i++)
            {
                if (toggleButtonGroup[i].name != toggle1.name)
                {
                    GameObject otherToggle = toggleButtonGroup[i];
                    if (otherToggle.GetComponent<Image>().color != new Color(0.6f, 0.6f, 0.6f, 1))
                    DOVirtual.Color(otherToggle.GetComponent<Image>().color, new Color(0.6f, 0.6f, 0.6f, 1f), 0.167f, c => { otherToggle.GetComponent<Image>().color = c; });
                }
                if (toggleButtonGroup[i].name == toggle1.name)
                {
                    GameObject curToggle = toggleButtonGroup[i];
                    curToggle.transform.DORotate(new Vector3(0, 0, 21.25f), 0.167f);
                    curToggle.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    Arrow.transform.localPosition = new Vector3(-94 + curToggle.transform.localPosition.x , -5 + curToggle.transform.localPosition.y, -20);
                    //Particle.transform.localPosition = curToggle.transform.localPosition;
                }
                if (toggleButtonGroup[i].name == currentToggle.name)
                {
                    GameObject oldToggle = toggleButtonGroup[i];
                    oldToggle.transform.DORotate(new Vector3(0, 0, 0f), 0.167f);
                }
            }

            chooseTimeline.Play();
            UISoundManager.PlaySound(UISoundType.木头声, 0.45f);
            SetText(toggle1.name, false);
            //prefabMap = Name下划线后的字
            int underscoreIndex = toggle1.name.IndexOf('_');

            // 如果找到了下划线，并且它不是最后一个字符
            if (underscoreIndex != -1 && underscoreIndex < toggle1.name.Length - 1)
            {
                // 截取下划线后面的子串
                prefabMap =  toggle1.name.Substring(underscoreIndex + 1);
            }
            currentToggle = toggle1;
        }
    }
}
