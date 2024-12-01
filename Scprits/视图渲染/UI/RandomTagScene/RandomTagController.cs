using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using UnityEngine.Playables;
using System;
using UnityEngine.UI;

public class RandomTagController : MonoBehaviour
{
    public PlayableDirector SceneInTimeline1;
    public PlayableDirector SceneInTimeline2;
    public GameObject Icons_firstPage;
    [SerializeField]
    public List<Sprite> IconSprite;
    List<string> toggle_id2Name; //将各个按钮的id转换为真正地块的字符串
    // Start is called before the first frame update
    void Awake()
    {
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
        Sequence awake = DOTween.Sequence();
        awake.AppendInterval(0.1f);
        awake.OnComplete(() => { SceneInTimeline1.Play(); SceneInTimeline2.Play(); });
        toggle_id2Name = new List<string>() { "水洼", "荆棘", "金枝", "灌木", "毒菇", "象征之森", "障碍物", "乌鸦", "萤火虫", "毒蛇", "九色鹿" };
        ConfigReader.prefabMap = "";
        for (int i = 0; i < toggle_id2Name.Count; i++)
        {
            int tag = 0;
            if (toggle_id2Name[i] != "毒蛇" && toggle_id2Name[i] != "九色鹿")
                tag = UnityEngine.Random.Range(0, 4);
            else
                tag = UnityEngine.Random.Range(0, 2);
            ConfigReader.tag[toggle_id2Name[i]] = tag;
            
        }
        ResetFirstIconPage();
        
    }
    void ResetFirstIconPage()
    {
        for (int i = 0; i < toggle_id2Name.Count; i++)
        {
            int tag = ConfigReader.tag[toggle_id2Name[i]];
            int Sprite_id = tag + 4 * i;
            Sprite s = IconSprite[Sprite_id];
            Icons_firstPage.transform.GetChild(i).GetComponent<Image>().sprite = s;
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void GoToNextPage()
    {
        UISoundManager.PlaySound(UISoundType.木头加植物生长, 0.7f);
        ConfigReader.prefabMap = "";
        //ConfigReader.SaveBattleInfo();

        GlobalGameManager.Instance.LoadScenebyName("SetBattleModeScene");
    }
    public void ReturnToLastPage()
    {
        UISoundManager.PlaySound(UISoundType.退出木头声, 0.5f);

        GlobalGameManager.Instance.LoadScenebyName("ChooseMapScene");
    }

}
