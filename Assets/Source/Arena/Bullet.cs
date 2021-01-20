using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

namespace TableSync
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private ParticleSystemBulletController particleSystemBulletController;
        [SerializeField] private Rigidbody bulletRigidbody;

        private void Awake()
        {
            particleSystemBulletController.Trail();
        }

        public void AddVelocity(float velocity)
        {
            bulletRigidbody.AddForce(velocity * transform.forward, ForceMode.VelocityChange);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var player = other.transform.GetComponent<Player>();
                if (player != null)
                {
                    var playerViewId = player.photonView.ViewID;
                    var bulletHitData = new BulletHitData
                    {
                        direction = new Vector2(other.impulse.x, other.impulse.z),
                        viewId = playerViewId
                    };

                    PhotonNetwork.RaiseEvent(
                        GameEvents.BulletPlayerHit,
                        bulletHitData,
                        EventsUtilities.RaiseEventOptionsReceiversAll,
                        SendOptions.SendReliable);
                }
            }

            particleSystemBulletController.Cutoff();
            Destroy(gameObject);
        }
    }
}