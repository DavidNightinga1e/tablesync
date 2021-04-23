using Leopotam.Ecs;
using Photon.Pun;
using Source.Arena.Components;
using Source.Arena.UnityComponents;
using UnityEngine;

namespace Source.Arena.Systems
{
    sealed class PlayerInstanceControlSystem : IEcsInitSystem
    {
        private readonly EcsWorld _world = null;

        private readonly RemotePlayerInstantiationProvider _remotePlayerInstantiationProvider = null;

        public void Init()
        {
            PhotonNetwork.AddCallbackTarget(this);
            if (_remotePlayerInstantiationProvider.remotePlayerBehaviour is null)
                _remotePlayerInstantiationProvider.OnRemotePlayerInstantiated += InitRemote;
            else
                InitRemote();
            InitLocal();
        }

        private void InitLocal()
        {
            var localPlayerEntity = _world.NewEntity();

            localPlayerEntity.Get<PlayerInput>();
            localPlayerEntity.Get<LocalPlayerTag>();
            localPlayerEntity.Get<PlayerAddedEvent>();
            ref var player = ref localPlayerEntity.Get<PlayerComponent>();

            var isBlue = PhotonNetwork.IsMasterClient;
            var spawn = isBlue ? GameRules.BlueSpawn : GameRules.OrangeSpawn;
            var gameObject = PhotonNetwork.Instantiate("Player", spawn, Quaternion.identity);

            player.IsBlue = isBlue;
            player.PlayerBehaviour = gameObject.transform.GetComponent<PlayerBehaviour>();
        }

        private void InitRemote()
        {
            var remotePlayerEntity = _world.NewEntity();

            remotePlayerEntity.Get<RemotePlayerTag>();
            remotePlayerEntity.Get<PlayerAddedEvent>();
            ref var player = ref remotePlayerEntity.Get<PlayerComponent>();

            var remotePlayerBehaviour = _remotePlayerInstantiationProvider.remotePlayerBehaviour;

            player.IsBlue = !PhotonNetwork.IsMasterClient;
            player.PlayerBehaviour = remotePlayerBehaviour;
        }
    }
}