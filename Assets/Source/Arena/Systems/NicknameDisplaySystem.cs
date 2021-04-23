using Leopotam.Ecs;
using Source.Arena.Components;

namespace Source.Arena.Systems
{
    public class NicknameDisplaySystem : IEcsRunSystem
    {
        private readonly EcsFilter<PlayerComponent, LocalPlayerTag, PlayerAddedEvent> _localPlayer = null;
        private readonly EcsFilter<PlayerComponent, RemotePlayerTag, PlayerAddedEvent> _remotePlayer = null;

        private readonly NicknameDisplay _nicknameDisplay = null;

        public void Run()
        {
            if (!_localPlayer.IsEmpty())
            {
                ref var playerComponent = ref _localPlayer.Get1(0);
                var nickname = playerComponent.PlayerBehaviour.photonView.Owner.NickName;
                _nicknameDisplay.Local = nickname;
            }

            if (!_remotePlayer.IsEmpty())
            {
                ref var playerComponent = ref _remotePlayer.Get1(0);
                var nickname = playerComponent.PlayerBehaviour.photonView.Owner.NickName;
                _nicknameDisplay.Remote = nickname;
            }
        }
    }
}