using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TableSync
{
    public class Game : MonoBehaviour
    {
        public Canvas canvas;
        public Camera inputCamera;
        public CameraController cameraController;
        public Dictionary<PlayerColor, Player> players = new Dictionary<PlayerColor, Player>(2);

        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private DialogBox dialogBox;
        [SerializeField] private InfoDisplay infoDisplay;

        private readonly Vector3 _blueStartPlayerPosition = new Vector3(4, 0, -4);
        private readonly Vector3 _orangeStartPlayerPosition = new Vector3(-4, 0, 4);
        private readonly Quaternion _blueStartPlayerRotation = Quaternion.Euler(0, 0, 0);
        private readonly Quaternion _orangeStartPlayerRotation = Quaternion.Euler(0, 180, 0);

        private const double CountdownLength = 2d;

        private bool _isGameRunning;
        private Coroutine _countdownCoroutine;

        public bool IsGameRunning => _isGameRunning;

        private const float BulletSpeed = 8;
        private const float BulletSpawnHeight = 0.5f;
        private PlayerColor localPlayerColor;
        private PlayerColor RemotePlayerColor => (PlayerColor) (((int) localPlayerColor + 1) % 2);

        void Start()
        {
            var isWantedBlue = FindObjectOfType<LocalSettingsProvider>().settings.isPreferedColorBlue;
            var isWantedOrange = !isWantedBlue;

            var isBluePlayerConnected = CustomRoomProperties.IsBluePlayerConnected;
            var isOrangePlayerConnected = CustomRoomProperties.IsOrangePlayerConnected;

            if (!isBluePlayerConnected && isWantedBlue ||
                isOrangePlayerConnected && isWantedOrange)
            {
                CustomRoomProperties.IsBluePlayerConnected = true;
                localPlayerColor = PlayerColor.Blue;
            }
            else
            {
                CustomRoomProperties.IsOrangePlayerConnected = true;
                localPlayerColor = PlayerColor.Orange;
            }

            SpawnPlayer(localPlayerColor);

            if (!(isBluePlayerConnected && isOrangePlayerConnected))
            {
                ShowWaitingForOthersDialogBox();
            }

            infoDisplay.PlayerName = PhotonNetwork.LocalPlayer.NickName;

            PhotonNetwork.RaiseEvent(
                GameEvent.PlayerJoined,
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
                    GameEvent.GameStarted,
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

        public void EndGame(GameEndResult gameEndResult)
        {
            _isGameRunning = false;
            dialogBox.IsVisible = true;
            dialogBox.RemoveAllListeners();
            dialogBox.YesButtonVisible = true;
            dialogBox.YesButton.onClick.AddListener(() => PhotonNetwork.LeaveRoom());
            dialogBox.NoButtonVisible = dialogBox.CancelButtonVisible = false;
            switch (gameEndResult)
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

        public void StartGame()
        {
            StopCountdown();
            _isGameRunning = true;
        }

        public void StartCountdown(double gameStartTime)
        {
            _countdownCoroutine = StartCoroutine(CountdownEnumerator(gameStartTime));
        }

        public void TryStartGame()
        {
            if (!CustomRoomProperties.IsBluePlayerConnected || !CustomRoomProperties.IsOrangePlayerConnected)
                return;

            var nickName = PhotonNetwork.CurrentRoom.Players.Last().Value.NickName;
            infoDisplay.EnemyName = nickName;

            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.RaiseEvent(
                GameEvent.CountdownStarted,
                PhotonNetwork.Time + CountdownLength,
                GameEventsUtilities.RaiseEventOptionsReceiversAll,
                SendOptions.SendReliable);
        }

        public void HitPlayer(Player player, Vector2 direction)
        {
            player.DisplayHit(direction);
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

            if (!PhotonNetwork.IsMasterClient) return;

            if (players[PlayerColor.Blue].Lives < 1)
            {
                PhotonNetwork.RaiseEvent(
                    GameEvent.GameEnded,
                    GameEndResult.OrangePlayerWon,
                    GameEventsUtilities.RaiseEventOptionsReceiversAll,
                    SendOptions.SendReliable);
            }

            if (players[PlayerColor.Orange].Lives < 1)
            {
                PhotonNetwork.RaiseEvent(
                    GameEvent.GameEnded,
                    GameEndResult.BluePlayerWon,
                    GameEventsUtilities.RaiseEventOptionsReceiversAll,
                    SendOptions.SendReliable);
            }
        }

        public void SpawnBullet(Vector2 position, float rotation)
        {
            var bulletPosition = new Vector3(
                position.x,
                BulletSpawnHeight,
                position.y);
            var bulletRotation = Quaternion.Euler(0, rotation, 0);
            var bulletInstance = Instantiate(
                bulletPrefab.gameObject,
                bulletPosition,
                bulletRotation);
            var bullet = bulletInstance.GetComponent<Bullet>();
            bullet.AddVelocity(BulletSpeed);
        }
    }
}