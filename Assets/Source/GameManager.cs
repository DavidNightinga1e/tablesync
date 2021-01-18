using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace TableSync.Demo
{
    public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        [SerializeField] private Bullet bulletPrefab;

        private readonly Vector3 _blueStartPlayerPosition = new Vector3(4, 0, -4);
        private readonly Vector3 _orangeStartPlayerPosition = new Vector3(-4, 0, 4);
        private readonly Quaternion _blueStartPlayerRotation = Quaternion.Euler(0, 0, 0);
        private readonly Quaternion _orangeStartPlayerRotation = Quaternion.Euler(0, 180, 0);

        private LocalSettingsProvider _localSettingsProvider;

        private const float BulletSpeed = 10;
        private const float BulletSpawnHeight = 0.5f;

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
                SpawnPlayer(PlayerColor.Blue);
            }
            else
            {
                properties["OrangePlayers"] = ++orangePlayers;
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

        public void Shoot(Player player)
        {
            var position = player.bulletSpawnPoint.position;
            var rotation = player.bulletSpawnPoint.rotation.eulerAngles;
            var bulletSpawnData = new BulletSpawnData
            {
                position = new Vector2(position.x, position.z),
                rotation = rotation.y
            };
            PhotonNetwork.RaiseEvent(
                GameControllerEvents.BulletShoot,
                bulletSpawnData,
                new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.All
                },
                SendOptions.SendReliable);
        }

        public void OnEvent(EventData photonEvent)
        {
            switch (photonEvent.Code)
            {
                case GameControllerEvents.BulletShoot:
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
            }
        }
    }
}