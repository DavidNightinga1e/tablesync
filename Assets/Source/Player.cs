using UnityEngine;

namespace TableSync.Demo
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private GameObject bloodParticleSystemPrefab;
        
        public PlayerColor playerColorType;
        public Transform bulletSpawnPoint;
        public CharacterController characterController;

        private GameController _gameController;

        private void Awake()
        {
            _gameController = FindObjectOfType<GameController>();
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire1")) 
                _gameController.Shoot(this);

            var inputMove = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            _gameController.MovePlayer(this, inputMove);
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            _gameController.RotatePlayer(this, ray);
        }

        public void Hit(Vector3 direction)
        {
            var collider = GetComponent<CapsuleCollider>();
            var targetPosition = collider.ClosestPointOnBounds(collider.bounds.center + direction);
            var instance = Instantiate(bloodParticleSystemPrefab, targetPosition, Quaternion.LookRotation(direction));
            instance.GetComponent<ParticleSystem>().Play();
        }
    }
}