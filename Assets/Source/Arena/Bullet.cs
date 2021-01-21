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

        private void OnCollisionEnter(Collision collision)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var player = collision.transform.GetComponentInParent<Player>();
                if (player != null)
                {
                    var playerViewId = player.photonView.ViewID;
                    var bulletHitData = new BulletHitEventData
                    {
                        direction = new Vector2(collision.impulse.x, collision.impulse.z),
                        viewId = playerViewId
                    };

                    PhotonNetwork.RaiseEvent(
                        GameEvents.BulletPlayerHit,
                        bulletHitData,
                        GameEventsUtilities.RaiseEventOptionsReceiversAll,
                        SendOptions.SendReliable);
                }
            }

            particleSystemBulletController.Cutoff();
            Destroy(gameObject);
        }
    }
}