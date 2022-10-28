using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public enum CameraAngles { 
    MENU,
    WHITETEAM,
    BLACKTEAM,
}


public class GameUI : MonoBehaviour
{

    #region singleton
    public static GameUI Instance { get; private set; }
    #endregion

    [SerializeField] Server Server;
    [SerializeField] Client Client;

    private ushort portNo = 8007; 

    [SerializeField] private float _transitionDuration = 0.15f;
    [SerializeField] private RectTransform _startUI;
    [SerializeField] private Button _localGameButton;
    [SerializeField] private Button _onlineGameButton;
    [Space(20)]
    [SerializeField] private RectTransform _onlineUI;
    [SerializeField] private Button _onlineHostButton;
    [SerializeField] private TMP_InputField _ipField;
    [SerializeField] private Button _connectButton;
    [SerializeField] private Button _hostBackButton;
    [Space(20)]
    [SerializeField] private RectTransform _waitUI;
    [SerializeField] private Button _waitBackButton;
    [Space(20)]
    [SerializeField] private RectTransform _localgameUI;
    [Space(20)]
    [SerializeField] private GameObject[] cameras;

    public Action<bool> SetLocalGame;

    private void Awake()
    {
        Instance = this;

        _localGameButton.onClick.AddListener(OnLocalGameButton);
        _onlineGameButton.onClick.AddListener(OnOnlineGameButton);
        _onlineHostButton.onClick.AddListener(OnHostGameButton);
        _connectButton.onClick.AddListener(OnOnlineConnectButton);
        _hostBackButton.onClick.AddListener(OnHostBackButton);
        _waitBackButton.onClick.AddListener(OnWaitUIBackButton);
        RegisterToEvent();
    }


    private void RegisterToEvent()
    {
        NetUtility.C_START_GAME += OnStartGameClient;
    }

    private void UnregisterToEvents()
    {
        NetUtility.C_START_GAME -= OnStartGameClient;
    }

    public void ChangeCamera(int cameraIndex) {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].SetActive(false);
        }

        cameras[cameraIndex].SetActive(true);
    }


    public void OnLeaveFromGameMenu() {
        ChangeCamera((int)CameraAngles.MENU);
        _startUI.DOLocalMoveY(0, _transitionDuration);
        _localgameUI.DOLocalMoveY(1200f, _transitionDuration);
        Debug.Log("Back to Start Menu");

    }

    private void OnStartGameClient(NetMessage msg)
    {
        _waitUI.DOLocalMoveX(-2500f, _transitionDuration);
    }


    private void OnLocalGameButton() {
        _startUI.DOLocalMoveY(-1200f, _transitionDuration);
        _localgameUI.DOLocalMoveY(0, _transitionDuration);

        Server.Init(portNo);
        //connecting to self
        Client.Init("127.0.0.1", portNo);

        SetLocalGame?.Invoke(true);
    }

    private void OnOnlineGameButton() {
        _startUI.DOLocalMoveX(2500, _transitionDuration);
        _onlineUI.DOLocalMoveX(0f, _transitionDuration);
        _waitUI.DOLocalMoveX(0f, _transitionDuration);
    }


    private void OnHostGameButton() {
        _onlineUI.DOLocalMoveY(-1200f, _transitionDuration);
        _waitUI.DOLocalMoveY(0, _transitionDuration);

        Server.Init(portNo);
        //connecting to self
        Client.Init("127.0.0.1", portNo);


    }


    private void OnOnlineConnectButton() {
        _onlineUI.DOLocalMoveY(-1200f, _transitionDuration);
        _waitUI.DOLocalMoveY(0, _transitionDuration);

        Client.Init(_ipField.text, portNo);
    }


    private void OnHostBackButton() {
        _startUI.DOLocalMoveX(0f, _transitionDuration);
        _onlineUI.DOLocalMoveX(-2500f, _transitionDuration);
        _waitUI.DOLocalMoveX(-2500f, _transitionDuration);
        //shut down server
        Server.ShutDown();
        Client.ShutDown();

    }

    private void OnWaitUIBackButton() {
        _onlineUI.DOLocalMoveY(0f, _transitionDuration);
        _waitUI.DOLocalMoveY(1200f, _transitionDuration);
        Server.ShutDown();
        Client.ShutDown();
    }




}
