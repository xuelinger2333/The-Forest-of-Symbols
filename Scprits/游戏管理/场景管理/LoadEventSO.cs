using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Load Event Channel")]
public class LoadEventChannelSO : ScriptableObject
{
    public UnityAction<string> OnLoadingRequested;

    public void RaiseEvent(string locationsToLoad)
    {
        if (OnLoadingRequested != null)
        {
            OnLoadingRequested.Invoke(locationsToLoad);
        }
        else
        {
            Debug.LogWarning("A Scene loading was requested, but nobody picked it up." +
                "Check why there is no SceneLoader already present, " +
                "and make sure it's listening on this Load Event channel.");
        }
    }
}
