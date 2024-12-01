using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Managing.Scened;
using FishNet.Connection;
using System.Linq;
using FishySteamworks;
using FishNet.Managing;
using FishNet;

public class BootStrapNetworkManager : NetworkBehaviour
{
    public static BootStrapNetworkManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void SpawnGameSceneObject(GameObject ObjToSpawn)
    {
        GameObject n = Instantiate(ObjToSpawn);
        InstanceFinder.ServerManager.Spawn(n, InstanceFinder.ClientManager.Connection);
    }
    public void ChangeNetworkScene(string sceneName, string[] sceneToClose)
    {
        Instance.Server_closeScene(sceneToClose);
        SceneLoadData sid = new SceneLoadData(sceneName);
        NetworkConnection[] conns = Instance.ServerManager.Clients.Values.ToArray();
        Instance.SceneManager.LoadConnectionScenes(conns, sid);
    }
    [ServerRpc(RequireOwnership = false)]
    void Server_closeScene(string[] sceneToClose, NetworkConnection conn = null)
    {
        Debug.Log($"Received on the server from connection {conn.ClientId}.");
        Client_closeScene(sceneToClose);
    }
    [ObserversRpc]
    void Client_closeScene(string[] sceneToClose)
    {
        foreach(var s in sceneToClose)
        {
            
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(s);
        }
    }
}
