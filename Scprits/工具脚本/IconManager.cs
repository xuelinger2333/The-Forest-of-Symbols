using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum IconType
{
    人偶主动,
    人偶绝技,
    中毒,
    医生咳嗽,
    骇客被动,
    魔女敌方,
}
public class IconManager : MonoBehaviour
{
    public static IconManager instance;
    public List<IconSO> icons;
    public GameObject prefab;
    private List<GameObject> freeObjsLeft = new List<GameObject>();
    private Dictionary<IconType,GameObject> playingObjsLeft = new Dictionary<IconType,GameObject>();
    private List<GameObject> freeObjsRight = new List<GameObject>();
    private Dictionary<IconType, GameObject> playingObjsRight = new Dictionary<IconType, GameObject>();
    public Transform parentLeft;
    public Transform parentRight;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void CreatObj(bool playerID)
    {
        GameObject obj = Instantiate(prefab);
        if (playerID)
        {
            freeObjsLeft.Add(obj);
            obj.transform.SetParent(parentLeft, false);
        }
        else
        {
            freeObjsRight.Add(obj);
            obj.transform.SetParent(parentRight, false);
        }
    }
    /// <summary>
    /// 传入参数分别是图标存在时间（若希望永久存在手动取消则填1001)，图标，所指向玩家以及图标层级
    /// </summary>
    /// <param name="time"></param>
    /// <param name="type"></param>
    /// <param name="playerID"></param>
    /// <param name="index"></param>
    public void PlayIcon(float time,IconType type,bool playerID,int index = 0)
    {
        if (playerID)
        {
            //Debug.Log("进入左侧判断");
            if (playingObjsLeft.ContainsKey(type))
            {
                //Debug.Log("进入左侧存在播放");
                StartIcon(playingObjsLeft[type], index, playerID, time, type);
            }
            else if (freeObjsLeft.Count == 0)
            {
                //Debug.Log("进入左侧没有空闲对象");
                CreatObj(playerID);
                playingObjsLeft.Add(type, freeObjsLeft[0]);
                StartIcon(freeObjsLeft[0], index, playerID, time, type);
                freeObjsLeft.RemoveAt(0);
            }
            else
            {
                //Debug.Log("进入左侧调用空闲");
                playingObjsLeft.Add(type, freeObjsLeft[0]);
                StartIcon(freeObjsLeft[0], index, playerID, time, type);
                freeObjsLeft.RemoveAt(0);
            }
        }
        else
        {
            //Debug.Log("进入右侧侧判断");
            if (playingObjsRight.ContainsKey(type))
            {
                //Debug.Log("进入右侧侧存在播放");
                StartIcon(playingObjsRight[type], index, playerID, time, type);
            }
            else if (freeObjsRight.Count == 0)
            {
                //Debug.Log("进入右侧没有空闲对象");
                CreatObj(playerID);
                playingObjsRight.Add(type, freeObjsRight[0]);
                StartIcon(freeObjsRight[0], index, playerID, time, type);
                freeObjsRight.RemoveAt(0);
            }
            else
            {
                //Debug.Log("进入右侧侧调用空闲");
                playingObjsRight.Add(type, freeObjsRight[0]);
                StartIcon(freeObjsRight[0], index, playerID, time, type);
                freeObjsRight.RemoveAt(0);
            }
        }
    }

    private void StartIcon(GameObject obj, int _index, bool _playerID, float _time, IconType _type)
    {
        obj.GetComponent<Icon>().Set(_index,_playerID,_time,_type);
        obj.SetActive(true);
    }
    /// <summary>
    /// 使特定图标消失
    /// </summary>
    /// <param name="_playerID"></param>
    /// <param name="type"></param>
    public void StopIcon(bool _playerID, IconType type)
    {
        if (_playerID)
        {
            if(playingObjsLeft.ContainsKey(type))
            {
                playingObjsLeft[type].SetActive(false);
            }
        }
        else
        {
            if(playingObjsRight.ContainsKey(type))
            {
                playingObjsRight[type].SetActive(false);
            }
        }
    }
    public void AddToPlayingList(GameObject gameObject,IconType type,bool playerID)
    {
        if(playerID)
        {
            freeObjsLeft.Add(gameObject);
            playingObjsLeft.Remove(type);
        }
        else
        {
            freeObjsRight.Add(gameObject);
            playingObjsRight.Remove(type);
        }
    }
}
