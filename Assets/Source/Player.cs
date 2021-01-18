using System;
using Photon.Pun;
using UnityEngine;

namespace TableSync.Demo
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject bloodParticleSystemPrefab;

        public PlayerColor playerColorType;
        public Transform bulletSpawnPoint;
        public CharacterController characterController;

        private GameManager _gameManager;
        private double _lastShootTime;

        private const float ReloadTime = 1;
        private const float PlayerSpeed = 3;
        private const int MaxLives = 3;

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
            characterController = GetComponent<CharacterController>();

            if (photonView.IsMine)
            {
                var cam = FindObjectOfType<CameraController>();
                switch (playerColorType)
                {
                    case PlayerColor.Blue:
                        cam.SetBlue();
                        break;
                    case PlayerColor.Orange:
                        cam.SetOrange();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void Update()
        {
            if (!photonView.IsMine) return;

            ProcessInput();
        }

        private void ProcessInput()
        {
            var inputMove = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (playerColorType == PlayerColor.Orange)
                inputMove = -inputMove; // because for orange player level is upside-down

            var translate = inputMove * (PlayerSpeed * Time.deltaTime);
            characterController.Move(translate);

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var origin = ray.origin;
            var direction = ray.direction;
            var directionMultiplier = -origin.y / direction.y;
            var xPos = origin.x + directionMultiplier * direction.x;
            var zPos = origin.z + directionMultiplier * direction.z;
            var targetLook = new Vector3(xPos, 0, zPos);
            transform.LookAt(targetLook);

            var currentTime = PhotonNetwork.Time;
            if (Input.GetButtonDown("Fire1") && currentTime > _lastShootTime + ReloadTime)
            {
                _gameManager.Shoot(this);
                _lastShootTime = currentTime;
            }
        }

        // public void GetDamage(Vector3 direction)
        // {
        //     var collider = GetComponent<CapsuleCollider>();
        //     var targetPosition = collider.ClosestPointOnBounds(collider.bounds.center + direction);
        //     var instance = Instantiate(bloodParticleSystemPrefab, targetPosition, Quaternion.LookRotation(direction));
        //     instance.GetComponent<ParticleSystem>().Play();
        // }
    }
}