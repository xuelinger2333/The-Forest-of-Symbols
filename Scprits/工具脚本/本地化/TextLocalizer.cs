using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextLocalizer : MonoBehaviour
{
    //默认globalManager一定存在，读取globalManager的语言设置
    public TextCategory textCategory;//确定哪一个种类是需要的
    public UI_Text_Category ui;
    public Map_Text_Category map;
    public Character_Text_Category character;
    public Instruction_Text_Category instruction;

    [SerializeField] Text TextUI;
    [SerializeField] TextMeshProUGUI TextMeshProUGUI;
    // Start is called before the first frame update
    void Start()
    {
        if (!TextUI)
        {
            TryGetComponent(out TextUI);
        }
        if (!TextMeshProUGUI)
        {
            TryGetComponent(out TextMeshProUGUI);
        }
        SystemLanguage language = GlobalGameManager.systemLanguage;
        TextQueryKey key = new TextQueryKey();
        key.textCategory = textCategory;
        key.ui = ui;
        key.map = map;
        key.character = character;
        key.instruction = instruction;
        if (TextUI)
        {
            switch (language)
            {
                case SystemLanguage.English:
                    TextUI.font = GlobalGameManager.Instance.EnglishFont;
                    break;
                case SystemLanguage.Chinese:
                    TextUI.font = GlobalGameManager.Instance.ChineseFont;
                    break;
            }

            TextUI.text = LocalizationManager.GetText(key, language);
        }
        if (TextMeshProUGUI)
        {
            TextMeshProUGUI.text = LocalizationManager.GetText(key, language);
        }
    }
}
