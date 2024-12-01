using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class BattleModeController : MonoBehaviour
{
    public PlayableDirector awakeTimeLine;
    ToggleGroup toggleGroup;
    public GameObject ModeHint;
    public Toggle oldToggle;
    string gameModestr = "mode1_1";
    void Awake()
    {
        toggleGroup = GetComponent<ToggleGroup>();
        Sequence awake = DOTween.Sequence();
        awake.AppendInterval(0.1f);
        awake.OnComplete(() => { awakeTimeLine.Play(); });
        oldToggle.transform.parent.transform.DOScale(1.1f, 0.2f);
    }
    public void ChooseMode()
    {
        UISoundManager.PlaySound(UISoundType.杏仁, 0.4F);
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
        if (toggle == oldToggle) return;
        UISoundManager.PlaySound(UISoundType.杏仁, 0.6F);
        Vector3 targetPos = new Vector3(toggle.transform.parent.localPosition.x, toggle.transform.parent.localPosition.y - 57, 0);
        ModeHint.transform.DOLocalMove(targetPos, 0.2f).SetEase(Ease.InOutCubic);
        toggle.transform.parent.transform.DOScale(1.1f, 0.2f);
        oldToggle.transform.parent.transform.DOScale(1f, 0.2f);
        oldToggle = toggle;
        gameModestr = toggle.name;
    }
    public void GoToNextPage()
    {
        UISoundManager.PlaySound(UISoundType.木头加剑, 0.6f);
        ConfigReader.battleMode = oldToggle.name;
        ConfigReader.SaveBattleInfo();
        GlobalGameManager.Instance.LoadScenebyName("ChooseCharacterScene");
    }
    public void ReturnToLastPage()
    {
        UISoundManager.PlaySound(UISoundType.退出木头声, 0.6f);
        GlobalGameManager.Instance.LoadScenebyName("ChooseMapScene");
    }
}
