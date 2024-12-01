using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum TextCategory
{
    UI,
    Map,
    Character,
    Instruction
}
public enum UI_Text_Category
{
    Null = 0,
    //选择地图模式页面
    开场白,
    开场白_确认,
    操作说明,
    选择模式,

    随机tag_标题,
    随机tag_内容,
    自选tag_标题,
    自选tag_内容,
    预制tag_标题,
    预制tag_内容,

    分蘖枝生,

    选择预制_说明,

    选择tag_说明,
    仪式介绍,
    仪式场地,
    仪式生物,

    仪式幕开,
    一语成谶,
    贰言戳纬,
    叁字阎昧,

    一局决胜负,
    三局抢两胜,
    五局抢三胜,

    确认,

    选择角色_说明,

    空格继续,
}
public enum Map_Text_Category
{
    Null = 0,
    水洼,
    水洼_解释,
    水洼预制_内容,
    水洼预制_标题1,
    水洼预制_标题2,

    水洼tag0_内容,
    水洼tag0_标题,
    水洼tag1_内容,
    水洼tag1_标题,
    水洼tag2_内容,
    水洼tag2_标题,
    水洼tag3_内容,
    水洼tag3_标题,

    灌木,
    灌木_解释,
    灌木预制_内容,
    灌木预制_标题1,
    灌木预制_标题2,

    灌木tag0_内容,
    灌木tag0_标题,
    灌木tag1_内容,
    灌木tag1_标题,
    灌木tag2_内容,
    灌木tag2_标题,
    灌木tag3_内容,
    灌木tag3_标题,

    金枝,
    金枝_解释,
    金枝预制_内容,
    金枝预制_标题1,
    金枝预制_标题2,

    金枝tag0_内容,
    金枝tag0_标题,
    金枝tag1_内容,
    金枝tag1_标题,
    金枝tag2_内容,
    金枝tag2_标题,
    金枝tag3_内容,
    金枝tag3_标题,

    荆棘,
    荆棘_解释,
    荆棘预制_内容,
    荆棘预制_标题1,
    荆棘预制_标题2,

    荆棘tag0_内容,
    荆棘tag0_标题,
    荆棘tag1_内容,
    荆棘tag1_标题,
    荆棘tag2_内容,
    荆棘tag2_标题,
    荆棘tag3_内容,
    荆棘tag3_标题,

    毒菇,
    毒菇_解释,
    毒菇预制_内容,
    毒菇预制_标题1,
    毒菇预制_标题2,

    毒菇tag0_内容,
    毒菇tag0_标题,
    毒菇tag1_内容,
    毒菇tag1_标题,
    毒菇tag2_内容,
    毒菇tag2_标题,
    毒菇tag3_内容,
    毒菇tag3_标题,

    遗迹,
    遗迹tag0_内容,
    遗迹tag0_标题,
    遗迹tag1_内容,
    遗迹tag1_标题,
    遗迹tag2_内容,
    遗迹tag2_标题,
    遗迹tag3_内容,
    遗迹tag3_标题,

    象征之林,
    象征之林tag0_内容,
    象征之林tag0_标题,
    象征之林tag1_内容,
    象征之林tag1_标题,
    象征之林tag2_内容,
    象征之林tag2_标题,
    象征之林tag3_内容,
    象征之林tag3_标题,

    乌鸦,
    乌鸦tag0_内容,
    乌鸦tag0_标题,
    乌鸦tag1_内容,
    乌鸦tag1_标题,
    乌鸦tag2_内容,
    乌鸦tag2_标题,
    乌鸦tag3_内容,
    乌鸦tag3_标题,

    萤火虫,
    萤火虫tag0_内容,
    萤火虫tag0_标题,
    萤火虫tag1_内容,
    萤火虫tag1_标题,
    萤火虫tag2_内容,
    萤火虫tag2_标题,
    萤火虫tag3_内容,
    萤火虫tag3_标题,

    毒蛇,
    毒蛇tag0_内容,
    毒蛇tag0_标题,
    毒蛇tag1_内容,
    毒蛇tag1_标题,

    九色鹿,
    九色鹿tag0_内容,
    九色鹿tag0_标题,
    九色鹿tag1_内容,
    九色鹿tag1_标题,

}
public enum Character_Text_Category
{
    Null = 0,
    人偶新娘,
    人偶新娘_主动,
    人偶新娘_绝招,
    人偶新娘_被动,

    瘟疫医生,
    瘟疫医生_主动,
    瘟疫医生_绝招,
    瘟疫医生_被动,

    倩影骇客,
    倩影骇客_主动,
    倩影骇客_绝招,
    倩影骇客_被动,

    织梦魔女,
    织梦魔女_主动,
    织梦魔女_绝招,
    织梦魔女_被动,

}
public enum Instruction_Text_Category
{
    Null = 0,
    移动,
    移动_内容,
    攻击,
    攻击_内容,
    蓄力攻击,
    蓄力攻击_内容,
    主动技能,
    主动技能_内容,
    绝招,
    绝招_内容,
    擦刀,
    擦刀_内容,
    生命值,
    生命值_内容,
    暴露状态,
    暴露状态_内容,
    地块与生物,
    地块与生物_内容,
    混沌秘仪,
    混沌秘仪_内容,
    移动_p1,
    移动_p2,
    隐匿与暴露,
    隐匿与暴露_介绍,
    隐匿暴露_提示,
    隐匿暴露_完成,
    完全隐匿,
    一级暴露,
    二级暴露,
    三级暴露,
    个,
    攻击_p1,
    攻击_p2,
    出擦晃刀,
    出擦晃刀_提示,
    出刀,
    出擦晃刀_介绍,
    蓄力攻击_p1,
    蓄力攻击_p2,
    蓄力与蓄力攻击,
    蓄力攻击_提示,
    蓄力攻击_介绍,
    绝招与充能,
    绝招与充能_提示,
    绝招与充能_介绍,
}
public class TextQueryKey
{
    public TextCategory textCategory { get; set; }//确定哪一个种类是需要的
    public UI_Text_Category ui;
    public Map_Text_Category map;
    public Character_Text_Category character;
    public Instruction_Text_Category instruction;
    public TextQueryKey() { }
    public TextQueryKey(UI_Text_Category U)
    {
        textCategory = TextCategory.UI;
        ui = U;
        map = Map_Text_Category.Null;
        character = Character_Text_Category.Null;
        instruction = Instruction_Text_Category.Null;
    }
    public TextQueryKey(Map_Text_Category M)
    {
        textCategory = TextCategory.Map;
        ui = UI_Text_Category.Null;
        map = M;
        character = Character_Text_Category.Null;
        instruction = Instruction_Text_Category.Null;
    }
    public TextQueryKey(Character_Text_Category C)
    {
        textCategory = TextCategory.Character;
        ui = UI_Text_Category.Null;
        character = C;
        map = Map_Text_Category.Null;
        instruction = Instruction_Text_Category.Null;
    }
    public TextQueryKey(Instruction_Text_Category I)
    {
        textCategory = TextCategory.Instruction;
        ui = UI_Text_Category.Null;
        character = Character_Text_Category.Null;
        map = Map_Text_Category.Null;
        instruction = I;
    }
}
public class LocalizationManager 
{
    static string[,] dataLines = null;
    public static string GetText(TextQueryKey key, SystemLanguage systemLanguage)
    {
        if (dataLines == null)
        {
            dataLines = ReadCSV();
        }
        int idx = 0;
        //根据传入的枚举种类，解析数据库的编号
        switch (key.textCategory)
        {
            case TextCategory.UI:
                idx = (int)key.ui;
                break;
            case TextCategory.Map:
                idx += Enum.GetValues(typeof(UI_Text_Category)).Length;
                idx += (int)key.map;
                break;
            case TextCategory.Character:
                idx += Enum.GetValues(typeof(UI_Text_Category)).Length;
                idx += Enum.GetValues(typeof(Map_Text_Category)).Length;
                idx += (int)key.character;
                break;
            case TextCategory.Instruction:
                idx += Enum.GetValues(typeof(UI_Text_Category)).Length;
                idx += Enum.GetValues(typeof(Map_Text_Category)).Length;
                idx += Enum.GetValues(typeof(Character_Text_Category)).Length;
                idx += (int)key.instruction;
                break;
        }
        if (idx < dataLines.Length)
        {
            //如果数据库中包含文本
            //根据语言种类参数返回语言
            //var data = dataLines[idx].Split(',');
            switch (systemLanguage)
            {
                case SystemLanguage.Chinese:
                    return System.Text.RegularExpressions.Regex.Unescape(dataLines[1, idx]);
                case SystemLanguage.English:
                    return System.Text.RegularExpressions.Regex.Unescape(dataLines[2, idx]);
            }
            //找不到对应语言
            Debug.Log("wrong language!");
            return "";
        }
        Debug.Log("no text!");
        //数据库没有对应文本
        return "";
    }
    static string[,] ReadCSV()
    {
        string[,] grid = SplitCsvGrid(Resources.Load<TextAsset>("GameText").text);
        Debug.Log("size = " + (1 + grid.GetUpperBound(0)) + "," + (1 + grid.GetUpperBound(1)));
        //DebugOutputGrid(grid);
        return grid;
    }
    

static public string[,] SplitCsvGrid(string csvText)
    {
        string[] lines = csvText.Split("\n"[0]);

        // finds the max width of row
        int width = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = SplitCsvLine(lines[i]);
            width = Mathf.Max(width, row.Length);
        }

        // creates new 2D string grid to output to
        string[,] outputGrid = new string[width + 1, lines.Length + 1];
        for (int y = 0; y < lines.Length; y++)
        {
            string[] row = SplitCsvLine(lines[y]);
            for (int x = 0; x < row.Length; x++)
            {
                outputGrid[x, y] = row[x];

                // This line was to replace "" with " in my output. 
                // Include or edit it as you wish.
                outputGrid[x, y] = outputGrid[x, y].Replace("\"\"", "\"");
            }
        }

        return outputGrid;
    }

    // splits a CSV row 
    static public string[] SplitCsvLine(string line)
    {
        return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
        @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
        System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                select m.Groups[1].Value).ToArray();
    }
}
