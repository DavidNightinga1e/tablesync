using System.Collections.Generic;
using UnityEngine;

namespace TableSync.Demo
{
    public class GameController : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public float bulletSpeed = 10;
        public float reloadTime = 1;
        public float playerSpeed = 2;

        public Dictionary<PlayerColor, float> lastShootTime = new Dictionary<PlayerColor, float>
        {
            {PlayerColor.Blue, 0},
            {PlayerColor.Orange, 0}
        };
        
        public Dictionary<PlayerColor, int> health = new Dictionary<PlayerColor, int>
        {
            {PlayerColor.Blue, 3},
            {PlayerColor.Orange, 3}
        };

        public void Shoot(Player player)
        {
            var currentTime = Time.time;
            var playerType = player.playerColorType;
            if (currentTime > lastShootTime[playerType] + reloadTime)
            {
                var bulletInstance = Instantiate(bulletPrefab, player.bulletSpawnPoint.position, player.bulletSpawnPoint.rotation);
                bulletInstance.GetComponent<Bullet>().AddVelocity(bulletSpeed);
                lastShootTime[playerType] = currentTime;
            }
        }

        public void MovePlayer(Player player, Vector3 inputMove)
        {
            var translate = inputMove * (playerSpeed * Time.deltaTime);
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
                health[playerInput.playerColorType] -= 1;
                playerInput.Hit(collision.impulse);
                if (health[playerInput.playerColorType] == 0)
                    Destroy(playerInput.gameObject);
            }
        }
    }
}