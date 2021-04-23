using Leopotam.Ecs;
using Source.Arena.Components;
using UnityEngine;

namespace Source.Arena.Systems
{
    public class PlayerMovementSystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerComponent, PlayerInput, LocalPlayerTag> _localPlayer = null;

        private const float Speed = 3f;

        public void Run()
        {
            ref var player = ref _localPlayer.Get1(0);
            ref var playerInput = ref _localPlayer.Get2(0);

            var inputMove = playerInput.Movement;
            var inputLook = playerInput.LookPosition;
            if (!player.IsBlue)
                inputMove = -inputMove; // map is upside-down for orange
            var move = new Vector3(inputMove.x, 0, inputMove.y); // todo: make ext method for converting v2 to v3
            var look = new Vector3(inputLook.x, 0, inputLook.y);
            player.PlayerBehaviour.characterController.Move(move * Speed);
            player.PlayerBehaviour.transform.LookAt(look);
        }
    }
}