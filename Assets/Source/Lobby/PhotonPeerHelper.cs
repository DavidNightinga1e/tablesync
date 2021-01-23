using ExitGames.Client.Photon;

namespace TableSync
{
    public class PhotonPeerHelper
    {
        public static void RegisterCustomTypes()
        {
            PhotonPeer.RegisterType(
                typeof(BulletSpawnEventData),
                0,
                BulletSpawnEventData.Serialize,
                BulletSpawnEventData.Deserialize);

            PhotonPeer.RegisterType(
                typeof(BulletHitEventData),
                1,
                BulletHitEventData.Serialize,
                BulletHitEventData.Deserialize);
        }
    }
}