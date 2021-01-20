﻿using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TableSync
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private DialogBox dialogBox;
        [SerializeField] private TMP_InputField privateRoomNameText;
        [SerializeField] private Button quickSearchButton;
        [SerializeField] private Button joinOrCreatePrivateRoomButton;

        private LocalSettingsProvider _localSettingsProvider;

        public static readonly RoomOptions DefaultRoomOptions = new RoomOptions
        {
            MaxPlayers = 2,
            CustomRoomProperties = new Hashtable
            {
                {"BluePlayers", 0},
                {"OrangePlayers", 0}
            }
        };

        private void Awake()
        {
            // todo move to entry point
            print(PhotonPeer.RegisterType(
                typeof(BulletSpawnData), 
                0, 
                BulletSpawnData.Serialize,
                BulletSpawnData.Deserialize));

            print(PhotonPeer.RegisterType(
                typeof(BulletHitData),
                1,
                BulletHitData.Serialize,
                BulletHitData.Deserialize));

            _localSettingsProvider = FindObjectOfType<LocalSettingsProvider>();

            dialogBox.IsVisible = true;
            dialogBox.YesButtonVisible = dialogBox.NoButtonVisible = false;
            dialogBox.CancelButtonVisible = true;
            dialogBox.Text.text = "Connecting to Master server, please wait";
            dialogBox.CancelButton.onClick.AddListener(OnCancelMasterConnection);

            quickSearchButton.onClick.AddListener(QuickSearch);
            joinOrCreatePrivateRoomButton.onClick.AddListener(JoinOrCreatePrivateRoom);

            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }

        private void JoinOrCreatePrivateRoom()
        {
            PhotonNetwork.NickName = _localSettingsProvider.settings.nickname;
            var privateRoomName = privateRoomNameText.text;
            PhotonNetwork.JoinOrCreateRoom(privateRoomName, DefaultRoomOptions, TypedLobby.Default);
        }

        private void QuickSearch()
        {
            PhotonNetwork.NickName = _localSettingsProvider.settings.nickname;
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(null, DefaultRoomOptions, TypedLobby.Default);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Arena");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            dialogBox.IsVisible = true;
            dialogBox.YesButtonVisible = dialogBox.NoButtonVisible = false;
            dialogBox.CancelButtonVisible = true;
            dialogBox.Text.text = $"[{returnCode}] Failed to join: {message}";
            dialogBox.CancelButton.onClick.AddListener(() =>
            {
                dialogBox.RemoveAllListeners();
                dialogBox.IsVisible = false;
            });
        }

        private void OnCancelMasterConnection()
        {
            Application.Quit();
        }

        public override void OnConnectedToMaster()
        {
            dialogBox.RemoveAllListeners();
            dialogBox.IsVisible = false;
        }
    }
}