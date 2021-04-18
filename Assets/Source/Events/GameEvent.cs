namespace TableSync
{
    /// <summary>
    /// GameEvent for Photon Event Code. Only bytes from 0 to 199 
    /// </summary>
    public static class GameEvent
    {
        public const byte BulletShoot = 0;
        public const byte BulletPlayerHit = 1;
        public const byte PlayerJoined = 2;
        public const byte CountdownStarted = 3;
        public const byte GameStarted = 4;
        public const byte GameEnded = 5;
    }
}