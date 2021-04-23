using System;
using ExitGames.Client.Photon;
using Leopotam.Ecs;
using Photon.Pun;
using Photon.Realtime;
using Simba;
using Source.Arena.Components;
using Source.Arena.UnityComponents;
using TableSync;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.Arena
{
    public class PlayerShootSystem : IEcsInitSystem, IEcsRunSystem, IOnDestroySystem, IOnEventCallback
    {
        private const double Cooldown = 0.150d;
        private const float BulletSpawnHeight = 0.2f;
        private const float BulletSpawnDistance = 0.7f;

        private readonly EcsFilter<PlayerComponent, PlayerInput, LocalPlayerTag>.Exclude<PlayerShootCooldown>
            _localPlayer = null;

        private readonly EcsFilter<PlayerComponent, RemotePlayerTag> _remotePlayer = null;

        private readonly EcsFilter<PlayerComponent, PlayerAddedEvent> _addedPlayer = null;

        private readonly PrefabProvider _prefabProvider = null;

        public void Init()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public void Run()
        {
            foreach (var i in _addedPlayer)
            {
                var playerComponent = _addedPlayer.Get1(i);
                playerComponent.PlayerBehaviour.OnHit += () => PlayerBehaviourOnOnHit(playerComponent);
            }

            if (_localPlayer.IsEmpty())
                return;

            ref var input = ref _localPlayer.Get2(0);
            ref var player = ref _localPlayer.Get1(0);

            if (!input.Fire) return;

            var entity = _localPlayer.GetEntity(0);
            ref var cooldown = ref entity.Get<PlayerShootCooldown>();
            cooldown.TimeLeft = Cooldown;

            var playerBehaviour = player.PlayerBehaviour;
            Object.Instantiate(
                _prefabProvider.bullet,
                playerBehaviour.transform.position
                + playerBehaviour.transform.forward * BulletSpawnDistance
                + Vector3.up * BulletSpawnHeight,
                playerBehaviour.transform.rotation);

            PhotonNetwork.RaiseEvent(GameEvent.BulletShoot, null,
                new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others
                }, SendOptions.SendReliable);
        }

        private void PlayerBehaviourOnOnHit(PlayerComponent playerComponent)
        {
            playerComponent.Health -= 1;
            Debug.Log($"{(playerComponent.IsBlue ? "blue" : "orange")} - {playerComponent.Health}");
        }

        public void OnEvent(EventData photonEvent)
        {
            switch (photonEvent.Code)
            {
                case GameEvent.BulletShoot:
                    if (photonEvent.Sender == PhotonNetwork.LocalPlayer.ActorNumber)
                        throw new ApplicationException(nameof(photonEvent.Sender) + " sender cannot be local player ");

                    ref var playerComponent = ref _remotePlayer.Get1(0);
                    var playerBehaviour = playerComponent.PlayerBehaviour;
                    Object.Instantiate(
                        _prefabProvider.bullet,
                        playerBehaviour.transform.position + playerBehaviour.transform.forward +
                        Vector3.up * BulletSpawnHeight,
                        playerBehaviour.transform.rotation);
                    break;
            }
        }

        public void OnDestroy()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }
    }
}