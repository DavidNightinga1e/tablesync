namespace TableSync
{
    public class CustomRoomProperties
    {
        public bool IsBluePlayerConnected;
        public bool IsOrangePlayerConnected;
        
        public static CustomRoomProperties Default => new CustomRoomProperties();
    }
}