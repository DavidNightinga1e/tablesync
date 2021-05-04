using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Simba;
using TableSync;
using UnityEngine;

namespace Source.Lobby.Systems
{
    public class MatchmakingSystem : IStartSystem, IOnDestroySystem, IMatchmakingCallbacks
    {
        public void Start()
        {
            PhotonNetwork.AddCallbackTarget(this);
            var component = SimbaComponent.Get<MatchmakingUserInput>();
            component.QuickSearchButton.onClick.AddListener(QuickJoin);
            component.JoinOrCreatePrivateRoomButton.onClick.AddListener(() => Join(component.PrivateRoomNameText.text));
        }

        private void QuickJoin()
        {
            PhotonNetwork.NickName = SettingsProvider.Nickname;
            PhotonNetwork.JoinRandomRoom();
        }

        private void Join(string roomName)
        {
            PhotonNetwork.NickName = SettingsProvider.Nickname;
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions {MaxPlayers = 2}, TypedLobby.Default);
        }

        public void OnDestroy()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        public void OnCreatedRoom()
        {
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.LogError($"{nameof(OnCreateRoomFailed)} - {returnCode} - {message}");
        }

        public void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Arena");
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogError($"{nameof(OnJoinRoomFailed)} - {returnCode} - {message}");
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            if (returnCode == 32760)
                Join(Guid.NewGuid().ToString());
            else 
                Debug.LogError($"{nameof(OnJoinRandomFailed)} - {returnCode} - {message}");
        }

        public void OnLeftRoom()
        {
        }
    }
}