using ExitGames.Client.Photon;
using Photon.Pun;

namespace TableSync
{
    public static class CustomRoomProperties
    {
        public static bool IsBluePlayerConnected
        {
            get => (bool) PhotonNetwork.CurrentRoom.CustomProperties[nameof(IsBluePlayerConnected)];
            set => PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable {{nameof(IsBluePlayerConnected), value}});
        }
        
        public static bool IsOrangePlayerConnected
        {
            get => (bool) PhotonNetwork.CurrentRoom.CustomProperties[nameof(IsOrangePlayerConnected)];
            set => PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable {{nameof(IsOrangePlayerConnected), value}});
        }
    }
}