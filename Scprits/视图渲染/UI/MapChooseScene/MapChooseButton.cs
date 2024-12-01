using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapChooseButton : MonoBehaviour
{
    Toggle toggle;
    public GameObject Icon;
    List<Transform> IconChild;
    bool isPressed;
    // Start is called before the first frame update
    void Awake()
    {
        IconChild = new List<Transform>();        
        for (int i = 0; i < Icon.transform.childCount; i++)
        {
            IconChild.Add(Icon.transform.GetChild(i));
        }
        toggle = GetComponent<Toggle>();
        isPressed = false;

        if (toggle.isOn)
        {
            ChangeFade();
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeFade()
    {
        if (toggle.isOn == isPressed) return;
        else isPressed = toggle.isOn;
        if (toggle.isOn)
        {
            for (int i = 0; i < IconChild.Count; i++)
            {
                Transform child = IconChild[i];
                DOVirtual.Color(new Color(0.645f, 0.645f, 0.645f), new Color(1, 1, 1, 1), 0.2f, c => { child.GetComponent<Image>().color = c; });
                    
            }
        }
        else
        {
            for (int i = 0; i < IconChild.Count; i++)
            {
                Transform child = IconChild[i];
                DOVirtual.Color(new Color(1, 1, 1, 1),new Color(0.645f, 0.645f, 0.645f),  0.2f, c => { child.GetComponent<Image>().color = c; });

            }

        }
    }
}
