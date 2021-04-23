using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Simba;
using Source.Lobby.Components;
using TableSync;

namespace Source.Lobby.Systems
{
    public class PhotonInitializationSystem : IAwakeSystem
    {
        public void Awake()
        {
            PhotonPeerHelper.RegisterCustomTypes();
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
            SimbaComponent.Get<LoadingPlaceholder>().IsVisible = true;
        }
    }
}