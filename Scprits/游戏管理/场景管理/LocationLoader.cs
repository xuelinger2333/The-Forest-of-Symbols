using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 负责监听场景加载与卸载的事件
/// </summary>
public class LocationLoader : MonoBehaviour
{
    [SerializeField] private string _initializationScene = default;
    [SerializeField] private LoadEventChannelSO _loadEventChannel = default;
    private List<AsyncOperation> _scenesToLoadAsyncOperations = new List<AsyncOperation>();
    private List<Scene> _ScenesToUnload = new List<Scene>();
    private string _activeScene;

    private void OnEnable()
    {
        _loadEventChannel.OnLoadingRequested += LoadScenes;
    }

    private void OnDisable()
    {
        _loadEventChannel.OnLoadingRequested -= LoadScenes;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == _initializationScene)
            LoadScenes("StartScene");
    }

    /// <summary>
    /// 切换场景
    /// </summary>
    /// <param name="locationsToLoad"></param>
    private void LoadScenes(string locationsToLoad)
    {
        //添加
        AddScenesToUnload();
        //if (locationsToLoad == "StartScene") AudioManager.Instance.PlayAudioCue("开始场景");
        //else if (locationsToLoad == "LibraryScene") AudioManager.Instance.PlayAudioCue("资料陈列");
        _activeScene = locationsToLoad;

        _scenesToLoadAsyncOperations.Add(SceneManager.LoadSceneAsync(_activeScene, LoadSceneMode.Additive));
        if (_scenesToLoadAsyncOperations.Count > 0)
        {
            // 确保列表不为空后再执行回调
            _scenesToLoadAsyncOperations[0].completed += SetActiveScene;
        }
        else
        {
            Debug.LogError("No scenes to load, _scenesToLoadAsyncOperations is empty.");
        }
        _scenesToLoadAsyncOperations.Clear();
        UnloadScenes();
    }

    /// <summary>
    /// 设置为活跃场景
    /// </summary>
    /// <param name="asyncOp"></param>
    private void SetActiveScene(AsyncOperation asyncOp)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_activeScene));
        Debug.Log(_activeScene);
    }

    /// <summary>
    /// 添加不需要的场景到Unload列表
    /// </summary>
    private void AddScenesToUnload()
    {
        for (int i = 0; i < SceneManager.sceneCount; ++i)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name != _initializationScene)
            {
                Debug.Log("Added scene to unload = " + scene.name);
                //Add the scene to the list of the scenes to unload
                _ScenesToUnload.Add(scene);
            }
        }
    }

    /// <summary>
    /// 卸载不需要的场景
    /// </summary>
    private void UnloadScenes()
    {
        if (_ScenesToUnload != null)
        {
            for (int i = 0; i < _ScenesToUnload.Count; ++i)
            {
                SceneManager.UnloadSceneAsync(_ScenesToUnload[i]);
            }
        }
        _ScenesToUnload.Clear();
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit!");
    }

}
