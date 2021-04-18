using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace TableSync
{
    public class GameEventCallback : MonoBehaviour, IOnEventCallback
    {
        [SerializeField] private Game game;

        private void Awake()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public void OnEvent(EventData photonEvent)
        {
            switch (photonEvent.Code)
            {
                case GameEvent.BulletShoot:
                    var bulletSpawnEventData = (BulletSpawnEventData) photonEvent.CustomData;
                    var position = bulletSpawnEventData.position;
                    var rotation = bulletSpawnEventData.rotation;
                    game.SpawnBullet(position, rotation);
                    break;
                case GameEvent.BulletPlayerHit:
                    var bulletHitData = (BulletHitEventData) photonEvent.CustomData;
                    var playerView = PhotonView.Find(bulletHitData.viewId);
                    var player = playerView.GetComponent<Player>();
                    var direction = bulletHitData.direction;
                    game.HitPlayer(player, direction);
                    break;
                case GameEvent.PlayerJoined:
                    game.TryStartGame();
                    break;
                case GameEvent.CountdownStarted:
                    var startTime = (double) photonEvent.CustomData;
                    game.StartCountdown(startTime);
                    break;
                case GameEvent.GameStarted:
                    game.StartGame();
                    break;
                case GameEvent.GameEnded:
                    var gameEndResult = (GameEndResult) photonEvent.CustomData;
                    game.EndGame(gameEndResult);
                    break;
            }
        }
    }
}