using System;
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
        [SerializeField] private TextMeshProUGUI version;
        [SerializeField] private Button quickSearchButton;
        [SerializeField] private Button joinOrCreatePrivateRoomButton;

        private LocalSettingsProvider _localSettingsProvider;

        private void Awake()
        {
            version.text = $"Version: {Application.version}";

            PhotonPeerHelper.RegisterCustomTypes();

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
            PhotonNetwork.JoinOrCreateRoom(privateRoomName,
                new RoomOptions
                {
                    CustomRoomProperties = new Hashtable
                    {
                        {nameof(CustomRoomProperties.IsBluePlayerConnected), false},
                        {nameof(CustomRoomProperties.IsOrangePlayerConnected), false}
                    },
                    IsVisible = false
                },
                TypedLobby.Default);
        }

        private void QuickSearch()
        {
            PhotonNetwork.NickName = _localSettingsProvider.settings.nickname;
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(null);
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !dialogBox.IsVisible)
            {
                dialogBox.RemoveAllListeners();
                dialogBox.IsVisible = true;
                dialogBox.YesButtonVisible = dialogBox.NoButtonVisible = true;
                dialogBox.CancelButtonVisible = false;
                dialogBox.Text.text = "Are you sure you want to leave the game?";
                dialogBox.YesButton.onClick.AddListener(Application.Quit);
                dialogBox.NoButton.onClick.AddListener(() => dialogBox.IsVisible = false);
            }
        }
    }
}