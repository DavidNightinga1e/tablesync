using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

namespace TableSync
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject bloodParticleSystemPrefab;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private Camera inputCamera;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private CharacterController characterController;
        
        public PlayerColor playerColorType;
        public Transform bulletSpawnPoint;

        private const float ReloadTime = 0.8f;
        private const float PlayerSpeed = 5f;
        
        private double _lastShootTime;
        
        private void Awake()
        {
            if (!photonView.IsMine) return;

            SetupCameraForThisPlayer();
        }

        private void SetupCameraForThisPlayer()
        {
            switch (playerColorType)
            {
                case PlayerColor.Blue:
                    cameraController.SetBlue();
                    break;
                case PlayerColor.Orange:
                    cameraController.SetOrange();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Update()
        {
            if (!photonView.IsMine) return;

            ProcessInputForThisPlayer();
        }

        private void ProcessInputForThisPlayer()
        {
            Vector3 inputMove;
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");
            var mousePosition = Input.mousePosition;
            switch (playerColorType)
            {
                case PlayerColor.Blue:
                    inputMove = Vector3.right * horizontalInput + Vector3.forward * verticalInput;
                    break;
                case PlayerColor.Orange:
                    inputMove = Vector3.left * horizontalInput + Vector3.back * verticalInput;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            LookAtMouse(inputCamera, mousePosition);

            if (gameManager.isGameRunning)
            {
                Move(inputMove);

                var currentTime = PhotonNetwork.Time;
                if (Input.GetButtonDown("Fire1") && currentTime > _lastShootTime + ReloadTime)
                {
                    Shoot();
                    _lastShootTime = currentTime;
                }
            }
        }

        private void Move(Vector3 inputMove)
        {
            var translate = inputMove * (PlayerSpeed * Time.deltaTime);
            characterController.Move(translate);
        }

        private void LookAtMouse(Camera cam, Vector3 mousePosition)
        {
            var ray = cam.ScreenPointToRay(mousePosition);
            var origin = ray.origin;
            var direction = ray.direction;
            var directionMultiplier = -origin.y / direction.y;
            var xPos = origin.x + directionMultiplier * direction.x;
            var zPos = origin.z + directionMultiplier * direction.z;
            var targetLook = new Vector3(xPos, 0, zPos);
            transform.LookAt(targetLook);
        }

        private void Shoot()
        {
            var position = bulletSpawnPoint.position;
            var rotation = bulletSpawnPoint.rotation.eulerAngles;
            var bulletSpawnData = new BulletSpawnData
            {
                position = new Vector2(position.x, position.z),
                rotation = rotation.y
            };
            PhotonNetwork.RaiseEvent(
                GameEvents.BulletShoot,
                bulletSpawnData,
                EventsUtilities.RaiseEventOptionsReceiversAll,
                SendOptions.SendReliable);
        }
        
        public void DisplayHit(Vector2 direction)
        {
            var direction3 = new Vector3(direction.x, 0, direction.y);
            var playerHitCollider = GetComponent<CapsuleCollider>();
            var targetPosition = playerHitCollider.ClosestPointOnBounds(playerHitCollider.bounds.center + direction3);
            var instance = Instantiate(bloodParticleSystemPrefab, targetPosition, Quaternion.LookRotation(direction));
            instance.GetComponent<ParticleSystem>().Play();
        }
    }
}