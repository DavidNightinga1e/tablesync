using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TableSync.Demo
{
    public class GameController : MonoBehaviour
    {
        #region SceneReferences
        private Player _bluePlayer;
        private Player _orangePlayer;
        #endregion

        #region Prefabs
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Player bluePlayerPrefab;
        [SerializeField] private Player orangePlayerPrefab;
        #endregion

        #region GameRuleValues
        private const float BulletSpeed = 10;
        private const float ReloadTime = 1;
        private const float PlayerSpeed = 2;
        private const int MaxLives = 3;
        
        private readonly Vector3 _blueStartPlayerPosition = new Vector3(4, 0, -4);
        private readonly Vector3 _orangeStartPlayerPosition = new Vector3(-4, 0, 4);
        private readonly Quaternion _blueStartPlayerRotation = Quaternion.Euler(0, 0, 0);
        private readonly Quaternion _orangeStartPlayerRotation = Quaternion.Euler(0, 180, 0);
        #endregion
        

        private readonly Dictionary<PlayerColor, float> _lastShootTime = new Dictionary<PlayerColor, float>
        {
            {PlayerColor.Blue, 0},
            {PlayerColor.Orange, 0}
        };

        private readonly Dictionary<PlayerColor, int> _health = new Dictionary<PlayerColor, int>
        {
            {PlayerColor.Blue, MaxLives},
            {PlayerColor.Orange, MaxLives}
        };

        private readonly List<Bullet> _bullets = new List<Bullet>(10);
        
        public void ResetGame()
        {
            Destroy(_bluePlayer.gameObject);
            _bluePlayer = 
                Instantiate(bluePlayerPrefab, _blueStartPlayerPosition, _blueStartPlayerRotation)
                .GetComponent<Player>();
            Destroy(_orangePlayer.gameObject);
            _orangePlayer = 
                Instantiate(orangePlayerPrefab, _orangeStartPlayerPosition, _orangeStartPlayerRotation)
                .GetComponent<Player>();
            _lastShootTime[PlayerColor.Blue] = _lastShootTime[PlayerColor.Orange] = -ReloadTime;
            _health[PlayerColor.Blue] = _health[PlayerColor.Orange] = MaxLives;
            foreach (var bullet in _bullets) 
                Destroy(bullet.gameObject);
            _bullets.Clear();
            
        }

        public void Shoot(Player player)
        {
            var currentTime = Time.time;
            var playerType = player.playerColorType;
            if (currentTime > _lastShootTime[playerType] + ReloadTime)
            {
                var bulletInstance = Instantiate(bulletPrefab, player.bulletSpawnPoint.position, player.bulletSpawnPoint.rotation);
                var bullet = bulletInstance.GetComponent<Bullet>();
                bullet.AddVelocity(BulletSpeed);
                _bullets.Add(bullet);
                _lastShootTime[playerType] = currentTime;
            }
        }

        public void MovePlayer(Player player, Vector3 inputMove)
        {
            var translate = inputMove * (PlayerSpeed * Time.deltaTime);
            player.characterController.Move(translate);
        }

        public void RotatePlayer(Player player, Ray ray)
        {
            var origin = ray.origin;
            var direction = ray.direction;
            var directionMultiplier = -origin.y / direction.y;
            var xPos = origin.x + directionMultiplier * direction.x;
            var zPos = origin.z + directionMultiplier * direction.z;
            var targetLook = new Vector3(xPos, 0, zPos);
            player.transform.LookAt(targetLook);
        }

        public void Hit(Collision collision)
        {
            var playerInput = collision.transform.GetComponent<Player>();
            if (playerInput)
            {
                _health[playerInput.playerColorType] -= 1;
                playerInput.Hit(collision.impulse);
                if (_health[playerInput.playerColorType] == 0)
                    Destroy(playerInput.gameObject);
            }
        }
    }
}