using ExitGames.Client.Photon;

namespace TableSync
{
    public static class CustomRoomPropertiesConverter
    {
        public static Hashtable ToHashtable(CustomRoomProperties properties)
        {
            return new Hashtable
            {
                {nameof(properties.IsBluePlayerConnected), properties.IsBluePlayerConnected},
                {nameof(properties.IsOrangePlayerConnected), properties.IsOrangePlayerConnected}
            };
        }

        public static CustomRoomProperties FromHashtable(Hashtable hashtable)
        {
            return new CustomRoomProperties
            {
                IsBluePlayerConnected = (bool)hashtable[nameof(CustomRoomProperties.IsBluePlayerConnected)],
                IsOrangePlayerConnected = (bool)hashtable[nameof(CustomRoomProperties.IsOrangePlayerConnected)]
            };
        }
    }
}