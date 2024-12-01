using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UI;
using System.Reflection;

public static class ConfigReader
{
    //配置在游戏中的存储
    static Dictionary<string, CharacterColorInfo> ColorInfoDic = null;
    static BattleInfo battleInfo = null;

    static List<string> mapName = new List<string>() { "水洼", "荆棘", "金枝", "灌木", "毒菇", "象征之森", "障碍物", "乌鸦", "萤火虫", "毒蛇", "九色鹿" };
    public static string player0Id;
    public static string player1Id;
    public static string prefabMap;
    public static Dictionary<string, int> tag = new Dictionary<string, int>();
    public static string battleMode;
public static void SaveBattleInfo()
    {
        if (battleInfo == null)
            battleInfo = new BattleInfo();
        battleInfo.player0Id = player0Id;
        battleInfo.player1Id = player1Id;
        battleInfo.prefabMap = prefabMap;

        for (int i = 0; i < 7; i++)
        {
            
            var propertyInfo = battleInfo.mapTag.GetType().GetField(mapName[i]);
            if (tag.ContainsKey(mapName[i]))
                propertyInfo.SetValue(battleInfo.mapTag, tag[mapName[i]]);
        }
        for (int i = 0; i < 4; i++)
        {
            var propertyInfo = battleInfo.creatureTag.GetType().GetField(mapName[i + 7]);
            if (tag.ContainsKey(mapName[i + 7]))
                propertyInfo.SetValue(battleInfo.creatureTag, tag[mapName[i + 7]]);
        }
    }
    static void FetchColor()
    {
        if (ColorInfoDic != null) return;
        string dataJson = GlobalGameManager.Instance.CharacterConfigs.text;
        nodeData nData = JsonUtility.FromJson<nodeData>(dataJson);
        ColorInfoDic = new Dictionary<string, CharacterColorInfo>();
        foreach(CharacterColorInfo ch in nData.data)
        {
            ColorInfoDic.Add(ch.characterId, ch);
        }
    }
    static void FetchBattleInfo()
    {
        if (battleInfo != null) return;
        string dataJson = GameManager.Instance.BattleConfigs.text;
        battleInfo  = JsonUtility.FromJson<BattleInfo>(dataJson);
    }
    public static CharacterColorInfo queryColorByCharacter(string character)
    {
        FetchColor();
        if (ColorInfoDic != null && ColorInfoDic.ContainsKey(character))
            return ColorInfoDic[character];
        else return null;
    }
    public static BattleInfo queryBattleInfo()
    {
        FetchBattleInfo();
        if (battleInfo != null)
            return battleInfo;
        else return null;
    }
}
//configs class
[Serializable]
public class MapTag
{
    public int 荆棘 = 0;
    public int 水洼 = 0;
    public int 金枝 = 0;
    public int 灌木 = 0;
    public int 毒菇 = 0;
    public int 障碍物 = 0;
    public int 象征之森 = 0;
}
[Serializable]
public class CreatureTag
{
    public int 乌鸦;
    public int 萤火虫;
    public int 九色鹿;
    public int 毒蛇;
}
[Serializable]
public class BattleInfo
{
    public string player0Id;
    public string player1Id;
    public string prefabMap;
    public MapTag mapTag;
    public CreatureTag creatureTag;
    public BattleInfo()
    {
        mapTag = new MapTag();
        creatureTag = new CreatureTag();
    }
}
[Serializable]
public class CharacterColorInfo
{
    public string characterId; //选择的角色名称，一切颜色属性的标识
    public string AnimatorControllerPath;
    public string ParticleSpritePath;
    public string dieSpritePath;
    public string uniqueSkillPath;
    public string activeSkillPath;
    public string passiveSkillPath;
    public string uniqueSkillContent;
    public string activeSkillContent;
    public string passiveSkillContent;
    
    public Vector4 selectCharacterColor1;
    public Vector4 selectCharacterColor2;
    public Vector4[] shadowColor;
    public Vector4 accumulateColor;
    public Vector3 particlePos;
    public float particleScale;
    public Vector4 lightColor;
}
[Serializable]
public class nodeData
{
    public List<CharacterColorInfo> data;
}