using FishNet.Managing;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BootStrapManager : MonoBehaviour
{
    public static BootStrapManager Instance;

    [SerializeField] string menuName;
    [SerializeField] NetworkManager networkManager;
    [SerializeField] FishySteamworks.FishySteamworks fishySteamwork;
    
    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequested;
    protected Callback<LobbyEnter_t> LobbyEntered;
    public static ulong CurrentLobbyId;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    public static void CreateLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 2);

    }
    private void Start()
    {
        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

    }
    void OnLobbyCreated(LobbyCreated_t callback)
    {
        //Debug.Log("Starting lobby creation:" + callback.m_eResult.ToString());
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            //错误处理
            Debug.Log("Starting lobby creation:" + callback.m_eResult.ToString());
            return;
        }
        CurrentLobbyId = callback.m_ulSteamIDLobby;
        SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyId), "HostAddress", SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyId), "Name", SteamFriends.GetPersonaName().ToString() + "的 房间");
        fishySteamwork.SetClientAddress(SteamUser.GetSteamID().ToString());
        fishySteamwork.StartConnection(true);
        Debug.Log("lobby creation is successful");
    }
    void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }
    void OnLobbyEntered(LobbyEnter_t callback)
    {
        CurrentLobbyId = callback.m_ulSteamIDLobby;
        SteamTestMenuManager.Instance.LobbyEntered(SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyId), "Name"), networkManager.IsServerStarted);
        fishySteamwork.SetClientAddress(SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyId), "HostAddress"));
        fishySteamwork.StartConnection(false);
    }
    public static void JoinByID(CSteamID id)
    {
        Debug.Log("attending to join Lobby with id" + id.m_SteamID);
        if (SteamMatchmaking.RequestLobbyData(id))
        {
            SteamMatchmaking.JoinLobby(id);
        }
        else
        {
            Debug.Log("Fail to join it with wrong steam id");
        }
    }
    public static void LeaveLobby()
    {
        SteamMatchmaking.LeaveLobby(new CSteamID(CurrentLobbyId));
        CurrentLobbyId = 0;

        Instance.fishySteamwork.StopConnection(false);
        if (Instance.networkManager.IsServerStarted)
        {
            Instance.fishySteamwork.StopConnection(true);
        }
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene(menuName, LoadSceneMode.Additive);
    }
}
