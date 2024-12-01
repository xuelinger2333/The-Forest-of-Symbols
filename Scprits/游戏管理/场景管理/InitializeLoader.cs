using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializeLoader : MonoBehaviour
{
#if UNITY_EDITOR
    public string initializationScene;
    public int targetFramerate = 0;

    private void Awake()
    {
        Application.targetFrameRate = targetFramerate;

        for (int i = 0; i < SceneManager.sceneCount; ++i)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == initializationScene)
            {
                return;
            }
        }
        SceneManager.LoadSceneAsync(initializationScene, LoadSceneMode.Additive);
    }
#endif
}
