using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BoardHover : MonoBehaviour
{
    public PlayableDirector playableDirector;
    bool rotate = false;
    private void OnMouseEnter()
    {
        if (playableDirector.state != PlayState.Playing)
        { 
            transform.DORotate(new Vector3(0, -20, 0), 0.1f);
            rotate = true;
        }
           
        Debug.Log(playableDirector.state);

    }
    private void OnMouseExit()
    {
        if (rotate)
        {
            transform.DORotate(new Vector3(0, 0, 0), 0.1f);
            rotate = false;
        }
        
    }
}
