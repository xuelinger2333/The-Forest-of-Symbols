using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TagChooseController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Icons_firstPage; 
    public PlayableDirector awakeTimeLine; //timeline_初始
    public PlayableDirector changeIconTimeLine; //timeline_改变按钮
    public PlayableDirector changeToFirstPage; //timeline_二级转一级
    public PlayableDirector enterNextTimeLine; //time_line血迹进入
    public Text IconTitle; //说明面板-标题
    public Text IconDesciption; //说明面板-说明
    public GameObject Icons; //预制体
    [SerializeField]
    public List<Sprite> IconSprite; //以4*id + type为单位的Sprite列表，供左侧说明面板使用
    [SerializeField]
    public List<GameObject> IconParent; //需要生成预制体(toggle)排的父物体集合
    [SerializeField]
    GameObject newIcon; //将要显示的新按钮
    [SerializeField]
    GameObject oldIcon; //当前左侧显示的按钮
    bool isInChoosingMap = true; //记录是否在二级界面
    int oldTag = 0, oldType = 0; //记录当前的按钮ID和按钮种类
    Map_Text_Category CurrentDescription;
    Coroutine reverseFirstToSecond; //二级界面倒放协程
    List<string> toggle_id2Name; //将各个按钮的id转换为真正地块的字符串
    void Awake()
    {
        toggle_id2Name = new List<string>() { "水洼", "荆棘", "金枝", "灌木", "毒菇", "象征之森", "障碍物", "乌鸦", "萤火虫", "毒蛇", "九色鹿" };
        ConfigReader.prefabMap = "";
        for (int i = 0; i < toggle_id2Name.Count; i++)
        {
            ConfigReader.tag[toggle_id2Name[i]] = 0;
        }
        Icons_firstPage.SetActive(false);
        Sequence awake = DOTween.Sequence();
        awake.AppendInterval(0.1f);
        awake.OnComplete(() => { awakeTimeLine.Play(); });
        List<string> tagName = new List<string>() { "水洼", "荆棘", "金枝", "灌木", "毒菇", "象征之林", "遗迹", "乌鸦", "萤火虫", "毒蛇", "九色鹿" };
        for (int i = 0; i < IconParent.Count; i++)
        {
            GameObject instance = Instantiate(Icons);
            instance.GetComponent<TagButtonParent>().controller = this;
            instance.GetComponent<TagButtonParent>().SetButtonId(i);
            instance.GetComponent<TagButtonParent>().SetToggleGroup(IconParent[i].GetComponent<ToggleGroup>());
            instance.GetComponent<TagButtonParent>().SetTextTag(tagName[i]);
            instance.transform.SetParent(IconParent[i].transform, false);
        }

    }
    private void Start()
    {
        IconDesciption.text = LocalizationManager.GetText(new TextQueryKey(Map_Text_Category.水洼tag0_内容), GlobalGameManager.systemLanguage);
        IconTitle.text = LocalizationManager.GetText(new TextQueryKey(Map_Text_Category.水洼tag0_标题), GlobalGameManager.systemLanguage);        
    }
    public void NewlySelectedTag(TagChooseButton button)
    {
        UISoundManager.PlaySound(UISoundType.杏仁, 0.5f);
        if (button.ButtonId != oldTag || button.TypeId != oldType)
        {
            ConfigReader.tag[toggle_id2Name[button.ButtonId]] = button.TypeId;
            changeIconTimeLine.Stop();
            newIcon.GetComponent<Image>().sprite = IconSprite[button.ButtonId * 4 + button.TypeId];
            oldIcon.GetComponent<Image>().sprite = IconSprite[oldTag * 4 + oldType];
            changeIconTimeLine.Play();
            
            oldTag = button.ButtonId; oldType = button.TypeId;
            CurrentDescription = button.tag_Description;
        }
    }
    void ResetFirstIconPage()
    {
        for (int i = 0; i < toggle_id2Name.Count; i++)
        {
            int tag = ConfigReader.tag[toggle_id2Name[i]];
            int Sprite_id = tag + 4 * i;
            Icons_firstPage.transform.GetChild(i).GetComponent<Image>().sprite = IconSprite[Sprite_id];
        }
    }
    void OnEnable()
    {
        changeIconTimeLine.stopped += OnPlayableDirectorStopped;
    }
    void OnDisable()
    {
        changeIconTimeLine.stopped -= OnPlayableDirectorStopped;
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (changeIconTimeLine == aDirector)
        {
            oldIcon.GetComponent<Image>().sprite = IconSprite[oldTag * 4 + oldType];

            IconDesciption.text = LocalizationManager.GetText(new TextQueryKey(CurrentDescription), GlobalGameManager.systemLanguage);
            IconTitle.text = LocalizationManager.GetText(new TextQueryKey(CurrentDescription + 1), GlobalGameManager.systemLanguage);
        }
    }
    public void GoToNextPage()
    {
        UISoundManager.PlaySound(UISoundType.木头加植物生长, 0.7f);
        ConfigReader.SaveBattleInfo();
        GlobalGameManager.Instance.LoadScenebyName("SetBattleModeScene");
    }
    public void ReturnToLastPage(bool canReturnToLastScene)
    {
        UISoundManager.PlaySound(UISoundType.退出木头声, 0.5f);
        if (isInChoosingMap)
        {
            isInChoosingMap = false;
            Icons_firstPage.SetActive(true);
            ResetFirstIconPage();

            if (reverseFirstToSecond != null) StopCoroutine(reverseFirstToSecond);
            changeToFirstPage.Play();
            enterNextTimeLine.Stop();
        }
        else
        {
            if (canReturnToLastScene)
                GlobalGameManager.Instance.LoadScenebyName("ChooseMapScene");
            else
            {
                changeToFirstPage.Stop();
                reverseFirstToSecond = StartCoroutine(Reverse(changeToFirstPage));
                enterNextTimeLine.Play();
                isInChoosingMap = true;
            }
        }
    }

    private IEnumerator Reverse(PlayableDirector chooseTimeline)
    {
        float dt = (float)chooseTimeline.duration;

        while (dt > 0)
        {
            dt -= Time.deltaTime / (float)chooseTimeline.duration;

            chooseTimeline.time = Mathf.Max(dt, 0);
            chooseTimeline.Evaluate();
            yield return null;
        }
        Icons_firstPage.SetActive(false);
    }
}
