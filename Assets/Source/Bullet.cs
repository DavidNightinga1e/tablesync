using UnityEngine;

namespace TableSync.Demo
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
            particleSystemBulletController.Cutoff();
            //FindObjectOfType<GameController>().Hit(other);
            Destroy(gameObject);
        }
    }
}