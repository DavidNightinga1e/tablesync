using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Simba;
using Source.Lobby.Components;

namespace Source.Lobby.Systems
{
    public class ConnectionSystem : IStartSystem, IConnectionCallbacks, IOnDestroySystem
    {
        private LoadingPlaceholder _placeholder;
        
        public void Start()
        {
            PhotonNetwork.AddCallbackTarget(this);
            _placeholder = SimbaComponent.Get<LoadingPlaceholder>();
        }
        public void OnDestroy()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnConnected()
        {
        }

        public void OnConnectedToMaster()
        {
            _placeholder.IsVisible = false;
        }

        public void OnDisconnected(DisconnectCause cause)
        {
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
        }
    }
}