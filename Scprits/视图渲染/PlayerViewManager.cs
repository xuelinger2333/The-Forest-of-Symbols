using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewManager : MonoBehaviour
{
    //对管理的 Player View Adapter提供“统一”处理的接口
    //禁止对某个特定adapter做特殊处理！
    [SerializeField] List<PlayerViewAdapter> ViewAdapters;
    [SerializeField] Material CommonMaterial;
    MaterialState CurrentState = MaterialState.Common;
    private enum MaterialState
    {
        Common,
        Changed,
        ChangedAndUnique
    }  

    public void ChangeMaterial(Material targetMaterial, bool CanBeChangeToOthers = true)
    {
        //如果现在正在不可打断状态，则返回
        if (CurrentState == MaterialState.ChangedAndUnique) return;
        //根据传入参数，设置状态
        if (CanBeChangeToOthers) CurrentState = MaterialState.Changed;
        else CurrentState = MaterialState.ChangedAndUnique;
        for (int i = 0; i < ViewAdapters.Count; i++)
        {
            ViewAdapters[i].sr.material = targetMaterial;
        }
    }

    public void ResetMaterial()
    {
        //恢复默认Material设定
        CurrentState = MaterialState.Common;
        for (int i = 0; i < ViewAdapters.Count; i++)
        {
            ViewAdapters[i].sr.material = CommonMaterial;
        }
    }

    public List<GameObject> GetGameObject()
    {
        //获取所有ViewAdapter gameobject的函数，禁止滥用
        List<GameObject> res = new List<GameObject>();
        for (int i = 0; i < ViewAdapters.Count; i++)
        {
            res.Add(ViewAdapters[i].gameObject);
        }
        return res;
    }

    public void StopAllAnimation()
    {
        for (int i = 0; i < ViewAdapters.Count; i++)
        {
            ViewAdapters[i].ani.enabled = false;
        }
    }

    public void ChangeSpriteColor(Color c)
    {
        for (int i = 0; i < ViewAdapters.Count; i++)
        {
            ViewAdapters[i].sr.color = c;
        }
    }
}
