using Leopotam.Ecs;
using Source.Arena.Components;
using UnityEngine;

namespace Source.Arena.Systems
{
    sealed class UserInputSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = null;

        private readonly EcsFilter<LocalPlayerTag> _localPlayers = null;

        private readonly Camera _camera = null;

        void IEcsRunSystem.Run()
        {
            foreach (var i in _localPlayers)
            {
                var entity = _localPlayers.GetEntity(i);
                ref var playerInput = ref entity.Get<PlayerInput>();

                playerInput.Fire = Input.GetMouseButton(0);

                playerInput.Movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized *
                                       Time.deltaTime;

                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                var origin = ray.origin;
                var direction = ray.direction;
                var directionMultiplier = -origin.y / direction.y;
                var xPos = origin.x + directionMultiplier * direction.x;
                var zPos = origin.z + directionMultiplier * direction.z;

                playerInput.LookPosition = new Vector2(xPos, zPos);
            }
        }
    }
}