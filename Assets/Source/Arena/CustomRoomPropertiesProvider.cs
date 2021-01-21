using Photon.Pun;

namespace TableSync
{
    public static class CustomRoomPropertiesProvider
    {
        public static CustomRoomProperties Download()
        {
            var propertiesHashtable = PhotonNetwork.CurrentRoom.CustomProperties;
            return CustomRoomPropertiesConverter.FromHashtable(propertiesHashtable);
        }

        public static void Upload(CustomRoomProperties customRoomProperties)
        {
            var hashtable = CustomRoomPropertiesConverter.ToHashtable(customRoomProperties);
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }
    }
}