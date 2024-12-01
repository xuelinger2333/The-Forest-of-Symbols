using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteamTestMenuManager : MonoBehaviour
{
    public static SteamTestMenuManager Instance;
    [SerializeField] GameObject menuScreen, lobbyScreen;
    [SerializeField] InputField lobbyInput;
    [SerializeField] Text lobbyName, lobbyId;
    [SerializeField] Button startGameButton;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        OpenMainMenu();
    }
    public void OnCreateLobbyClick()
    {
        BootStrapManager.CreateLobby();
    }
    public void OpenMainMenu()
    {
        CloseAllScreens();
        menuScreen.SetActive(true);
    }
    public void OpenLobbyMenu()
    {
        CloseAllScreens();
        lobbyScreen.SetActive(true);
    }
    void CloseAllScreens()
    {
        menuScreen.SetActive(false);
        lobbyScreen.SetActive(false);
    }
    public void JoinLobby()
    {
        CSteamID steamID = new CSteamID(Convert.ToUInt64(lobbyInput.text));
        BootStrapManager.JoinByID(steamID);
    }
    public void LeaveLobby()
    {
        BootStrapManager.LeaveLobby();
        OpenMainMenu();
    }
    public void LobbyEntered(string lobbyName, bool isHost)
    {
        Instance.lobbyName.text = lobbyName;
        Instance.startGameButton.gameObject.SetActive(isHost);
        lobbyId.text = BootStrapManager.CurrentLobbyId.ToString();
        OpenLobbyMenu();
    }
    public void StartGame()
    {
        string[] sceneToClose = new string[] { "SteamTestMenu" };
        BootStrapNetworkManager.Instance.ChangeNetworkScene("DemoScene", sceneToClose);
    }
}
