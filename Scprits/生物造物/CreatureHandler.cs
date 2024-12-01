using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class CreatureHandler : MonoBehaviour
{
    public string CreatureName;
    private static readonly object lockObject = new object(); //锁
    public GameObject creatureView;
    public MapGenerator map = null;
    private List<int> indexPool = new List<int>();
    public int creatureNumCap;
    protected Dictionary<int, Creature> creatures = new Dictionary<int, Creature>();
    [HideInInspector]
    public Player player0, player1;
    public Dictionary<int, Player> PlayerDictionary = new Dictionary<int, Player>();
    public GameObject HintParent;
    static GameObject hint_left = null, hint_right = null;
    protected virtual void Awake()
    {
        
        for (int i = 0; i < creatureNumCap; i++)
        {
            indexPool.Add(i);
        }
    }
    protected virtual void Start()
    {
        map = MapGenerator.Instance;
        player0 = GameManager.Instance.player0;
        player1 = GameManager.Instance.player1;
        PlayerDictionary.Add(Convert.ToInt32(player0.playerID), player0);
        PlayerDictionary.Add(Convert.ToInt32(player1.playerID), player1);
    }

    protected virtual void Update()
    {
        List<int> keys = new List<int>(creatures.Keys);

        for (int i = keys.Count - 1; i >= 0; i--)
        {
            int key = keys[i];
            if (creatures.ContainsKey(key))
            {
                creatures[key].Update();
                if (map.mapData[creatures[key].pos_x, creatures[key].pos_y].type == "象征之森")
                {
                    RemoveCreaturebyId(key);
                }
            }
        }
    }
    protected void createUIHint(bool player)
    {
        if (player && hint_left != null)
        {
            Destroy(hint_left); hint_left = null;
        }
        if (!player && hint_right != null)
        {
            Destroy(hint_right); hint_right = null;
        }
        GameObject instance = Instantiate(PrefabControl.Instance.CreatureHintPrefab);
        instance.GetComponent<CreatureHintController>().SetTarget(CreatureName);
        instance.transform.SetParent(HintParent.transform, false);
        if (!player)
        {
            hint_right = instance;
            instance.transform.localPosition = new Vector2(480, 372);
        }
        else
        {
            hint_left = instance;
        }
    }
    protected void AddCreaturebyObject(Creature creature, int CreartureIdtoRemove = 0)
    {
        if (indexPool.Count == 0)
        {
            RemoveCreaturebyId(CreartureIdtoRemove);
        };
        int Id = indexPool[indexPool.Count - 1];
        indexPool.RemoveAt(indexPool.Count - 1);
        creatures.Add(Id, creature);
        creature.creatureId = Id;
        if (creatureView != null) 
        {
            GameObject creatureInstance = Instantiate(creatureView, this.transform);
            CreatureViewAdapter view1 = creatureInstance.GetComponent<CreatureViewAdapter>();
            view1.isInMap2 = false;
            view1.creatureId = Id;
            GameObject creatureInstance_map2 = Instantiate(creatureView, this.transform);
            CreatureViewAdapter view2 = creatureInstance_map2.GetComponent<CreatureViewAdapter>();
            view2.isInMap2 = true;
            view2.creatureId = Id;
            creature.view1 = view1;
            creature.view2 = view2;
        }
    }

    protected virtual void RemoveCreaturebyId(int creatureId)
    {
        indexPool.Add(creatureId);
        creatures[creatureId].Die();
        creatures.Remove(creatureId);           
    }
    protected void RemoveInvalidCreatures()
    {
        List<int> keysToRemove = new List<int>();
        foreach (var key in creatures.Keys)
        {
            if (creatures[key].isTriggered == true)
                keysToRemove.Add(key);
        }

        foreach (var key in keysToRemove)
        {
            RemoveCreaturebyId(creatures[key].creatureId);
        }
    }

    protected virtual void ExecuteEffect(Creature creature){}

    public void HandleCreatureTrigger(int creatureId)
    {
        if (creatureId < creatureNumCap && creatures.ContainsKey(creatureId))
        {
            Creature creature = creatures[creatureId];
            ExecuteEffect(creature);//执行触发操作，例如赋予player效果
        }
    }


    //暴露给View的接口
    public Vector2 QueryCreaturePositionbyId(int creatureId)
    {
        if (creatureId < creatureNumCap && creatures.ContainsKey(creatureId))
        {
            return new Vector2(creatures[creatureId].pos_x, creatures[creatureId].pos_y);
        }
        else return Vector2.zero;
    }
    public Vector2 QueryCreatureUpdatePositionbyId(int creatureId)
    {
        if (creatureId < creatureNumCap && creatures.ContainsKey(creatureId))
        {
            return creatures[creatureId].pos_update;
        }
        else return Vector2.zero;
    }
    public Vector2 QueryCreatureFaceDir(int creatureId)
    {
        if (creatureId < creatureNumCap && creatures.ContainsKey(creatureId))
        {
            return creatures[creatureId].faceDir;
        }
        else return Vector2.zero;
    }

    internal bool QueryCreatureAnimationById(int creatureId, string aniName)
    {
        if (creatureId < creatureNumCap && creatures.ContainsKey(creatureId))
        {
            return creatures[creatureId].aniBoolDictionary[aniName];
        }
        else return false;
    }
    public bool QueryCreatureIdValid(int creatureId)
    {
        if (creatureId < creatureNumCap && creatures.ContainsKey(creatureId))
        {
            return true;
        }
        else return false;
    }

    public KeyValuePair<int, int> QueryMapRotationCounts()
    {
        return new KeyValuePair<int, int>(map.rotationCounts1, map.rotationCounts2);
    }


    internal bool QueryPosIsGround(int x, int y)
    {
        return !map.GetMapData(x, y).isObstacle;
    }
}
