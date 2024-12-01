using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum IconType
{
    ��ż����,
    ��ż����,
    �ж�,
    ҽ������,
    ���ͱ���,
    ħŮ�з�,
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
    /// ��������ֱ���ͼ�����ʱ�䣨��ϣ�����ô����ֶ�ȡ������1001)��ͼ�꣬��ָ������Լ�ͼ��㼶
    /// </summary>
    /// <param name="time"></param>
    /// <param name="type"></param>
    /// <param name="playerID"></param>
    /// <param name="index"></param>
    public void PlayIcon(float time,IconType type,bool playerID,int index = 0)
    {
        if (playerID)
        {
            //Debug.Log("��������ж�");
            if (playingObjsLeft.ContainsKey(type))
            {
                //Debug.Log("���������ڲ���");
                StartIcon(playingObjsLeft[type], index, playerID, time, type);
            }
            else if (freeObjsLeft.Count == 0)
            {
                //Debug.Log("�������û�п��ж���");
                CreatObj(playerID);
                playingObjsLeft.Add(type, freeObjsLeft[0]);
                StartIcon(freeObjsLeft[0], index, playerID, time, type);
                freeObjsLeft.RemoveAt(0);
            }
            else
            {
                //Debug.Log("���������ÿ���");
                playingObjsLeft.Add(type, freeObjsLeft[0]);
                StartIcon(freeObjsLeft[0], index, playerID, time, type);
                freeObjsLeft.RemoveAt(0);
            }
        }
        else
        {
            //Debug.Log("�����Ҳ���ж�");
            if (playingObjsRight.ContainsKey(type))
            {
                //Debug.Log("�����Ҳ����ڲ���");
                StartIcon(playingObjsRight[type], index, playerID, time, type);
            }
            else if (freeObjsRight.Count == 0)
            {
                //Debug.Log("�����Ҳ�û�п��ж���");
                CreatObj(playerID);
                playingObjsRight.Add(type, freeObjsRight[0]);
                StartIcon(freeObjsRight[0], index, playerID, time, type);
                freeObjsRight.RemoveAt(0);
            }
            else
            {
                //Debug.Log("�����Ҳ����ÿ���");
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
    /// ʹ�ض�ͼ����ʧ
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
