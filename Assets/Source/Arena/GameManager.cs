using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TableSync
{
    public class GameManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        public Canvas canvas;
        public Camera inputCamera;
        public CameraController cameraController;
        public Dictionary<PlayerColor, Player> players = new Dictionary<PlayerColor, Player>(2);
        
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private DialogBox dialogBox;

        private readonly Vector3 _blueStartPlayerPosition = new Vector3(4, 0, -4);
        private readonly Vector3 _orangeStartPlayerPosition = new Vector3(-4, 0, 4);
        private readonly Quaternion _blueStartPlayerRotation = Quaternion.Euler(0, 0, 0);
        private readonly Quaternion _orangeStartPlayerRotation = Quaternion.Euler(0, 180, 0);

        private const double CountdownLength = 2d;

        private bool _isGameRunning;
        private Coroutine _countdownCoroutine;

        public bool IsGameRunning => _isGameRunning;

        private const float BulletSpeed = 10;
        private const float BulletSpawnHeight = 0.5f;

        void Start()
        {
            var isWantedBlue = FindObjectOfType<LocalSettingsProvider>().settings.isPreferedColorBlue;
            var isWantedOrange = !isWantedBlue;

            var roomProperties = CustomRoomPropertiesProvider.Download();

            if (!roomProperties.IsBluePlayerConnected && isWantedBlue ||
                roomProperties.IsOrangePlayerConnected && isWantedOrange)
            {
                roomProperties.IsBluePlayerConnected = true;
                SpawnPlayer(PlayerColor.Blue);
            }
            else
            {
                roomProperties.IsOrangePlayerConnected = true;
                SpawnPlayer(PlayerColor.Orange);
            }

            if (!(roomProperties.IsBluePlayerConnected && roomProperties.IsOrangePlayerConnected))
            {
                ShowWaitingForOthersDialogBox();
            }

            CustomRoomPropertiesProvider.Upload(roomProperties);

            PhotonNetwork.RaiseEvent(
                GameEvents.PlayerJoined,
                null,
                new RaiseEventOptions {Receivers = ReceiverGroup.MasterClient},
                SendOptions.SendReliable);
        }

        private void ShowWaitingForOthersDialogBox()
        {
            dialogBox.RemoveAllListeners();
            dialogBox.CancelButtonVisible = true;
            dialogBox.YesButtonVisible = false;
            dialogBox.NoButtonVisible = false;
            dialogBox.CancelButton.onClick.AddListener(() => PhotonNetwork.LeaveRoom());
            dialogBox.IsVisible = true;
            dialogBox.Text.text = "Waiting for other player to join";
        }

        private void StartCountdown(double gameStartTime)
        {
            _countdownCoroutine = StartCoroutine(CountdownEnumerator(gameStartTime));
        }

        private void StopCountdown()
        {
            StopCoroutine(_countdownCoroutine);
            dialogBox.IsVisible = false;
        }

        private IEnumerator CountdownEnumerator(double gameStartTime)
        {
            dialogBox.RemoveAllListeners();
            dialogBox.CancelButtonVisible = false;
            dialogBox.YesButtonVisible = false;
            dialogBox.NoButtonVisible = false;
            dialogBox.CancelButton.onClick.AddListener(() => PhotonNetwork.LeaveRoom());
            dialogBox.IsVisible = true;

            var timeRemains = gameStartTime - PhotonNetwork.Time;
            while (timeRemains > 0)
            {
                dialogBox.Text.text = $"Starting in {timeRemains = gameStartTime - PhotonNetwork.Time:0.000}";
                yield return null;
            }

            dialogBox.IsVisible = false;
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.RaiseEvent(
                    GameEvents.GameStarted,
                    null,
                    GameEventsUtilities.RaiseEventOptionsReceiversAll,
                    SendOptions.SendReliable);
            }
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
                    ProcessBulletShoot(photonEvent);
                    break;
                case GameEvents.BulletPlayerHit:
                    ProcessBulletHit(photonEvent);
                    break;
                case GameEvents.PlayerJoined:
                    ProcessPlayerJoined(photonEvent);
                    break;
                case GameEvents.CountdownStarted:
                    ProcessCountdownStarted(photonEvent);
                    break;
                case GameEvents.GameStarted:
                    ProcessGameStarted(photonEvent);
                    break;
                case GameEvents.GameEnded:
                    ProcessGameEnded(photonEvent);
                    break;
            }
        }

        private void ProcessGameEnded(EventData photonEvent)
        {
            print("processing game end");
            _isGameRunning = false;
            dialogBox.IsVisible = true;
            dialogBox.RemoveAllListeners();
            dialogBox.YesButtonVisible = true;
            dialogBox.YesButton.onClick.AddListener(() => PhotonNetwork.LeaveRoom());
            dialogBox.NoButtonVisible = dialogBox.CancelButtonVisible = false;
            switch ((GameEndResult) photonEvent.CustomData)
            {
                case GameEndResult.BluePlayerWon:
                    dialogBox.Text.text = "Blue player won!";
                    break;
                case GameEndResult.OrangePlayerWon:
                    dialogBox.Text.text = "Orange player won!";
                    break;
                case GameEndResult.Draw:
                    dialogBox.Text.text = "That's a draw. No one won.";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            dialogBox.Text.text += "\nPress [yes] to leave the match";
        }

        private void ProcessGameStarted(EventData photonEvent)
        {
            StopCountdown();
            _isGameRunning = true;
        }

        private void ProcessCountdownStarted(EventData photonEvent)
        {
            StartCountdown((double) photonEvent.CustomData);
        }

        private void ProcessPlayerJoined(EventData photonEvent)
        {
            var customRoomProperties = CustomRoomPropertiesProvider.Download();
            if (customRoomProperties.IsBluePlayerConnected && customRoomProperties.IsOrangePlayerConnected)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.RaiseEvent(
                    GameEvents.CountdownStarted,
                    PhotonNetwork.Time + CountdownLength,
                    GameEventsUtilities.RaiseEventOptionsReceiversAll,
                    SendOptions.SendReliable);
            }
        }

        private void ProcessBulletHit(EventData photonEvent)
        {
            print($"processing bullet hit {photonEvent.Sender}, {photonEvent.Sender}");
            
            var bulletHitData = (BulletHitEventData) photonEvent.CustomData;
            var playerView = PhotonView.Find(bulletHitData.viewId);
            var player = playerView.GetComponent<Player>();
            player.DisplayHit(bulletHitData.direction);
            switch (player.playerColorType)
            {
                case PlayerColor.Blue:
                    players[PlayerColor.Blue].Lives -= 1;
                    break;
                case PlayerColor.Orange:
                    players[PlayerColor.Orange].Lives -= 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (PhotonNetwork.IsMasterClient)
            {
                if (players[PlayerColor.Blue].Lives < 1)
                {
                    PhotonNetwork.RaiseEvent(
                        GameEvents.GameEnded,
                        GameEndResult.OrangePlayerWon,
                        GameEventsUtilities.RaiseEventOptionsReceiversAll,
                        SendOptions.SendReliable);
                }

                if (players[PlayerColor.Orange].Lives < 1)
                {
                    PhotonNetwork.RaiseEvent(
                        GameEvents.GameEnded,
                        GameEndResult.BluePlayerWon,
                        GameEventsUtilities.RaiseEventOptionsReceiversAll,
                        SendOptions.SendReliable);
                }
            }
        }

        private void ProcessBulletShoot(EventData photonEvent)
        {
            var bulletSpawnData = (BulletSpawnEventData) photonEvent.CustomData;
            var bulletPosition = new Vector3(
                bulletSpawnData.position.x,
                BulletSpawnHeight,
                bulletSpawnData.position.y);
            var bulletRotation = Quaternion.Euler(0, bulletSpawnData.rotation, 0);
            var bulletInstance = Instantiate(
                bulletPrefab.gameObject,
                bulletPosition,
                bulletRotation);
            var bullet = bulletInstance.GetComponent<Bullet>();
            bullet.AddVelocity(BulletSpeed);
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Lobby");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PhotonNetwork.LeaveRoom();
            }
        }
    }
}