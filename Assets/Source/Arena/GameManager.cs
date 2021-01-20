using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace TableSync
{
    public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        [SerializeField] private Bullet bulletPrefab;

        private readonly Vector3 _blueStartPlayerPosition = new Vector3(4, 0, -4);
        private readonly Vector3 _orangeStartPlayerPosition = new Vector3(-4, 0, 4);
        private readonly Quaternion _blueStartPlayerRotation = Quaternion.Euler(0, 0, 0);
        private readonly Quaternion _orangeStartPlayerRotation = Quaternion.Euler(0, 180, 0);

        private LocalSettingsProvider _localSettingsProvider;

        public event Action<int> OnBluePlayerLivesUpdate;
        public event Action<int> OnOrangePlayerLivesUpdate;
        
        private int _bluePlayerLives;
        private int _orangePlayerLives;

        public bool isGameRunning;

        public int BluePlayerLives
        {
            get => _bluePlayerLives;
            private set => OnBluePlayerLivesUpdate?.Invoke(_bluePlayerLives = value);
        }

        public int OrangePlayerLives
        {
            get => _orangePlayerLives;
            private set => OnOrangePlayerLivesUpdate?.Invoke(_orangePlayerLives = value);
        }

        public const float BulletSpeed = 10;
        public const float BulletSpawnHeight = 0.5f;

        void Start()
        {
            _localSettingsProvider = FindObjectOfType<LocalSettingsProvider>();
            var isWantedBlue = _localSettingsProvider.settings.isPreferedColorBlue;

            var properties = PhotonNetwork.CurrentRoom.CustomProperties;
            var bluePlayers = (int) properties["BluePlayers"];
            var orangePlayers = (int) properties["OrangePlayers"];
            if (bluePlayers == 0 && isWantedBlue || orangePlayers > 0 && !isWantedBlue)
            {
                properties["BluePlayers"] = ++bluePlayers;
                BluePlayerLives = 3;
                SpawnPlayer(PlayerColor.Blue);
            }
            else
            {
                properties["OrangePlayers"] = ++orangePlayers;
                OrangePlayerLives = 3;
                SpawnPlayer(PlayerColor.Orange);
            }

            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }

        private void SpawnPlayer(PlayerColor playerColor)
        {
            var isBlue = playerColor == PlayerColor.Blue;

            var prefabName = isBlue
                ? ResourcesPrefabs.BluePlayer
                : ResourcesPrefabs.OrangePlayer;
            var startPosition = isBlue
                ? _blueStartPlayerPosition
                : _orangeStartPlayerPosition;
            var startRotation = isBlue
                ? _blueStartPlayerRotation
                : _orangeStartPlayerRotation;
            PhotonNetwork.Instantiate(prefabName, startPosition, startRotation);
        }

        public void OnEvent(EventData photonEvent)
        {
            switch (photonEvent.Code)
            {
                case GameEvents.BulletShoot:
                    var bulletSpawnData = (BulletSpawnData) photonEvent.CustomData;
                    var bulletPosition = new Vector3(
                        bulletSpawnData.position.x,
                        BulletSpawnHeight,
                        bulletSpawnData.position.y);
                    var bulletRotation = Quaternion.Euler(0, bulletSpawnData.rotation, 0);
                    var bulletInstance = Instantiate(bulletPrefab.gameObject, bulletPosition,
                        bulletRotation);
                    var bullet = bulletInstance.GetComponent<Bullet>();
                    bullet.AddVelocity(BulletSpeed);
                    break;

                case GameEvents.BulletPlayerHit:
                    var bulletHitData = (BulletHitData) photonEvent.CustomData;
                    var playerView = PhotonView.Find(bulletHitData.viewId);
                    var player = playerView.GetComponent<Player>();
                    player.DisplayHit(bulletHitData.direction);
                    switch (player.playerColorType)
                    {
                        case PlayerColor.Blue:
                            BluePlayerLives -= 1;
                            break;
                        case PlayerColor.Orange:
                            OrangePlayerLives -= 1;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
            }
        }
    }
}