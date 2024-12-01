using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HintInfo
{
    public Sprite sprite;
    public Color edgeColor;
}
public class CreatureHintController : MonoBehaviour
{
    int cur_id;
    public GameObject Spread;
    public Image SpreadColor;
    public List<HintInfo> creatureHints;
    Dictionary<string, int> hintSearchDictionary = new Dictionary<string, int>();
    // Start is called before the first frame update
    void Awake()
    {
        hintSearchDictionary["乌鸦"] = 0;
        hintSearchDictionary["九色鹿"] = 1;
        hintSearchDictionary["毒蛇"] = 2;
        hintSearchDictionary["萤火虫"] = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetTarget(string creatureName)
    {
        if (!hintSearchDictionary.ContainsKey(creatureName)) return;
        cur_id = hintSearchDictionary[creatureName];
        GetComponent<Image>().sprite = creatureHints[cur_id].sprite;
        Sequence hintColorChange = DOTween.Sequence();
        SpreadColor.color = creatureHints[cur_id].edgeColor;
        hintColorChange.AppendInterval(0.5666f);
        hintColorChange.AppendCallback(StartSpread);
        Color transparent = new Color(creatureHints[cur_id].edgeColor.r, creatureHints[cur_id].edgeColor.g, creatureHints[cur_id].edgeColor.b, 0);
        hintColorChange.Append(DOVirtual.Color(creatureHints[cur_id].edgeColor, transparent, 0.4333f, c => { SpreadColor.color = c; }));

    }
    public void StartSpread()
    {
        Spread.SetActive(true);
        Spread.transform.DOScale(1.5f, 0.4333f);
    }
    public void Interrupt()
    {
        DestroyMe();
    }
    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
