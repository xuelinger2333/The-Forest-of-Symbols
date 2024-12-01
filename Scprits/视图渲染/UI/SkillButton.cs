using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.UI;
public class SkillButton : MonoBehaviour
{
    public bool playerId;
    [SerializeField]Character player;
    public Image image;
    CharacterColorInfo ColorInfo;
    [SerializeField]
    int Passive_Active_Unique;
    bool isShadowHacker = false;
    bool isShadowHackerRecallIcon = false;
    string specialSkillPath_shadowHacker = "骇客主动技冷却";

    // Start is called before the first frame update
    void Start()
    {
        if (!player)
            if (playerId) player = GameManager.Instance.player1 as Character;
            else player = GameManager.Instance.player0 as Character;
        if (player.characterName == "倩影骇客") isShadowHacker = true;

        ColorInfo = ConfigReader.queryColorByCharacter(player.characterName);
        switch (Passive_Active_Unique)
        {
            case 0:
                GetComponent<Image>().sprite = Resources.Load<Sprite>(ColorInfo.passiveSkillPath);
                break;
            case 1:
                GetComponent<Image>().sprite = Resources.Load<Sprite>(ColorInfo.activeSkillPath);
                break;
            case 2:
                GetComponent<Image>().sprite = Resources.Load<Sprite>(ColorInfo.uniqueSkillPath);
                break;

        }
    }
    public void ChangeSkillButtonSprite(string path)
    {
        GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
    }
    // Update is called once per frame
    void Update()
    {
        switch (Passive_Active_Unique)
        {
            case 0:
                float2 pinfo = player.getPassiveSkillInfo();
                image.fillAmount = 1 - ((pinfo.y - pinfo.x) / pinfo.y);
                break;
            case 1:            
                float2 info = player.getActiveSkillInfo();
                image.fillAmount = 1 - ((info.y - info.x) / info.y);
                
                if (image.fillAmount == 0)
                {
                    if (isShadowHacker && isShadowHackerRecallIcon)
                    {
                        ChangeSkillButtonSprite(ColorInfo.activeSkillPath);
                        isShadowHackerRecallIcon = false;
                    }
                }
                if (image.fillAmount == 1)
                {   
                    if (isShadowHacker && !isShadowHackerRecallIcon)
                    {
                        ChangeSkillButtonSprite(specialSkillPath_shadowHacker);
                        isShadowHackerRecallIcon = true;
                    }

                }
                break;
            case 2:
                info = player.getUniqueSkillInfo();
                image.fillAmount = 1 - info.x / info.y;
                break;
        }
        if (image.fillAmount < 0) image.fillAmount = 0;
    }
}
