using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagButtonParent : MonoBehaviour
{
    public TagChooseController controller;
    public void SetButtonId(int id)
    {
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i).GetComponent<TagChooseButton>().ButtonId = id;
        }
    }
    public void SetToggleGroup(ToggleGroup group)
    {
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i).GetComponent<Toggle>().group = group;
        }
    }
    public void SetTextTag(string tag)
    {
        Map_Text_Category map_tag;
        for (int i = 0; i < 4; i++)
        {
            bool success = Enum.TryParse(tag + $"tag{i}_内容", out map_tag);
            if (success)
            {
                transform.GetChild(i).GetComponent<TagChooseButton>().tag_Description = map_tag;
            }
        }
    }
}
