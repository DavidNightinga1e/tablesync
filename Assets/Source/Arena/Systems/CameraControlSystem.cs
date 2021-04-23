using Leopotam.Ecs;
using Source.Arena.Components;
using UnityEngine;

namespace Source.Arena.Systems
{
    public class CameraControlSystem : IEcsInitSystem
    {
        private readonly Camera _mainCamera = null;

        private readonly EcsFilter<PlayerComponent, LocalPlayerTag> _localPlayers = null;

        private readonly Vector3 _bluePlayerCameraPosition = new Vector3(0, 20, -12);
        private readonly Vector3 _orangePlayerCameraPosition = new Vector3(0, 20, 12);
        private readonly Vector3 _bluePlayerCameraRotation = new Vector3(60, 0, 0);
        private readonly Vector3 _orangePlayerCameraRotation = new Vector3(60, 180, 0);

        public void Init()
        {
            ref var player = ref _localPlayers.Get1(0);
            
            _mainCamera.transform.position =
                player.IsBlue ? _bluePlayerCameraPosition : _orangePlayerCameraPosition;
            _mainCamera.transform.rotation =
                Quaternion.Euler(player.IsBlue ? _bluePlayerCameraRotation : _orangePlayerCameraRotation);
        }
    }
}