using DG.Tweening;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MapChooseController : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public UnityEngine.UI.Toggle currentToggle;
    public GameObject ChooseHint;
    public GameObject GoAheadHint;
    public Text textHint;
    public Text textHint_child;
    Dictionary<string, Vector3> hintPos;
    Dictionary<string, ParticleSystem> ParticleDic;
    string nextScene = "RandomTagScene";
    [SerializeField] GameObject page_firstDescription;
    [SerializeField] GameObject page_自选;
    [SerializeField] GameObject page_预制;
    [SerializeField] GameObject page_随机;
    [SerializeField] GameObject currentPage;
    [SerializeField] PlayableDirector timeline_预制;
    [SerializeField] PlayableDirector timeline_自选;
    [SerializeField] PlayableDirector timeline_随机;
    [SerializeField] PlayableDirector currentTimeline;
    [SerializeField] ParticleSystem notClick;
    [SerializeField] ParticleSystem Click;
    // Start is called before the first frame update
    void Awake()
    {
        hintPos = new Dictionary<string, Vector3>();
        hintPos["随机"] = new Vector3(-445, 286, -15);
        hintPos["自选"] = new Vector3(-727, 111, -15);
        hintPos["预制"] = new Vector3(-645, -260, -15);

        ParticleDic = new Dictionary<string, ParticleSystem>();
        Sequence s = DOTween.Sequence();
        s.InsertCallback(0.2f, MyCallback);
        //ParticleDic["随机"] = Instantiate(notClick, hintPos["随机"], notClick.transform.rotation);
        //ParticleDic["随机"].transform.SetParent(transform, false);

    }
    void MyCallback()
    {
        ParticleDic["自选"] = Instantiate(notClick, hintPos["自选"], notClick.transform.rotation);
        ParticleDic["自选"].transform.SetParent(transform, false);
        ParticleDic["预制"] = Instantiate(notClick, hintPos["预制"], notClick.transform.rotation);
        ParticleDic["预制"].transform.SetParent(transform, false);
    }
    private void Start()
    {

        if (PlayerPrefs.GetInt("firstGame", 0) == 0)
        {
            Time.timeScale = 0;
            PlayerPrefs.SetInt("firstGame", 1);
            PlayerPrefs.Save();
            page_firstDescription.SetActive(true);
        }
        else
        {
            Sequence awake = DOTween.Sequence();
            awake.AppendInterval(0.1f);
            awake.AppendCallback(PlayAwakeTimeLine);
            page_firstDescription.SetActive(false);
        }         
    }
    public void OnStartButtonClick()
    {
        Time.timeScale = 1;
        PlayAwakeTimeLine();
        page_firstDescription.SetActive(false);
    }
    void PlayAwakeTimeLine()
    {
        currentTimeline.Play();
        ChooseHint.SetActive(true);
        GoAheadHint.SetActive(true);
    }
    // Update is called once per frame

    public void ChangeScene(string SceneName)
    {
        if (SceneName == "next")
        {
            SceneName = nextScene;
            UISoundManager.PlaySound(UISoundType.渐进木头声, 0.5f);
        }
        if (SceneName == "StartScene")
        {
            UISoundManager.PlaySound(UISoundType.退出木头声, 0.5f);
        }
        GlobalGameManager.Instance.LoadScenebyName(SceneName);
    }
    public void CallPageTimeLineByToggle()
    {
        UnityEngine.UI.Toggle toggle1 = toggleGroup.ActiveToggles().FirstOrDefault();
        if (toggle1 == currentToggle) return;
        else
        {
            UISoundManager.PlaySound(UISoundType.纸张, 0.5f);
            switch (toggle1.name)
            {
                case "Toggle_自选":
                    currentPage.SetActive(false);
                    currentTimeline.Stop();
                    timeline_自选.Play();
                    page_自选.SetActive(true);
                    currentTimeline = timeline_自选;
                    currentPage = page_自选;
                    ChooseHint.transform.localPosition = hintPos["自选"];
                    nextScene = "ChooseTagScene";

                    textHint.text = LocalizationManager.GetText(new TextQueryKey(UI_Text_Category.自选tag_标题),GlobalGameManager.systemLanguage);
                    textHint_child.text = LocalizationManager.GetText(new TextQueryKey(UI_Text_Category.自选tag_内容), GlobalGameManager.systemLanguage); ;
                    if (ParticleDic["自选"] != null)
                    {
                        Destroy(ParticleDic["自选"].gameObject);
                        ParticleSystem newParticleSystem = Instantiate(Click, hintPos["自选"], Click.transform.rotation);
                        newParticleSystem.transform.SetParent(transform, false);
                        Destroy(newParticleSystem.gameObject, newParticleSystem.main.duration);
                    }

                    break;
                case "Toggle_随机":
                    currentPage.SetActive(false);
                    currentTimeline.Stop();
                    timeline_随机.Play();
                    page_随机.SetActive(true);
                    currentTimeline = timeline_随机;
                    currentPage = page_随机;
                    ChooseHint.transform.localPosition = hintPos["随机"];
                    nextScene = "RandomTagScene";

                    textHint.text = LocalizationManager.GetText(new TextQueryKey(UI_Text_Category.随机tag_标题), GlobalGameManager.systemLanguage);
                    textHint_child.text = LocalizationManager.GetText(new TextQueryKey(UI_Text_Category.随机tag_内容), GlobalGameManager.systemLanguage);
                    if (ParticleDic["随机"] != null)
                    {
                        Destroy(ParticleDic["随机"].gameObject);
                        ParticleSystem newParticleSystem = Instantiate(Click, hintPos["随机"], Click.transform.rotation);
                        newParticleSystem.transform.SetParent(transform, false);
                        Destroy(newParticleSystem.gameObject, newParticleSystem.main.duration);
                    }
                    break;
                case "Toggle_预制":
                    currentPage.SetActive(false);
                    currentTimeline.Stop();
                    timeline_预制.Play();
                    page_预制.SetActive(true);
                    currentTimeline = timeline_预制;
                    currentPage = page_预制;
                    ChooseHint.transform.localPosition = hintPos["预制"];
                    nextScene = "ChoosePrefabScene";
                    
                    textHint.text = LocalizationManager.GetText(new TextQueryKey(UI_Text_Category.预制tag_标题), GlobalGameManager.systemLanguage);
                    textHint_child.text = LocalizationManager.GetText(new TextQueryKey(UI_Text_Category.预制tag_内容), GlobalGameManager.systemLanguage);
                    if (ParticleDic["预制"] != null)
                    {
                        Destroy(ParticleDic["预制"].gameObject);
                        ParticleSystem newParticleSystem = Instantiate(Click, hintPos["预制"], Click.transform.rotation);
                        newParticleSystem.transform.SetParent(transform, false);
                        Destroy(newParticleSystem.gameObject, newParticleSystem.main.duration);
                    }
                    break;
            }
            ChooseHint.GetComponent<ParticleSystem>().Play();
            GoAheadHint.GetComponent<ParticleSystem>().Play();
            string Name = currentToggle.name[7..];
            ParticleDic[Name] = Instantiate(notClick, hintPos[Name], notClick.transform.rotation);
            ParticleDic[Name].transform.SetParent(transform, false);
            currentToggle = toggle1;
        }
    }
}
