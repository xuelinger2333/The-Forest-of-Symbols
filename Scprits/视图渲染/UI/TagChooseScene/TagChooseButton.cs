using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagChooseButton : MonoBehaviour
{
    Toggle toggle;
    public string ButtonContent;
    public Map_Text_Category tag_Description;
    public int ButtonId;
    public int TypeId;
    public GameObject choose;
    public GameObject tick;
    public void Awake()
    {
        toggle = GetComponent<Toggle>();
    }
    public void OnTagTogglePressed()
    {
        if (toggle.isOn)
        {
            transform.parent.GetComponent<TagButtonParent>().controller.NewlySelectedTag(this);
            Sequence chooseS = DOTween.Sequence();
            choose.transform.localPosition = this.transform.localPosition;
            chooseS.Append(DOVirtual.Color(new Color(0, 0, 0, 0), new Color(0, 0, 0, 0.5F), 0.033f, c => { choose.GetComponent<Image>().color = c; }));
            chooseS.Append(DOVirtual.Color(new Color(0, 0, 0, 0.5f), new Color(0, 0, 0, 0.78F), 0.05f, c => { choose.GetComponent<Image>().color = c; }));
            Sequence tickS = DOTween.Sequence();
            tickS.AppendInterval(0.05f);
            tickS.Append(DOVirtual.Color(new Color(1, 1, 1, 0), new Color(1, 1, 1, 0.5F), 0.017f, c => { tick.GetComponent<Image>().color = c; }));
            tickS.Append(DOVirtual.Color(new Color(1, 1, 1, 0.5f), new Color(1, 1, 1, 1F), 0.05f, c => { tick.GetComponent<Image>().color = c; }));
        }
    }
}
